using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class CrowdSpawningSystem : SystemBase
{
    private float spawnInterval = 0;
    private bool isInitialSpawn = false;

    protected override void OnCreate()
    {
        RequireForUpdate<CrowdPrefab>();
        RequireForUpdate<CrowdSpawner>();
    }

    protected override void OnUpdate()
    {
        if(isInitialSpawn is false) {
            var spawner = SystemAPI.GetSingleton<CrowdSpawner>();
            for (int i = 0; i < spawner.spawnCount; i++)
            {
                Spawn();
            }
            isInitialSpawn = true;
        }

        if(spawnInterval > 0) {
            spawnInterval -= SystemAPI.Time.DeltaTime;
            return;
        }

        spawnInterval = 0.1f;

        for(int i = 0; i < 2; i++) {
            Spawn();
        }
    }

    private void Spawn() {
        var aabb = new AABB { Center = new float3(0, 0, 0), Extents = new float3(1, 1, 0) };

        var prefabBuffer = SystemAPI.GetSingletonBuffer<CrowdPrefab>();

        // 무작위 위치 생성
        Vector2 randomPosition = new Vector2(
            Random.Range(aabb.Min.x, aabb.Max.x),
            Random.Range(aabb.Min.y, aabb.Max.y)
        );

        Entity prefabEntity = prefabBuffer[Random.Range(0, prefabBuffer.Length)].value;
        Entity crowdMemberEntity = EntityManager.Instantiate(prefabEntity);
        EntityManager.AddComponent<PhysicsMassOverride>(crowdMemberEntity);
        var transformRW = SystemAPI.GetComponentRW<LocalTransform>(crowdMemberEntity);
        transformRW.ValueRW.Position = new float3(randomPosition.x, randomPosition.y, 0);
        var rigidBodyAspect = SystemAPI.GetAspect<RigidBodyAspect>(crowdMemberEntity);
    }
}