using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
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
            RequireForUpdate<Grabbed>();
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButton(0))
                return;

            foreach (Entity entity in grabbedQuery.ToEntityArray(Allocator.Temp))
            {
                var rigidBodyAspect = SystemAPI.GetAspect<RigidBodyAspect>(entity);
                rigidBodyAspect.IsKinematic = false;
                rigidBodyAspect.LinearVelocity = float3.zero;
            }

            EntityManager.RemoveComponent<Grabbed>(grabbedQuery);
        }
    }
}