using Unity.Entities;
using UnityEngine;

public class SmallCatSpawnerAuthoring : MonoBehaviour
{
    public int spawnCount;
    public int maxSpawnCountInFrame;
    public class Baker : Baker<SmallCatSpawnerAuthoring>
    {
        public override void Bake(SmallCatSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SmallCatSpawner()
            {
                spawnCount = authoring.spawnCount,
                maxSpawnCountInFrame = authoring.maxSpawnCountInFrame
            });
        }
    }
}