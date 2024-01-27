using Unity.Entities;
using UnityEngine;

public partial class CrowdSpriteAnimationSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<CrowdSettings>();
        RequireForUpdate<CrowdSpriteAnimationData>();
    }

    protected override void OnUpdate()
    {
        var settings = SystemAPI.GetSingleton<CrowdSettings>();
        var animationData = SystemAPI.GetSingleton<CrowdSpriteAnimationData>();
        foreach ((RefRW<SmallCat> smallCatRW, Entity entity) in SystemAPI.Query<RefRW<SmallCat>>().WithEntityAccess())
        {
            float spriteElapsed = (float)(SystemAPI.Time.ElapsedTime - smallCatRW.ValueRO.LastSpriteSetTime);
            var spriteRenderer = EntityManager.GetComponentObject<SpriteRenderer>(entity);
            // change sprite
            if (spriteElapsed > animationData.SpriteDuration)
            {
                if (smallCatRW.ValueRO.SpriteIndex == 0)
                {
                    if (smallCatRW.ValueRO.LaughScore >= settings.LaughSpriteAnimationThreshold)
                        spriteRenderer.sprite = Resources.Load<Sprite>(animationData.laughSprite2.ToString());
                    else
                        spriteRenderer.sprite = Resources.Load<Sprite>(animationData.idleSprite2.ToString());
                    smallCatRW.ValueRW.SpriteIndex = 1;
                }
                else
                {
                    if (smallCatRW.ValueRO.LaughScore >= settings.LaughSpriteAnimationThreshold)
                        spriteRenderer.sprite = Resources.Load<Sprite>(animationData.laughSprite1.ToString());
                    else
                        spriteRenderer.sprite = Resources.Load<Sprite>(animationData.idleSprite1.ToString());
                    smallCatRW.ValueRW.SpriteIndex = 0;
                }
                smallCatRW.ValueRW.LastSpriteSetTime = SystemAPI.Time.ElapsedTime;
            }
        }
    }
}