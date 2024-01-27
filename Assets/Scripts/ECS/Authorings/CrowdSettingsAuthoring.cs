using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CrowdSettingsAuthoring : MonoBehaviour
{
    public float canInfectThreshold;
    public float beInfectedThreshold;
    public float laughSpriteAnimationThreshold;
    public float laughPointDecreasePerSecond;

    public float2 idleStatusSpeedRange;
    public float2 idleStatusAmplitudeRange;
    public float2 idleStatusDurationRange;

    public float2 moveStatusSpeedRange;
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
                LaughSpriteAnimationThreshold = authoring.laughSpriteAnimationThreshold,
                LaughPointDecreasePerSecond = authoring.laughPointDecreasePerSecond,
                IdleStatusSpeedRange = authoring.idleStatusSpeedRange,
                IdleStatusAmplitudeRange = authoring.idleStatusAmplitudeRange,
                IdleStatusDurationRange = authoring.idleStatusDurationRange,
                MoveStatusSpeedRange = authoring.moveStatusSpeedRange,
                MoveStatusDurationRange = authoring.moveStatusDurationRange
            });
        }
    }
}

public struct CrowdSettings : IComponentData
{
    public float CanInfectThreshold;
    public float BeInfectedThreshold;
    public float LaughSpriteAnimationThreshold;
    public float LaughPointDecreasePerSecond;
    public float2 IdleStatusSpeedRange;
    public float2 IdleStatusAmplitudeRange;
    public float2 IdleStatusDurationRange;
    public float2 MoveStatusSpeedRange;
    public float2 MoveStatusDurationRange;
}