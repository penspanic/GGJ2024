using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using Random = UnityEngine.Random;

public partial class SmallCatSpawningSystem : SystemBase
{
    private float spawnInterval = 0;
    private bool isInitialSpawn = false;
    private int spawnCount = 0;

    protected override void OnCreate()
    {
        RequireForUpdate<CrowdPrefab>();
        RequireForUpdate<SmallCatSpawner>();
        RequireForUpdate<SmallCatZone>();
    }

    protected override void OnUpdate()
    {
        if(isInitialSpawn is false) {
            var spawner = SystemAPI.GetSingleton<SmallCatSpawner>();
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

        // for(int i = 0; i < 2; i++) {
        //     Spawn();
        // }
    }

    private void Spawn()
    {
        var zone = SystemAPI.GetSingleton<SmallCatZone>();
        var prefabBuffer = SystemAPI.GetSingletonBuffer<CrowdPrefab>();

        // 무작위 위치 생성
        float2 randomPosition = new float2(
            Random.Range(zone.aabb.Min.x, zone.aabb.Max.x),
            Random.Range(zone.aabb.Min.y, zone.aabb.Max.y)
        );

        Entity prefabEntity = prefabBuffer[Random.Range(0, prefabBuffer.Length)].value;
        Entity furEntity = EntityManager.Instantiate(prefabEntity);
        EntityManager.AddComponent<PhysicsMassOverride>(furEntity);
        var transformRW = SystemAPI.GetComponentRW<LocalTransform>(furEntity);
        transformRW.ValueRW.Position = new float3(randomPosition.x, randomPosition.y, SmallCat.ZPosition);
    }
}