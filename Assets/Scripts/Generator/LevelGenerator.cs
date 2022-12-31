using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private static Vector2Int[] NextRoomDirectionChoices = new Vector2Int[] {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    private RoomGenerator roomGenerator;

    void Awake()
    {
        roomGenerator = GetComponentInChildren<RoomGenerator>();
    }

    public Level Generate(GeneratorConfiguration configuration)
    {
        Level level = new Level(configuration.mapWidthHeight);

        // Create the start room.
        Vector2Int startRoomPosition = GenerateStartRoomPosition(level, configuration);
        Room startRoom = roomGenerator.GenerateStartRoom(startRoomPosition, configuration);
        level.AddStartRoom(startRoom);

        // Initialize the agent.
        Stack<Vector2Int> roomPositionsToVisit = new Stack<Vector2Int>();
        HashSet<Vector2Int> roomPositionsVisited = new HashSet<Vector2Int>();
        roomPositionsToVisit.Push(level.startRoomPosition);

        // Run the agent which will create the next rooms.
        while (roomPositionsToVisit.Count != 0)
        {
            Vector2Int currentRoomPosition = roomPositionsToVisit.Pop();
            Room currentRoom = level.GetRoom(currentRoomPosition);

            HashSet<Vector2Int> nextRoomDirections = GenerateNextRoomDirections(level, currentRoom, configuration);

            nextRoomDirections.ToList().ForEach(nextRoomDirection =>
            {
                Vector2Int nextRoomPosition = new Vector2Int(
                    Mathf.Max(0, Mathf.Min(currentRoom.position.x + nextRoomDirection.x, level.rooms.GetLength(0) - 1)),
                    Mathf.Max(0, Mathf.Min(currentRoom.position.y + nextRoomDirection.y, level.rooms.GetLength(1) - 1)));

                // Ignore next rooms with the same position than the current one.
                if (nextRoomPosition == currentRoom.position)
                {
                    return;
                }

                Room existingRoom = level.GetRoom(nextRoomPosition);

                if (existingRoom == null)
                {
                    Room nextRoom = GenerateNextRoom(level, currentRoom, nextRoomPosition, configuration);

                    AddExitToCurrentRoom(currentRoom, nextRoomDirection);
                    AddExitToNextRoom(nextRoom, nextRoomDirection);
                    level.UpdateRoom(currentRoom);
                    level.AddRoom(nextRoom);

                    // Add the next room to the stack of rooms to visit, if it has not been visited yet.
                    roomPositionsVisited.Add(currentRoomPosition);
                    if (!roomPositionsVisited.Contains(nextRoom.position))
                    {
                        roomPositionsToVisit.Push(nextRoom.position);
                    }
                }
                else
                {
                    // Ignore next rooms already existing with a different section than the current one.
                    if (existingRoom.section != currentRoom.section)
                    {
                        return;
                    }

                    AddExitToCurrentRoom(currentRoom, nextRoomDirection);
                    AddExitToNextRoom(existingRoom, nextRoomDirection);
                    level.UpdateRoom(currentRoom);
                    level.UpdateRoom(existingRoom);
                }
            });
        }

        // Create the end room if possible.
        (Vector2Int endRoomPosition, Room parentToEndRoom, Vector2Int parentToEndRoomDirection) = GenerateEndRoomPosition(level);
        Room endRoom = roomGenerator.GenerateEndRoom(endRoomPosition, level.GetHigherSection(), configuration);
        level.AddEndRoom(endRoom);

        AddExitToCurrentRoom(parentToEndRoom, parentToEndRoomDirection);
        AddExitToNextRoom(endRoom, parentToEndRoomDirection);
        level.UpdateRoom(parentToEndRoom);
        level.UpdateRoom(endRoom);

        return level;
    }

    private Vector2Int GenerateStartRoomPosition(Level level, GeneratorConfiguration configuration)
    {
        return new Vector2Int(
            Random.Range(configuration.startRoomMargin, level.rooms.GetLength(0) - configuration.startRoomMargin),
            Random.Range(configuration.startRoomMargin, level.rooms.GetLength(1) - configuration.startRoomMargin));
    }

    private HashSet<Vector2Int> GenerateNextRoomDirections(Level level, Room currentRoom,
        GeneratorConfiguration configuration)
    {
        int minNumberOfNextRoomDirections;
        int maxNumberOfNextRoomDirections;

        // For the start room, ensure that we will have exactly 1 next room.
        if (currentRoom.position == level.startRoomPosition)
        {
            minNumberOfNextRoomDirections = 1;
            maxNumberOfNextRoomDirections = 1;
        }
        else
        {
            int numberOfRoomsInSection = level.GetNumberOfRoomsInSection(currentRoom.section);

            // If there are not enough rooms in the current section, ensure that there will be at least 1 next room. 
            if (numberOfRoomsInSection <= configuration.numberOfRoomsInSectionLowThreshold)
            {
                minNumberOfNextRoomDirections = 1;
                maxNumberOfNextRoomDirections = 4;
            }
            // If there are too many rooms in the current section, reduce the probability of having a high number of next rooms.
            else if (numberOfRoomsInSection >= configuration.numberOfRoomsInSectionHighThreshold)
            {
                minNumberOfNextRoomDirections = 0;
                maxNumberOfNextRoomDirections = 1;
            }
            else
            {
                minNumberOfNextRoomDirections = 0;
                maxNumberOfNextRoomDirections = 4;
            }
        }

        int numberOfNextRoomDirections = Random.Range(minNumberOfNextRoomDirections, maxNumberOfNextRoomDirections + 1);

        HashSet<Vector2Int> nextRoomDirections = new HashSet<Vector2Int>();
        while (nextRoomDirections.Count != numberOfNextRoomDirections)
        {
            nextRoomDirections.Add(NextRoomDirectionChoices[Random.Range(0, NextRoomDirectionChoices.Length)]);
        }

        return nextRoomDirections;
    }

    private Room GenerateNextRoom(Level level, Room currentRoom, Vector2Int nextRoomPosition,
        GeneratorConfiguration configuration)
    {
        int nextRoomSection = GenerateNextRoomSection(level, currentRoom, configuration);

        return roomGenerator.Generate(nextRoomPosition, nextRoomSection, configuration);
    }

    private int GenerateNextRoomSection(Level level, Room currentRoom, GeneratorConfiguration configuration)
    {
        int numberOfRoomsInSection = level.GetNumberOfRoomsInSection(currentRoom.section);

        // If there are not enough rooms in the current section, do not change section.
        if (numberOfRoomsInSection < configuration.numberOfRoomsInSectionHighThreshold * configuration.numberOfRoomsInSectionThresholdRatio)
        {
            return currentRoom.section;
        }

        if (Random.Range(0, 1 + 1) == 1)
        {
            return level.GetHigherSection() + 1;
        }
        else
        {
            return currentRoom.section;
        }
    }

    private void AddExitToCurrentRoom(Room currentRoom, Vector2Int nextRoomDirection)
    {
        currentRoom.exits.Add(nextRoomDirection);
    }

    private void AddExitToNextRoom(Room nextRoom, Vector2Int nextRoomDirection)
    {
        nextRoom.exits.Add(-nextRoomDirection);
    }

    private (Vector2Int, Room, Vector2Int) GenerateEndRoomPosition(Level level)
    {
        int higherSection = level.GetHigherSection();

        // Extract all rooms of the higher section.
        List<Room> higherSectionRooms = new List<Room>();
        for (int x = 0; x < level.rooms.GetLength(0); x++)
        {
            for (int y = 0; y < level.rooms.GetLength(1); y++)
            {
                if (level.rooms[x, y] != null && level.rooms[x, y].section == higherSection)
                {
                    higherSectionRooms.Add(level.rooms[x, y]);
                }
            }
        }

        // Choose the first room with an available exit.
        for (int i = 0; i < higherSectionRooms.Count; i++)
        {
            Room room = higherSectionRooms[i];

            if (!room.exits.Contains(Vector2Int.up) && level.rooms[room.position.x, Mathf.Min(room.position.y + 1, level.rooms.GetLength(1) - 1)] == null)
            {
                return (new Vector2Int(room.position.x, Mathf.Min(room.position.y + 1, level.rooms.GetLength(1) - 1)), room, Vector2Int.up);
            }
            if (!room.exits.Contains(Vector2Int.right) && level.rooms[Mathf.Min(room.position.x + 1, level.rooms.GetLength(0) - 1), room.position.y] == null)
            {
                return (new Vector2Int(Mathf.Min(room.position.x + 1, level.rooms.GetLength(0) - 1), room.position.y), room, Vector2Int.right);
            }
            if (!room.exits.Contains(Vector2Int.down) && level.rooms[room.position.x, Mathf.Max(0, room.position.y - 1)] == null)
            {
                return (new Vector2Int(room.position.x, Mathf.Max(0, room.position.y - 1)), room, Vector2Int.down);
            }
            if (!room.exits.Contains(Vector2Int.left) && level.rooms[Mathf.Max(0, room.position.x - 1), room.position.y] == null)
            {
                return (new Vector2Int(Mathf.Max(0, room.position.x - 1), room.position.y), room, Vector2Int.left);
            }
        }

        // Throw an exception if no appropriate room has been found.
        throw new GeneratorException("Cannot generate end room position");
    }
}
