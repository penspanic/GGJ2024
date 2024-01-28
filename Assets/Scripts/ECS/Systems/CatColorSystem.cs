using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public partial class CatColorSystem : SystemBase
    {
        private Color[] colors = null;

        private Color baseColor;

        protected override void OnCreate()
        {
            base.OnCreate();

            ColorUtility.TryParseHtmlString("#1F1F1FFF", out baseColor);
            ColorUtility.TryParseHtmlString("#A1FEFFFF", out var color1);
            ColorUtility.TryParseHtmlString("#FEFF8DFF", out var color2);
            ColorUtility.TryParseHtmlString("#FFa1E6FF", out var color3);
            ColorUtility.TryParseHtmlString("#A5A1FFFF", out var color4);
            ColorUtility.TryParseHtmlString("#FF6A6AFF", out var color5);
            colors = new[] { color1, color2, color3, color4, color5 };
        }

        protected override void OnUpdate()
        {
            foreach ((RefRW<SmallCat> crowdPerson, Entity entity) in SystemAPI.Query<RefRW<SmallCat>>().WithEntityAccess())
            {
                if (crowdPerson.ValueRO.MainColor == default)
                    crowdPerson.ValueRW.MainColor = colors[Random.Range(0, colors.Length)];

                var spriteRenderer = EntityManager.GetComponentObject<SpriteRenderer>(entity);
                Color currentColor = default;
                if (crowdPerson.ValueRO.LaughScore == 0f)
                    currentColor = baseColor;
                else if (crowdPerson.ValueRO.LaughScore < 0.5f)
                    currentColor = Color.Lerp(baseColor, crowdPerson.ValueRW.MainColor, crowdPerson.ValueRO.LaughScore * 2f);
                spriteRenderer.color = currentColor;
            }
        }
    }
}