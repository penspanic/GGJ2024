using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public partial class CatColorSystem : SystemBase
    {
        private Color[] colors = new[]
        {
            Color.yellow,
            Color.green,
            Color.blue,
            Color.magenta,
        };

        private Color baseColor;

        protected override void OnCreate()
        {
            base.OnCreate();
            ColorUtility.TryParseHtmlString("#080808FF", out baseColor);
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
                    currentColor = (baseColor + crowdPerson.ValueRW.MainColor) / 2f;
                else
                    currentColor = crowdPerson.ValueRW.MainColor;

                spriteRenderer.color = currentColor;
            }
        }
    }
}