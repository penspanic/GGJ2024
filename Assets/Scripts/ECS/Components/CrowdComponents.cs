using Unity.Entities;
using Unity.Mathematics;

public struct CrowdPerson : IComponentData
{
}

public struct Grabbed : IComponentData
{
    public float3 offset;
}