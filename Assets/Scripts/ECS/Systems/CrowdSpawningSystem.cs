using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class CrowdSpawningSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<CrowdPrefab>();
        RequireForUpdate<CrowdSpawner>();
    }

    protected override void OnUpdate()
    {
        var spawner = SystemAPI.GetSingleton<CrowdSpawner>();
        var prefabBuffer = SystemAPI.GetSingletonBuffer<CrowdPrefab>();
        // // spawn crowds in the collider area
        // spawner.colliderRef.LoadAsync();
        // bool loadResult = spawner.colliderRef.WaitForCompletion();
        // if (loadResult is false)
        // {
        //     Debug.LogError($"Collider Load Failed");
        //     Enabled = false;
        //     return;
        // }
        //
        // PolygonCollider2D collider = spawner.colliderRef.Result;
        // if (collider == null)
        //     return;

        var aabb = new AABB { Center = new float3(0, 0, 0), Extents = new float3(5, 3, 0) };

        int maxTry = 10000;
        for (int i = 0; i < spawner.spawnCount; i++)
        {
            // 무작위 위치 생성
            Vector2 randomPosition = new Vector2(
                Random.Range(aabb.Min.x, aabb.Max.x),
                Random.Range(aabb.Min.y, aabb.Max.y)
            );

            // 위치가 Collider 내부에 있는지 확인
            // if (collider.OverlapPoint(randomPosition))
            {
                // Collider 내부에 군중 생성
                Entity prefabEntity = prefabBuffer[Random.Range(0, prefabBuffer.Length)].value;
                Entity crowdMember = EntityManager.Instantiate(prefabEntity);
                var transformRW = SystemAPI.GetComponentRW<LocalTransform>(crowdMember);
                transformRW.ValueRW.Position = new float3(randomPosition.x, randomPosition.y, 0);
            }
            // else
            // {
            //     --maxTry;
            //     if (maxTry <= 0)
            //     {
            //         Debug.LogError($"Spawn Failed.");
            //         break;
            //     }
            //     // Collider 외부에 군중을 생성하지 않도록 조치
            //     i--;
            // }
        }

        Enabled = false;
    }
}