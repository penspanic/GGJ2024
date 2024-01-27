using Unity.Entities;
using UnityEngine;

public class FurPrefabsAuthoring : MonoBehaviour
{
    public GameObject[] crowds;
    public class PrefabsAuthoringBaker : Baker<FurPrefabsAuthoring>
    {
        public override void Bake(FurPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var buffer = AddBuffer<FurPrefab>(entity);
            foreach (var crowd in authoring.crowds)
            {
                var prefab = GetEntity(crowd, TransformUsageFlags.Dynamic);
                buffer.Add(new FurPrefab { value = prefab });
            }
        }
    }
}

