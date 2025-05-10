using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level
{
    private readonly Room[,] rooms;
    private Vector2Int startRoomPosition;
    private Vector2Int endRoomPosition;
    private readonly Dictionary<int, int> numberOfRoomsBySection;

    public Level(int mapWidthHeight)
    {
        rooms = new Room[mapWidthHeight, mapWidthHeight];
        numberOfRoomsBySection = new Dictionary<int, int>();
    }

    public Room[,] Rooms => rooms;

    public Vector2Int StartRoomPosition => startRoomPosition;

    public Vector2Int EndRoomPosition => endRoomPosition;

    public void AddStartRoom(Room startRoom)
    {
        rooms[startRoom.Position.x, startRoom.Position.y] = startRoom;
        startRoomPosition = startRoom.Position;
        numberOfRoomsBySection[startRoom.Section] = 1;
    }

    public void AddEndRoom(Room endRoom)
    {
        rooms[endRoom.Position.x, endRoom.Position.y] = endRoom;
        endRoomPosition = endRoom.Position;
        numberOfRoomsBySection[endRoom.Section] = numberOfRoomsBySection.GetValueOrDefault(endRoom.Section, 0) + 1;
    }

    public void AddRoom(Room room)
    {
        rooms[room.Position.x, room.Position.y] = room;
        numberOfRoomsBySection[room.Section] = numberOfRoomsBySection.GetValueOrDefault(room.Section, 0) + 1;
    }

    public void UpdateRoom(Room room)
    {
        rooms[room.Position.x, room.Position.y] = room;
    }

    public Room GetRoom(Vector2Int position)
    {
        return rooms[position.x, position.y];
    }

    public int GetNumberOfRoomsInSection(int section)
    {
        return numberOfRoomsBySection.GetValueOrDefault(section, 0);
    }

    public int GetHigherSection()
    {
        return numberOfRoomsBySection.Keys.Max();
    }
}
