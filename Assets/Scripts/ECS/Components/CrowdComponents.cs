using Unity.Entities;
using Unity.Mathematics;

public struct CrowdPerson : IComponentData
{
    public float LaughScore;

    public bool CanInfect() => LaughScore >= 0.5f;
    public bool CanBeInfected() => LaughScore < 0.5f;

}

public struct Grabbed : IComponentData
{
    public float3 offset;
}