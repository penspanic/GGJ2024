using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public unsafe partial class MainScoreHandlingSystem : SystemBase
{
    private EntityQuery crowdQuery;
    private EntityTypeHandle entityTypeHandle;
    private NativeList<DistanceHit> overlapResults;
    private EntityArchetype effectReqArcheType;

    protected override void OnCreate()
    {
        base.OnCreate();
        crowdQuery = GetEntityQuery(ComponentType.ReadOnly<SmallCat>());
        entityTypeHandle = GetEntityTypeHandle();
        RequireForUpdate<PhysicsWorldSingleton>();
        overlapResults = new NativeList<DistanceHit>(Allocator.Persistent);
        effectReqArcheType = EntityManager.CreateArchetype(ComponentType.ReadOnly<LolEffectRequest>());

        MainScene.Instance.OnScoreChanged += OnMainScoreChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        MainScene.Instance.OnScoreChanged -= OnMainScoreChanged;
        overlapResults.Dispose();
    }

    private unsafe void OnMainScoreChanged(int previousScore, int newScore)
    {
        entityTypeHandle.Update(this);
        // select random small cat to give laugh score
        int totalSmallCats = crowdQuery.CalculateEntityCount();
        if (totalSmallCats == 0)
            return;

        // 2.327
        using var crowds = crowdQuery.ToEntityArray(Allocator.Temp);
        Entity selected = default;
        while (true)
        {
            int catIndex = UnityEngine.Random.Range(0, totalSmallCats);
            selected = crowds[catIndex];
            // 무대에 가리는 놈은 제외하도록.
            if (SystemAPI.GetComponent<LocalTransform>(selected).Position.y > 2.327f)
                break;
        }

        float3 position = SystemAPI.GetComponent<LocalTransform>(selected).Position;
        // TODO: Add global effect system and use here. 

        var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        overlapResults.Clear();
        var filter = new CollisionFilter()
        {
            BelongsTo = 1 << 2,
            CollidesWith = 1 << 2,
        };
        if (physicsWorldSingleton.OverlapSphere(position, 0.5f, ref overlapResults, filter) is false)
            return;

        // give laugh point to overlapped entities, selected : 0.5, distance < 0.2f, 0.3, others : 0.1
        using var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (DistanceHit distanceHit in overlapResults)
        {
            var smallCat = SystemAPI.GetComponent<SmallCat>(distanceHit.Entity);
            float laughScore = 0.1f;
            if (distanceHit.Entity == selected)
                laughScore = 0.5f;
            else if (distanceHit.Distance < 0.2f)
                laughScore = 0.3f;

            smallCat.LaughScore = math.min(smallCat.LaughScore + laughScore, 1f);
            var effectReq = ecb.CreateEntity(effectReqArcheType);
            ecb.SetComponent(effectReq, new LolEffectRequest() { targetPosition = distanceHit.Position });
            ecb.SetComponent(distanceHit.Entity, smallCat);
        }
        ecb.Playback(EntityManager);
    }

    protected override void OnUpdate()
    {
    }
}