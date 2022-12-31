using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level
{
    public Room[,] rooms;

    public Vector2Int startRoomPosition;

    public Vector2Int endRoomPosition;

    private Dictionary<int, int> numberOfRoomsBySection;

    public Level(int mapWidthHeight)
    {
        rooms = new Room[mapWidthHeight, mapWidthHeight];
        numberOfRoomsBySection = new Dictionary<int, int>();
    }

    public void AddStartRoom(Room startRoom)
    {
        rooms[startRoom.position.x, startRoom.position.y] = startRoom;
        startRoomPosition = startRoom.position;
        numberOfRoomsBySection[startRoom.section] = 1;
    }

    public void AddEndRoom(Room endRoom)
    {
        rooms[endRoom.position.x, endRoom.position.y] = endRoom;
        endRoomPosition = endRoom.position;
        numberOfRoomsBySection[endRoom.section] = numberOfRoomsBySection.GetValueOrDefault(endRoom.section, 0) + 1;
    }

    public void AddRoom(Room room)
    {
        rooms[room.position.x, room.position.y] = room;
        numberOfRoomsBySection[room.section] = numberOfRoomsBySection.GetValueOrDefault(room.section, 0) + 1;
    }

    public void UpdateRoom(Room room)
    {
        rooms[room.position.x, room.position.y] = room;
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
