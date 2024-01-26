using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public partial class GrabReleaseSystem : SystemBase
    {
        private EntityQuery grabbedQuery;
        protected override void OnCreate()
        {
            base.OnCreate();
            grabbedQuery = GetEntityQuery(typeof(Grabbed));
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButton(0))
                return;

            EntityManager.RemoveComponent<Grabbed>(grabbedQuery);
        }
    }
}