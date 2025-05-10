using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData")]
public class RoomData : ScriptableObject
{
    public int roomWidthHeight;

    public Vector2Int position;

    public HashSet<Vector2Int> exits = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> lockedDoors = new HashSet<Vector2Int>();

    public void Initialize(Room room)
    {
        roomWidthHeight = room.Spawnables.GetLength(0);
        position = room.Position;
        exits = room.Exits;
        doors = room.Doors;
        lockedDoors = room.LockedDoors;
    }
}
