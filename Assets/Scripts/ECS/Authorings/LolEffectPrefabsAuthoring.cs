using Unity.Entities;
using UnityEngine;

public class LolEffectPrefabsAuthoring : MonoBehaviour
{
    public GameObject[] prefabs;
    public class LolEffectAuthoringBaker : Baker<LolEffectPrefabsAuthoring>
    {
        public override void Bake(LolEffectPrefabsAuthoring prefabsAuthoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var buffer = AddBuffer<LolEffectPrefab>(entity);
            foreach (var prefab in prefabsAuthoring.prefabs)
            {
                buffer.Add(new LolEffectPrefab { Value = GetEntity(prefab, TransformUsageFlags.Dynamic) });
            }
        }
    }
}

public struct LolEffectPrefab : IBufferElementData
{
    public Entity Value;
}