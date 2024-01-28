using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Random = UnityEngine.Random;

public struct LolEffectRequest : IComponentData
{
    public float3 targetPosition;
}

public partial class CrowdEffectSystem : SystemBase
{
    private EntityQuery requestQuery;
    protected override void OnCreate()
    {
        requestQuery = GetEntityQuery(typeof(LolEffectRequest));
        //poolQuery = GetEntityQuery(typeof(LolEffectPrefab));
        RequireForUpdate<LolEffectPrefab>();
        RequireForUpdate<LolEffectRequest>();
    }

    protected override void OnUpdate()
    {
        var prefabBuffer = SystemAPI.GetSingletonBuffer<LolEffectPrefab>();
        using var ecb = new EntityCommandBuffer(Allocator.Temp);
        double time = SystemAPI.Time.ElapsedTime;
        foreach (LolEffectRequest req in SystemAPI.Query<LolEffectRequest>())
        {
            var prefab = prefabBuffer[Random.Range(0, prefabBuffer.Length)].Value;
            var effect = ecb.Instantiate(prefab);
            ecb.SetComponent(effect, LocalTransform.FromPositionRotation(
                req.targetPosition, quaternion.Euler(0f, 0f, Random.Range(-30, 30))));
            double endTime = time + Random.Range(0.5f, 1.3f);
            ecb.AddComponent(effect, new LifeTime()
            {
                endTime = endTime
            });
            ecb.AddComponent(effect, new SpriteAlphaFade()
            {
                startTime = endTime - 0.4f,
                endTime = endTime
            });
            ecb.SetComponent(effect, new PhysicsVelocity
            {
                Linear = new float3(Random.Range(-0.15f, 0.15f), Random.Range(1f, 2f), 0f)
            });
        }

        ecb.Playback(EntityManager);
        EntityManager.DestroyEntity(requestQuery);
    }
}