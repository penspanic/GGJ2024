using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems
{
    public partial class CatStateProcessingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach ((CatState catState, CatIdle catIdle, RefRW<LocalTransform> transformRW, Entity entity) in SystemAPI.Query<CatState, CatIdle, RefRW<LocalTransform>>().WithEntityAccess())
            {
                // move sin graph
                var time = SystemAPI.Time.ElapsedTime - catState.StateStartTime;
                var pos = catIdle.startPosition + new float2(0, math.sin((float)time * CatIdle.Speed) * CatIdle.Amplitude);
                //transformRW.ValueRW.Position = new float3(pos.x, pos.y, 0);
            }

            foreach ((CatState catState, CatWalk catWalk, RefRW<LocalTransform> transformRW, Entity entity) in SystemAPI.Query<CatState, CatWalk, RefRW<LocalTransform>>().WithEntityAccess())
            {
                // move direction
                float stateElapsed = (float)(SystemAPI.Time.ElapsedTime - catState.StateStartTime);
                float stateTotalTime = (float)catState.StateDuration;
                float speedFactor = 1 + math.sin(math.lerp(0, math.PI, stateElapsed / stateTotalTime)) * 2;
                var delta = catWalk.direction * SystemAPI.Time.DeltaTime * CatWalk.Speed * speedFactor;
                transformRW.ValueRW.Position += new float3(delta, 0);
            }

            foreach ((CatState catState, CatJump catJump, RefRW<LocalTransform> transformRW, Entity entity) in SystemAPI.Query<CatState, CatJump, RefRW<LocalTransform>>().WithEntityAccess())
            {
                float stateElapsed = (float)(SystemAPI.Time.ElapsedTime - catState.StateStartTime);
                float posX = math.lerp(catJump.startPosition.x, catJump.targetPosition.x, stateElapsed / catState.StateDuration);
                float posY = catJump.startPosition.y + catJump.jumpHeight * math.sin(math.lerp(0, math.PI, stateElapsed / catState.StateDuration));
                transformRW.ValueRW.Position = new float3(posX, posY, 0);
            }
        }
    }
}