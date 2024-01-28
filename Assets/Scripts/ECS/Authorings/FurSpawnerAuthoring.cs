using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

public class FurSpawnerAuthoring : MonoBehaviour
{
    public int spawnCount;
    public int maxSpawnCountInFrame;
    public class Baker : Baker<FurSpawnerAuthoring>
    {
        public override void Bake(FurSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FurSpawner()
            {
                spawnCount = authoring.spawnCount,
                maxSpawnCountInFrame = authoring.maxSpawnCountInFrame
            });
        }
    }
}