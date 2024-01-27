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

        protected override void OnCreate()
        {
            base.OnCreate();
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
                    currentColor = Color.gray;
                else if (crowdPerson.ValueRO.LaughScore < 0.5f)
                    currentColor = (Color.gray + crowdPerson.ValueRW.MainColor) / 2f;
                else
                    currentColor = crowdPerson.ValueRW.MainColor;

                spriteRenderer.color = currentColor;
            }
        }
    }
}