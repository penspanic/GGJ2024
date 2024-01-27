using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace ECS.Systems
{
    public partial class LaughScoreInfectionSystem : SystemBase
    {
        private NativeList<DistanceHit> overlapResults;
        protected override void OnCreate()
        {
            RequireForUpdate<PhysicsWorldSingleton>();
            overlapResults = new NativeList<DistanceHit>(Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            overlapResults.Dispose();
        }

        protected override void OnUpdate()
        {
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            NativeHashSet<Entity> overlapEntities = new NativeHashSet<Entity>(100, Allocator.Temp);
            foreach ((CrowdPerson crowdPerson, RefRO<LocalTransform> localTransform, Entity entity) in SystemAPI.Query<CrowdPerson, RefRO<LocalTransform>>().WithEntityAccess())
            {
                if (crowdPerson.CanInfect() is false)
                    return;

                float3 position = localTransform.ValueRO.Position;
                overlapResults.Clear();
                physicsWorld.OverlapSphere(position, radius: 1f, ref overlapResults, CollisionFilter.Default);
                foreach (DistanceHit distanceHit in overlapResults)
                {
                    overlapEntities.Add(distanceHit.Entity);
                }
            }

            foreach (Entity entity in overlapEntities)
            {
                if (EntityManager.HasComponent<CrowdPerson>(entity) is false)
                    continue;
                
                var crowdPersonRW = SystemAPI.GetComponentRW<CrowdPerson>(entity);
                if (crowdPersonRW.ValueRW.CanBeInfected() is false)
                    continue;

                crowdPersonRW.ValueRW.LaughScore += 0.1f;
            }
        }
    }
}