using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public partial class PersonColorSystem : SystemBase
    {
        private Color[] colors = new[]
        {
            Color.Lerp(Color.yellow, Color.green, 0f),
            Color.Lerp(Color.yellow, Color.green, 0.25f),
            Color.Lerp(Color.yellow, Color.green, 0.5f),
            Color.Lerp(Color.yellow, Color.green, 0.75f),
            Color.Lerp(Color.yellow, Color.green, 1f),
        };
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            foreach ((CrowdPerson crowdPerson, Entity entity) in SystemAPI.Query<CrowdPerson>().WithEntityAccess())
            {
                var spriteRenderer = EntityManager.GetComponentObject<SpriteRenderer>(entity);
                int colorIndex = crowdPerson.LaughScore == 1f ? 4 : ((int)(crowdPerson.LaughScore * 4)) % 4;
                spriteRenderer.color = colors[colorIndex];
            }
        }
    }
}