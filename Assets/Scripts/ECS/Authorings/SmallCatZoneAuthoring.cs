using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;

[RequireComponent(typeof(PhysicsShapeAuthoring))]
public class SmallCatZoneAuthoring : MonoBehaviour
{
    public class Baker : Baker<SmallCatZoneAuthoring>
    {
        public override void Bake(SmallCatZoneAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var shape = authoring.GetComponent<PhysicsShapeAuthoring>().GetBoxProperties();
            var zone = new SmallCatZone()
            {
                aabb = new AABB()
                {
                    Center = shape.Center,
                    Extents = shape.Size / 2f
                }
            };
            AddComponent(entity, zone);
        }
    }
}

public struct SmallCatZone : IComponentData
{
    public AABB aabb;
}