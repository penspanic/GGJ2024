using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public partial class CatFSMSystem : SystemBase
    {
        private static readonly Dictionary<CatStateType, float> probabilities = new()
        {
            { CatStateType.Idle, 0.5f },
            { CatStateType.Walk, 0.4f },
            { CatStateType.Jump, 0.1f }
        };

        protected override void OnUpdate()
        {
            double time = SystemAPI.Time.ElapsedTime;
            var changed = new NativeHashMap<Entity, CatStateType>(1000, Allocator.Temp);
            foreach ((SmallCat smallCat, RefRW<CatState> catStateRW, Entity entity) in SystemAPI.Query<SmallCat, RefRW<CatState>>().WithEntityAccess())
            {
                var state = catStateRW.ValueRO;
                if (state.StateEndTime < time)
                    continue;

                var stateType = SelectNextState(time, state.State, out double endTime);
                catStateRW.ValueRW.State = stateType;
                catStateRW.ValueRW.StateEndTime = endTime;
                changed[entity] = stateType;
            }

            if (changed.IsEmpty)
                return;

            using var ecb = new EntityCommandBuffer(Allocator.Temp);
            // foreach ((Entity entity, CatStateType state) in changed.GetKeyValueArrays(Allocator.Temp))
            // {
            //     switch (state)
            //     {
            //         case CatStateType.Idle:
            //             ecb.AddComponent<CatIdle>(entity);
            //             ecb.RemoveComponent<CatWalk>(entity);
            //             ecb.RemoveComponent<CatJump>(entity);
            //             break;
            //         case CatStateType.Walk:
            //             ecb.AddComponent<CatWalk>(entity);
            //             ecb.RemoveComponent<CatIdle>(entity);
            //             ecb.RemoveComponent<CatJump>(entity);
            //             break;
            //         case CatStateType.Jump:
            //             ecb.AddComponent<CatJump>(entity);
            //             ecb.RemoveComponent<CatIdle>(entity);
            //             ecb.RemoveComponent<CatWalk>(entity);
            //             break;
            //     }
            // }
        }

        private static CatStateType SelectNextState(double time, CatStateType currentState, out double endTime)
        {
            // select state by probabilities
            float random = Random.value;
            float sum = 0;
            CatStateType state = CatStateType.Unknown;
            foreach ((CatStateType eachState, float probability) in probabilities)
            {
                sum += probability;
                if (random < sum)
                {
                    endTime = time + Random.Range(1f, 3f);
                    state = eachState;
                    break;
                }
            }

            if (state is CatStateType.Unknown)
                state = CatStateType.Idle;

            endTime = time + Random.Range(1f, 3f);
            return state;
        }
    }
}