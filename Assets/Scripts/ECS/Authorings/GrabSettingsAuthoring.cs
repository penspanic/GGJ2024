using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class GrabSettingsAuthoring : MonoBehaviour
{
    public PhysicsCategoryTags BelongsTo;
    public PhysicsCategoryTags CollidesWith;

    public class Baker : Baker<GrabSettingsAuthoring>
    {
        public override void Bake(GrabSettingsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GrabSettings()
            {
                filter = new CollisionFilter()
                {
                    BelongsTo = authoring.BelongsTo.Value,
                    CollidesWith = authoring.CollidesWith.Value
                }
            });
        }
    }
}

public struct GrabSettings : IComponentData
{
    public CollisionFilter filter;
}