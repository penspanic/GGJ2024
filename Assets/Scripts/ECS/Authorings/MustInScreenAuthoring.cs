using Unity.Entities;
using UnityEngine;

public class MustInScreenAuthoring : MonoBehaviour
{
    public class MustInScreenAuthoringBaker : Baker<MustInScreenAuthoring>
    {
        public override void Bake(MustInScreenAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<MustInScreen>(entity);
        }
    }
}