using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace ECS.Systems
{
    public partial class LaughInjectionSystem : SystemBase
    {
        private NativeList<DistanceHit> overlapResults;
        private EntityArchetype effectReqArcheType;
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<PhysicsWorldSingleton>();
            RequireForUpdate<CrowdSettings>();
            overlapResults = new NativeList<DistanceHit>(Allocator.Persistent);
            effectReqArcheType = EntityManager.CreateArchetype(ComponentType.ReadOnly<LolEffectRequest>());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            overlapResults.Dispose();
        }

        protected override void OnUpdate()
        {
            // inject laugh score to mouse position overlapped entities
            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            float3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(1) is false)
                return;

            worldPosition.z = SmallCat.ZPosition;
            overlapResults.Clear();
            var collisionFilter = new CollisionFilter()
            {
                BelongsTo = 1 << 2,
                CollidesWith = 1 << 2,
            };
            if (physicsWorldSingleton.CollisionWorld.OverlapSphere(worldPosition, radius: 0.2f, ref overlapResults, collisionFilter) is false)
                return;

            Debug.Log("Injected laugh score to " + overlapResults.Length + " entities");
            using var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (DistanceHit distanceHit in overlapResults)
            {
                var smallCat = EntityManager.GetComponentData<SmallCat>(distanceHit.Entity);
                smallCat.LaughScore = math.min(smallCat.LaughScore + 0.1f, 1f);
                ecb.SetComponent(distanceHit.Entity, smallCat);
                var effectReq = ecb.CreateEntity(effectReqArcheType);
                ecb.SetComponent(effectReq, new LolEffectRequest { targetPosition = distanceHit.Position });
            }
            ecb.Playback(EntityManager);
        }
    }
}