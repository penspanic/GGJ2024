using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class DestroyOutOfScreenSystem : SystemBase
{
    private float margin;
    protected override void OnCreate()
    {
        RequireForUpdate<MustInScreen>();
    }

    protected override void OnUpdate()
    {
        var camera = Camera.main;
        if (camera == null)
            return;

        var rect = camera.rect;
        var aabb = new AABB()
        {
            Center = float3.zero,
            Extents = new float3(camera.orthographicSize * camera.aspect * rect.width, camera.orthographicSize * rect.height, 10) + new float3(margin)
        };
        var ecb = new EntityCommandBuffer(Allocator.Temp); 
        foreach ((RefRW<LocalTransform> localTransform, Entity entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<MustInScreen>().WithEntityAccess())
        {
            if (!aabb.Contains(localTransform.ValueRO.Position))
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(EntityManager);
    }
}