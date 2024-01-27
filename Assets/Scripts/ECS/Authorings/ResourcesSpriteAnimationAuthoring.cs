using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ResourcesSpriteAnimationAuthoring : MonoBehaviour
{
    public string idleSprite1;
    public string idleSprite2;
    public string laughSprite1;
    public string laughSprite2;
    public float spriteDuration;
    public class ResourcesSpriteAnimationAuthoringBaker : Baker<ResourcesSpriteAnimationAuthoring>
    {
        public override void Bake(ResourcesSpriteAnimationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CrowdSpriteAnimationData()
            {
                idleSprite1 = authoring.idleSprite1,
                idleSprite2 = authoring.idleSprite2,
                laughSprite1 = authoring.laughSprite1,
                laughSprite2 = authoring.laughSprite2,
                SpriteDuration = authoring.spriteDuration
            });
        }
    }
}

public struct CrowdSpriteAnimationData : IComponentData
{
    public FixedString128Bytes idleSprite1;
    public FixedString128Bytes idleSprite2;
    public FixedString128Bytes laughSprite1;
    public FixedString128Bytes laughSprite2;
    public float SpriteDuration;
}