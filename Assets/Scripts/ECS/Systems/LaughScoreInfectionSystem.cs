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
        private ComponentLookup<SmallCat> catLookup;
        protected override void OnCreate()
        {
            RequireForUpdate<SimulationSingleton>();
            RequireForUpdate<PhysicsWorldSingleton>();
            RequireForUpdate<CrowdSettings>();
            overlapResults = new NativeList<DistanceHit>(Allocator.Persistent);
            catLookup = GetComponentLookup<SmallCat>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            overlapResults.Dispose();
        }

        protected override void OnUpdate()
        {
            catLookup.Update(this);
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            Dependency = new CollisionEventJob()
            {
                catLookup = catLookup,
                settings = SystemAPI.GetSingleton<CrowdSettings>(),
                time = SystemAPI.Time.ElapsedTime
            }.Schedule(simulation, Dependency);
        }

        public struct CollisionEventJob : ICollisionEventsJob
        {
            public ComponentLookup<SmallCat> catLookup;
            public CrowdSettings settings;

            public double time;
            public void Execute(CollisionEvent collisionEvent)
            {
                if (catLookup.HasComponent(collisionEvent.EntityA) is false)
                    return;
                if (catLookup.HasComponent(collisionEvent.EntityB) is false)
                    return;

                RefRW<SmallCat> crowdPersonA = catLookup.GetRefRW(collisionEvent.EntityA);
                RefRW<SmallCat> crowdPersonB = catLookup.GetRefRW(collisionEvent.EntityB);

                if (crowdPersonA.ValueRO.CanInfect(time, settings.CanInfectThreshold))
                {
                    crowdPersonA.ValueRW.LastInfectionTime = time;
                    if (crowdPersonB.ValueRO.LaughScore < settings.BeInfectedThreshold)
                    {
                        crowdPersonB.ValueRW.LaughScore += 0.1f;
                    }
                }
                if (crowdPersonB.ValueRO.CanInfect(time, settings.CanInfectThreshold))
                {
                    crowdPersonB.ValueRW.LastInfectionTime = time;
                    if (crowdPersonA.ValueRO.LaughScore < settings.BeInfectedThreshold)
                    {
                        crowdPersonA.ValueRW.LaughScore += 0.1f;
                    }
                }
            }
        }
    }
}