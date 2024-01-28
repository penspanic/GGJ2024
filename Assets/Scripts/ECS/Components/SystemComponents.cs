using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

public struct FurSpawner : IComponentData
{
    public int spawnCount;
}

public struct SmallCatSpawner : IComponentData
{
    public int spawnCount;
    public int maxSpawnCountInFrame;
}

public struct FurPrefab : IBufferElementData
{
    public Entity value;
}

public struct CrowdPrefab : IBufferElementData
{
    public Entity value;
}

public struct MustInScreen : IComponentData
{
}

public struct LifeTime : IComponentData
{
    public double endTime;
}

public struct SpriteAlphaFade : IComponentData
{
    public double startTime;
    public double endTime;
}