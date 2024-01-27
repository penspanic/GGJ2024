using Unity.Entities;
using Unity.Mathematics;

public struct Fur : IComponentData
{
}

public struct Grabbed : IComponentData
{
    public float2 offset;
}