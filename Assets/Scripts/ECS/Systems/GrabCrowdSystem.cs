using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
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
        if (physicsWorldSingleton.CollisionWorld.OverlapSphere(worldPosition, radius: 0.5f, ref overlapResults, CollisionFilter.Default) is false)
            return;

        Debug.Log("Grabbed " + overlapResults.Length + " entities");
        using var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (DistanceHit distanceHit in overlapResults)
        {
            ecb.AddComponent<Grabbed>(distanceHit.Entity, new Grabbed()
            {
                offset = worldPosition - EntityManager.GetComponentData<LocalTransform>(distanceHit.Entity).Position
            });
            var rigidBodyAspect = SystemAPI.GetAspect<RigidBodyAspect>(distanceHit.Entity);
            rigidBodyAspect.IsKinematic = true;
        }

        ecb.Playback(EntityManager);
    }
}