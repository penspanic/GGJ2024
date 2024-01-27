using Unity.Entities;
using Unity.Mathematics;

public struct CrowdPerson : IComponentData
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

public struct Grabbed : IComponentData
{
    public float3 offset;
}