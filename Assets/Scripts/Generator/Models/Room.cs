using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private Vector2Int position;
    private int section;
    private HashSet<Vector2Int> exits;
    private HashSet<Vector2Int> doors;
    private HashSet<Vector2Int> lockedDoors;
    private Spawnable[,] spawnables;

    public Room(Vector2Int position, int section, int roomWidthHeight)
    {
        this.position = position;
        this.section = section;
        exits = new HashSet<Vector2Int>();
        doors = new HashSet<Vector2Int>();
        lockedDoors = new HashSet<Vector2Int>();
        spawnables = new Spawnable[roomWidthHeight, roomWidthHeight];
    }

    public Vector2Int Position => position;

    public int Section => section;

    public HashSet<Vector2Int> Exits => exits;

    public HashSet<Vector2Int> Doors => doors;

    public HashSet<Vector2Int> LockedDoors => lockedDoors;

    public Spawnable[,] Spawnables => spawnables;

    public void AddSpawnable(Spawnable spawnable)
    {
        spawnables[spawnable.Position.x, spawnable.Position.y] = spawnable;
    }

    public Spawnable GetSpawnable(Vector2Int position)
    {
        return spawnables[position.x, position.y];
    }

    public bool HasAdjacentSpawnables(Vector2Int position)
    {
        for (int x = Mathf.Max(0, position.x - 1); x < Mathf.Min(position.x + 1 + 1, spawnables.GetLength(0)); x++)
        for (int y = Mathf.Max(0, position.y - 1); y < Mathf.Min(position.y + 1 + 1, spawnables.GetLength(1)); y++)
        {
            if (x == position.x && y == position.y) continue;

            if (spawnables[x, y] != null) return true;
        }

        return false;
    }
}
