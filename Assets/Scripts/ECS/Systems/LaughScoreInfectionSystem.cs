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
        private ComponentLookup<Fur> crowdPersonLookup;
        protected override void OnCreate()
        {
            RequireForUpdate<SimulationSingleton>();
            RequireForUpdate<PhysicsWorldSingleton>();
            overlapResults = new NativeList<DistanceHit>(Allocator.Persistent);
            crowdPersonLookup = GetComponentLookup<Fur>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            overlapResults.Dispose();
        }

        protected override void OnUpdate()
        {
            crowdPersonLookup.Update(this);
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            Dependency = new CollisionEventJob()
            {
                crowdPersonLookup = crowdPersonLookup,
                time = SystemAPI.Time.ElapsedTime
            }.Schedule(simulation, Dependency);
        }

        public struct CollisionEventJob : ICollisionEventsJob
        {
            public ComponentLookup<Fur> crowdPersonLookup;
            public double time;
            public void Execute(CollisionEvent collisionEvent)
            {
                if (crowdPersonLookup.HasComponent(collisionEvent.EntityA) is false)
                    return;
                if (crowdPersonLookup.HasComponent(collisionEvent.EntityB) is false)
                    return;

                RefRW<Fur> crowdPersonA = crowdPersonLookup.GetRefRW(collisionEvent.EntityA);
                RefRW<Fur> crowdPersonB = crowdPersonLookup.GetRefRW(collisionEvent.EntityB);

                if (crowdPersonA.ValueRO.CanInfect(time))
                {
                    crowdPersonA.ValueRW.LastInfectionTime = time;
                    if (crowdPersonB.ValueRO.CanBeInfected())
                    {
                        crowdPersonB.ValueRW.LaughScore += 0.1f;
                    }
                }
                if (crowdPersonB.ValueRO.CanInfect(time))
                {
                    crowdPersonB.ValueRW.LastInfectionTime = time;
                    if (crowdPersonA.ValueRO.CanBeInfected())
                    {
                        crowdPersonA.ValueRW.LaughScore += 0.1f;
                    }
                }
            }
        }
    }
}