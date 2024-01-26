using Unity.Entities;
using UnityEngine;

public class CrowdPersonAuthoring : MonoBehaviour
{
    public class Baker : Baker<CrowdPersonAuthoring>
    {
        public override void Bake(CrowdPersonAuthoring authoring)
        {
            AddComponent<CrowdPerson>(GetEntity(TransformUsageFlags.Dynamic));
        }
    }
}
