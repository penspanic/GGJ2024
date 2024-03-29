using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems
{
    public partial class GrabbedMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<Grabbed>();
            EntityManager.CreateSingleton<CatFurStatus>();
        }

        protected override void OnUpdate()
        {
            bool isGrabbed = false;
            //using var ecb = new EntityCommandBuffer(Allocator.Temp);
            // Grabbed된 Fur 들을 마우스 포지션에 따라 이동시킨다.

            // 털
            foreach ((Grabbed grabbed, RefRW<LocalTransform> localTransformRW, Entity entity) in SystemAPI.Query<Grabbed, RefRW<LocalTransform>>()
                         .WithNone<SmallCat>().WithEntityAccess())
            {
                var mousePosition = Input.mousePosition;
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.x -= grabbed.offset.x;
                worldPosition.y -= grabbed.offset.y;
                worldPosition.z = 0f;
                localTransformRW.ValueRW.Position = worldPosition;
                isGrabbed = true;
            }

            // 관중
            foreach ((Grabbed grabbed, RefRW<LocalTransform> localTransformRW, Entity entity) in SystemAPI.Query<Grabbed, RefRW<LocalTransform>>()
                         .WithAll<SmallCat>().WithEntityAccess())
            {
                var mousePosition = Input.mousePosition;
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                worldPosition.x -= grabbed.offset.x;
                worldPosition.y -= grabbed.offset.y;
                worldPosition.z = SmallCat.ZPosition;
                localTransformRW.ValueRW.Position = worldPosition;
                isGrabbed = true;
            }

            if (isGrabbed)
            {
                var catFurStatus = SystemAPI.GetSingleton<CatFurStatus>();
                catFurStatus.LastTouchTime = UnityEngine.Time.time;
                SystemAPI.SetSingleton(catFurStatus);
            }
        }
    }
}