using Unity.Entities;

public partial class LifeTimeSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<LifeTime>();
    }

    protected override void OnUpdate()
    {
        double time = SystemAPI.Time.ElapsedTime;
        Entities
            .WithStructuralChanges()
            .ForEach((Entity entity, ref LifeTime lifeTime) =>
            {
                if (lifeTime.endTime < time)
                {
                    EntityManager.DestroyEntity(entity);
                }
            }).Run();
    }
}