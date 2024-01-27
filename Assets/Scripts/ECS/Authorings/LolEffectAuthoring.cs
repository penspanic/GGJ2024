using Unity.Entities;
using UnityEngine;

public class LolEffectAuthoring : MonoBehaviour
{
    public class LolEffectAuthoringBaker : Baker<LolEffectAuthoring>
    {
        public override void Bake(LolEffectAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<LolEffect>(entity);
        }
    }
}

public struct LolEffect : IComponentData
{
}