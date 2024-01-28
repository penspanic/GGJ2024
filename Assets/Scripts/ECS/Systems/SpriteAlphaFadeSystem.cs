using Unity.Entities;
using UnityEngine;

public partial class SpriteAlphaFadeSystem : SystemBase
{
    protected override void OnUpdate()
    {
        double time = SystemAPI.Time.ElapsedTime;
        foreach ((SpriteAlphaFade spriteAlphaFade, Entity entity) in SystemAPI.Query<SpriteAlphaFade>().WithEntityAccess())
        {
            float alpha = 1f;
            if (time > spriteAlphaFade.startTime)
            {
                var renderer = EntityManager.GetComponentObject<SpriteRenderer>(entity);
                alpha = (float) (spriteAlphaFade.endTime - time) / (float) (spriteAlphaFade.endTime - spriteAlphaFade.startTime);
                var color = renderer.color;
                color.a = alpha;
                renderer.color = color;
            }
        }
    }
}