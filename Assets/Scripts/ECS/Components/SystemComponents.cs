using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

public struct CrowdSpawner : IComponentData
{
    public WeakObjectReference<PolygonCollider2D> colliderRef;
    public int spawnCount;
}

public struct CrowdPrefab : IBufferElementData
{
    public Entity value;
}