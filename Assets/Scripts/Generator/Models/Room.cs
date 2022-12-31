using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int position;

    public int section;

    public HashSet<Vector2Int> exits;

    public Spawnable[,] spawnables;

    public Room(int roomWidthHeight)
    {
        spawnables = new Spawnable[roomWidthHeight, roomWidthHeight];
        exits = new HashSet<Vector2Int>();
    }

    public void AddSpawnable(Spawnable spawnable)
    {
        spawnables[spawnable.position.x, spawnable.position.y] = spawnable;
    }

    public Spawnable GetSpawnable(Vector2Int position)
    {
        return spawnables[position.x, position.y];
    }

    public bool HasAdjacentSpawnables(Vector2Int position)
    {
        for (int x = Mathf.Max(0, position.x - 1); x < Mathf.Min(position.x + 1 + 1, spawnables.GetLength(0)); x++)
        {
            for (int y = Mathf.Max(0, position.y - 1); y < Mathf.Min(position.y + 1 + 1, spawnables.GetLength(1)); y++)
            {
                if (x == position.x && y == position.y)
                {
                    continue;
                }

                if (spawnables[x, y] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
