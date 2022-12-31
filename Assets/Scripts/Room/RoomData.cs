using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData")]
public class RoomData : ScriptableObject
{
    public int roomWidthHeight;

    public Vector2Int position;

    public HashSet<Vector2Int> exits = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

    public void Initialize(Room room)
    {
        roomWidthHeight = room.spawnables.GetLength(0);
        position = room.position;
        exits = room.exits;
        doors = room.exits;
    }
}
