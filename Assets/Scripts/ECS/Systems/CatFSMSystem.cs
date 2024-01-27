using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS.Systems
{
    public partial class CatFSMSystem : SystemBase
    {
        private static readonly Dictionary<CatStateType, float> probabilities = new()
        {
            { CatStateType.Idle, 0.5f },
            { CatStateType.Walk, 0.5f },
            // { CatStateType.Jump, 0.2f }
        };

        protected override void OnCreate()
        {
            RequireForUpdate<CrowdSettings>();
        }

        protected override void OnUpdate()
        {
            double time = SystemAPI.Time.ElapsedTime;
            var settings = SystemAPI.GetSingleton<CrowdSettings>();
            var changed = new NativeHashMap<Entity, CatStateType>(1000, Allocator.Temp);
            foreach ((SmallCat smallCat, RefRW<CatState> catStateRW, Entity entity) in
                     SystemAPI.Query<SmallCat, RefRW<CatState>>().WithNone<Grabbed>().WithEntityAccess())
            {
                var state = catStateRW.ValueRO;
                if (state.StateEndTime != 0 && state.StateEndTime > time)
                    continue;

                var stateType = SelectNextState(settings, time, smallCat.LaughScore, state.State, out double endTime);
                catStateRW.ValueRW.State = stateType;
                catStateRW.ValueRW.StateEndTime = endTime;
                changed[entity] = stateType;
            }

            if (changed.IsEmpty)
                return;

            using var ecb = new EntityCommandBuffer(Allocator.Temp);
            using var enumerator = changed.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Entity entity = enumerator.Current.Key;
                CatStateType state = enumerator.Current.Value;
                var startPos = SystemAPI.GetComponent<LocalTransform>(entity).Position.xy;
                var rigidBodyAspect = SystemAPI.GetAspect<RigidBodyAspect>(entity);
                rigidBodyAspect.LinearVelocity = float3.zero;
                var stateRW = SystemAPI.GetComponentRW<CatState>(entity);
                stateRW.ValueRW.StateStartTime = time;
                switch (state)
                {
                    case CatStateType.Ready:
                    case CatStateType.Stop:
                    {
                        rigidBodyAspect.IsKinematic = false;
                        ecb.RemoveComponent<CatIdle>(entity);
                        ecb.RemoveComponent<CatWalk>(entity);
                        ecb.RemoveComponent<CatJump>(entity);
                        break;
                    }
                    case CatStateType.Idle:
                        rigidBodyAspect.IsKinematic = false;
                        ecb.AddComponent(entity, new CatIdle()
                        {
                            startPosition = startPos,
                            speed = Random.Range(settings.IdleStatusSpeedRange.x, settings.IdleStatusSpeedRange.y),
                            amplitude = Random.Range(settings.IdleStatusAmplitudeRange.x, settings.IdleStatusAmplitudeRange.y),
                            reverse = Random.value < 0.5f
                        });
                        ecb.RemoveComponent<CatWalk>(entity);
                        ecb.RemoveComponent<CatJump>(entity);
                        break;
                    case CatStateType.Walk:
                        rigidBodyAspect.IsKinematic = true;
                        // direction : random 0~360 degree direction
                        ecb.AddComponent(entity, new CatWalk()
                        {
                            directionWithSpeed = Random.insideUnitCircle.normalized *
                                                 Random.Range(settings.MoveStatusSpeedRange.x, settings.MoveStatusSpeedRange.y)
                        });
                        ecb.RemoveComponent<CatIdle>(entity);
                        ecb.RemoveComponent<CatJump>(entity);
                        break;
                    case CatStateType.Jump:
                        ecb.AddComponent(entity, new CatJump()
                        {
                            startPosition = startPos,
                            targetPosition = startPos + new float2(Random.Range(-2, 2), Random.Range(-2, 2)),
                            jumpHeight = Random.Range(0.5f, 1.5f)
                        });
                        ecb.RemoveComponent<CatIdle>(entity);
                        ecb.RemoveComponent<CatWalk>(entity);
                        break;
                }
            }
            ecb.Playback(EntityManager);
        }

        private static CatStateType SelectNextState(in CrowdSettings settings, double time, float laughScore, CatStateType currentState, out double endTime)
        {
            if (laughScore < 0.5f)
            {
                endTime = time + Random.Range(0.2f, 1f);
                return CatStateType.Idle;
            }

            if (currentState == CatStateType.Unknown)
            {
                endTime = time + Random.Range(0.5f, 3f);
                return CatStateType.Ready;
            }
            if (currentState == CatStateType.Idle ||
                currentState == CatStateType.Stop)
            {
                endTime = time + Random.Range(settings.MoveStatusDurationRange.x, settings.MoveStatusDurationRange.y);
                return CatStateType.Walk;
            }
            if (currentState == CatStateType.Walk)
            {
                endTime = time + Random.Range(0.5f, 1.5f);
                return CatStateType.Stop;
            }

            throw new NotImplementedException($"{currentState} not handled!");
        }
    }
}