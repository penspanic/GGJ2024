using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CrowdSettingsAuthoring : MonoBehaviour
{
    public float canInfectThreshold;
    public float beInfectedThreshold;
    public float idleStatusSpeed;
    public float2 idleStatusDurationRange;
    public float moveStatusSpeed;
    public float2 moveStatusDurationRange;

    public class CrowdSettingsAuthoringBaker : Baker<CrowdSettingsAuthoring>
    {
        public override void Bake(CrowdSettingsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CrowdSettings()
            {
                CanInfectThreshold = authoring.canInfectThreshold,
                BeInfectedThreshold = authoring.beInfectedThreshold,
                IdleStatusSpeed = authoring.idleStatusSpeed,
                IdleStatusDurationRange = authoring.idleStatusDurationRange,
                MoveStatusSpeed = authoring.moveStatusSpeed,
                MoveStatusDurationRange = authoring.moveStatusDurationRange
            });
        }
    }
}

public struct CrowdSettings : IComponentData
{
    public float CanInfectThreshold;
    public float BeInfectedThreshold;
    public float IdleStatusSpeed;
    public float2 IdleStatusDurationRange;
    public float MoveStatusSpeed;
    public float2 MoveStatusDurationRange;
}