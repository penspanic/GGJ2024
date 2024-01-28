using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class FurSpawningSystem : SystemBase
{
    private float spawnInterval = 0;
    private bool isInitialSpawn = false;
    private int spawnCount = 0;
    private EntityQuery furQuery;

    protected override void OnCreate()
    {
        RequireForUpdate<FurPrefab>();
        RequireForUpdate<FurSpawner>();
        furQuery = GetEntityQuery(typeof(Fur));
    }

    protected override void OnUpdate()
    {
        var spawner = SystemAPI.GetSingleton<FurSpawner>();
        if(isInitialSpawn is false) {
            for (int i = 0; i < spawner.maxSpawnCountInFrame && spawnCount < spawner.spawnCount; i++, spawnCount++)
            {
                Spawn();
            }

            if (spawnCount >= spawner.spawnCount)
                isInitialSpawn = true;
        }

        if(spawnInterval > 0) {
            spawnInterval -= SystemAPI.Time.DeltaTime;
            return;
        }

        spawnInterval = 0.1f;

        int furCount = furQuery.CalculateEntityCount();
        int needCount = spawner.spawnCount - furCount;
        for(int i = 0; i < math.min(needCount, 5); i++)
        {
            Spawn();
        }
    }

    private void Spawn() {
        var aabb = new AABB { Center = new float3(0.22f, -2.2f, 0), Extents = new float3(1, 1, 0) };

        var prefabBuffer = SystemAPI.GetSingletonBuffer<FurPrefab>();

        // 무작위 위치 생성
        Vector2 randomPosition = new Vector2(
            Random.Range(aabb.Min.x, aabb.Max.x),
            Random.Range(aabb.Min.y, aabb.Max.y)
        );

        Entity prefabEntity = prefabBuffer[Random.Range(0, prefabBuffer.Length)].value;
        Entity furEntity = EntityManager.Instantiate(prefabEntity);
        EntityManager.AddComponent<PhysicsMassOverride>(furEntity);
        var transformRW = SystemAPI.GetComponentRW<LocalTransform>(furEntity);
        transformRW.ValueRW.Position = new float3(randomPosition.x, randomPosition.y, 0);
        var rigidBodyAspect = SystemAPI.GetAspect<RigidBodyAspect>(furEntity);
    }
}