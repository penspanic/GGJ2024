using Unity.Entities;
using UnityEngine;

public class CrowdPrefabsAuthoring : MonoBehaviour
{
    public GameObject[] crowds;
    public class PrefabsAuthoringBaker : Baker<CrowdPrefabsAuthoring>
    {
        public override void Bake(CrowdPrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var buffer = AddBuffer<CrowdPrefab>(entity);
            foreach (var crowd in authoring.crowds)
            {
                var prefab = GetEntity(crowd, TransformUsageFlags.Dynamic);
                buffer.Add(new CrowdPrefab { value = prefab });
            }
        }
    }
}