using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData")]
public class RoomData : ScriptableObject
{
    [SerializeField] private int roomWidthHeight;
    [SerializeField] private Vector2Int position;
    private HashSet<Vector2Int> exits = new();
    private HashSet<Vector2Int> doors = new();
    private HashSet<Vector2Int> lockedDoors = new();

    public int RoomWidthHeight => roomWidthHeight;

    public Vector2Int Position => position;

    public HashSet<Vector2Int> Exits => exits;

    public HashSet<Vector2Int> Doors => doors;

    public HashSet<Vector2Int> LockedDoors => lockedDoors;

    public void Initialize(Room room)
    {
        roomWidthHeight = room.Spawnables.GetLength(0);
        position = room.Position;
        exits = room.Exits;
        doors = room.Doors;
        lockedDoors = room.LockedDoors;
    }
}
