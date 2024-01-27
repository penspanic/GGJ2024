using Unity.Entities;
using UnityEngine;

public class RodAuthoring : MonoBehaviour
{
    public class RodAuthoringBaker : Baker<RodAuthoring>
    {
        public override void Bake(RodAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Rod());
        }
    }
}

public struct Rod : IComponentData
{
}