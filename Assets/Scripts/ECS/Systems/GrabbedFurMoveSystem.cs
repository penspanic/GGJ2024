using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    public partial class GrabbedFurMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<Grabbed>();
        }

        protected override void OnUpdate()
        {
            //using var ecb = new EntityCommandBuffer(Allocator.Temp);
            // Grabbed된 Fur 들을 마우스 포지션에 따라 이동시킨다.
            foreach ((Grabbed grabbed, RefRW<LocalTransform> localTransformRW, Entity entity) in SystemAPI.Query<Grabbed, RefRW<LocalTransform>>().WithEntityAccess())
            {
                var mousePosition = Input.mousePosition;
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.z = 0f;
                localTransformRW.ValueRW.Position = new float3(worldPosition) - grabbed.offset;
            }
        }
    }
}