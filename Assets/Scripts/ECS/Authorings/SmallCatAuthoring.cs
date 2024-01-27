using Unity.Entities;
using UnityEngine;

public class SmallCatAuthoring : MonoBehaviour
{
    public class Baker : Baker<SmallCatAuthoring>
    {
        public override void Bake(SmallCatAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<SmallCat>(entity);
            AddComponent(entity, new CatState() { State = CatStateType.Idle });
        }
    }
}

public struct SmallCat : IComponentData
{
    public float LaughScore;
    public double LastInfectionTime;

    public readonly bool CanInfect(double time)
    {
        if (LaughScore < 0.5f)
            return false;

        double elapsed = time - LastInfectionTime;
        // 0처리는 InfectionSystem Job에서 값 설정해서 어쩔수 없이 둠.
        if (elapsed != 0 && elapsed < 0.5f)
            return false;
        return true;
    }

    public readonly bool CanBeInfected() => LaughScore < 0.5f;
}

public struct CatState : IComponentData
{
    public CatStateType State;
    public double StateEndTime;
}

public enum CatStateType
{
    Unknown,
    Idle,
    Walk,
    Jump
}

public struct CatIdle : IComponentData { }
public struct CatWalk : IComponentData { }
public struct CatJump : IComponentData { }