using Unity.Entities;
using Unity.Mathematics;

public partial class LaughPointDecreaseSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireForUpdate<CrowdSettings>();
    }

    protected override void OnUpdate()
    {
        var settings = SystemAPI.GetSingleton<CrowdSettings>();
        foreach (RefRW<SmallCat> smallCatRW in SystemAPI.Query<RefRW<SmallCat>>())
        {
            smallCatRW.ValueRW.LaughScore = math.max(0, smallCatRW.ValueRO.LaughScore - (settings.LaughPointDecreasePerSecond * SystemAPI.Time.DeltaTime));
        }
    }
}