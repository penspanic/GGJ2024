using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;

namespace ECS.Systems
{
    public partial class CatStateProcessingSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<PhysicsWorldSingleton>();
            RequireForUpdate<SmallCatZone>();
            RequireForUpdate<CrowdSettings>();
        }

        protected override void OnUpdate()
        {
            var smallCatZone = SystemAPI.GetSingleton<SmallCatZone>();
            var settings = SystemAPI.GetSingleton<CrowdSettings>();
            foreach ((CatState catState, CatIdle catIdle, RefRW<LocalTransform> transformRW, Entity entity)
                     in SystemAPI.Query<CatState, CatIdle, RefRW<LocalTransform>>().WithNone<Grabbed>().WithEntityAccess())
            {
                // move sin graph
                var time = SystemAPI.Time.ElapsedTime - catState.StateStartTime;
                var pos = catIdle.startPosition + new float2(0, math.sin((float)time * catIdle.speed) * catIdle.amplitude * (catIdle.reverse ? -1 : 1));
                var newPos = new float3(pos.x, pos.y, 0);
                if (smallCatZone.aabb.Contains(newPos) is false)
                    continue;

                transformRW.ValueRW.Position = newPos;
            }

            foreach ((CatState catState, CatWalk catWalk, RefRW<LocalTransform> transformRW, Entity entity)
                     in SystemAPI.Query<CatState, CatWalk, RefRW<LocalTransform>>().WithNone<Grabbed>().WithEntityAccess())
            {
                // move direction
                float stateElapsed = (float)(SystemAPI.Time.ElapsedTime - catState.StateStartTime);
                float stateTotalTime = (float)catState.StateDuration;
                float speedFactor = 1 + math.sin(math.lerp(0, math.PI, stateElapsed / stateTotalTime)) * 2;
                float2 delta = catWalk.directionWithSpeed * SystemAPI.Time.DeltaTime * speedFactor;
                var newPosition = transformRW.ValueRW.Position + new float3(delta, 0);
                if (smallCatZone.aabb.Contains(newPosition) is false)
                    continue;

                transformRW.ValueRW.Position = newPosition;
            }

            foreach ((CatState catState, CatJump catJump, RefRW<LocalTransform> transformRW, Entity entity)
                     in SystemAPI.Query<CatState, CatJump, RefRW<LocalTransform>>().WithNone<Grabbed>().WithEntityAccess())
            {
                float stateElapsed = (float)(SystemAPI.Time.ElapsedTime - catState.StateStartTime);
                float posX = math.lerp(catJump.startPosition.x, catJump.targetPosition.x, stateElapsed / catState.StateDuration);
                float posY = catJump.startPosition.y + catJump.jumpHeight * math.sin(math.lerp(0, math.PI, stateElapsed / catState.StateDuration));
                var newPos = new float3(posX, posY, 0);
                if (smallCatZone.aabb.Contains(newPos) is false)
                    continue;

                transformRW.ValueRW.Position = new float3(posX, posY, 0);
            }
        }
    }
}