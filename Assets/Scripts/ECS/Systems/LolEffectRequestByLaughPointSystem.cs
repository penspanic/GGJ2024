using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

public partial class LolEffectRequestByLaughPointSystem : SystemBase
{
    private EntityArchetype effectReqArcheType;
    protected override void OnCreate()
    {
        RequireForUpdate<LolEffectPrefab>();
        RequireForUpdate<SmallCat>();
        effectReqArcheType = EntityManager.CreateArchetype(ComponentType.ReadOnly<LolEffectRequest>());
    }

    public static float GetEffectSpawnInterval(float laughScore)
    {
        return 0.2f / laughScore;
    }

    protected override void OnUpdate()
    {
        double time = SystemAPI.Time.ElapsedTime;
        using var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach ((RefRW<SmallCat> smallCat, RefRO<LocalTransform> localTransform) in SystemAPI.Query<RefRW<SmallCat>, RefRO<LocalTransform>>())
        {
            if (smallCat.ValueRO.LaughScore < 0.01f || smallCat.ValueRO.NextEffectRequestTime == 0)
                continue;

            if (time < smallCat.ValueRO.NextEffectRequestTime)
                continue;

            float spawnInterval = GetEffectSpawnInterval(smallCat.ValueRO.LaughScore);
            smallCat.ValueRW.NextEffectRequestTime = time + spawnInterval;
            var req = ecb.CreateEntity(effectReqArcheType);
            ecb.SetComponent(req, new LolEffectRequest() { targetPosition = localTransform.ValueRO.Position });
        }
        ecb.Playback(EntityManager);
    }
}