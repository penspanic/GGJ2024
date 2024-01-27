using Unity.Entities;
using Unity.Mathematics;
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
    public double StateStartTime;
    public double StateEndTime;

    public float StateDuration => (float)(StateEndTime - StateStartTime);
}

public enum CatStateType
{
    Unknown,
    Stop,
    Idle,
    Walk,
    Jump
}

public struct CatIdle : IComponentData
{
    public float2 startPosition;

    public const float Speed = 5f;
    public const float Amplitude = 0.1f;
}

public struct CatWalk : IComponentData
{
    public float2 direction;

    public const float Speed = 1f;
}

public struct CatJump : IComponentData
{
    public float2 startPosition;
    public float2 targetPosition;
    public float jumpHeight;
}