using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial class GrabCrowdSystem : SystemBase
{
    private NativeList<DistanceHit> overlapResults;
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<SimulationSingleton>();
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
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        float3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) is false)
            return;

        worldPosition.z = 0;
        overlapResults.Clear();
        if (physicsWorldSingleton.CollisionWorld.OverlapSphere(worldPosition, radius: 0.35f, ref overlapResults, CollisionFilter.Default) is false)
            return;

        Debug.Log("Grabbed " + overlapResults.Length + " entities");
        using var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (DistanceHit distanceHit in overlapResults)
        {
            var collider = physicsWorldSingleton.CollisionWorld.Bodies[distanceHit.RigidBodyIndex].Collider;
            // check if collider tag is "Fur"
            if (collider.Value.Value != 0x46757200)
                continue;

            ecb.AddComponent<Grabbed>(distanceHit.Entity, new Grabbed()
            {
                offset = worldPosition - EntityManager.GetComponentData<LocalTransform>(distanceHit.Entity).Position
            });
        }

        ecb.Playback(EntityManager);
    }
}