using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

public class CrowdSpawnerAuthoring : MonoBehaviour
{
    public int spawnCount;
    public class Baker : Baker<CrowdSpawnerAuthoring>
    {
        public override void Bake(CrowdSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CrowdSpawner()
            {
                // colliderRef = authoring.colliderRef,
                spawnCount = authoring.spawnCount
            });
        }
    }
}