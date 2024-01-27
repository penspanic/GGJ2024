using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class RodMovingSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<Rod>();
    }

    protected override void OnUpdate()
    {
        foreach (RefRW<LocalTransform> transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Rod>())
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            transform.ValueRW.Position = worldPos;
        }
    }
}