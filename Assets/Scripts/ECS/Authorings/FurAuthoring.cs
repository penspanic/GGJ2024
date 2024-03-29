using Unity.Entities;
using UnityEngine;

public class FurAuthoring : MonoBehaviour
{
    public class Baker : Baker<FurAuthoring>
    {
        public override void Bake(FurAuthoring authoring)
        {
            AddComponent<Fur>(GetEntity(TransformUsageFlags.Dynamic));
        }
    }
}


public struct CatFurStatus : IComponentData
{
    public float LastTouchTime;
}
