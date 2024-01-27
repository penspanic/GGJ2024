using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

public struct FurSpawner : IComponentData
{
    public WeakObjectReference<PolygonCollider2D> colliderRef;
    public int spawnCount;
}

public struct FurPrefab : IBufferElementData
{
    public Entity value;
}

public struct MustInScreen : IComponentData
{
}