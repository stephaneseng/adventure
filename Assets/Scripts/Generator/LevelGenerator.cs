using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private static readonly Vector2Int[] NextRoomDirectionChoices =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    private RoomGenerator roomGenerator;

    private void Awake()
    {
        roomGenerator = GetComponentInChildren<RoomGenerator>();
    }

    public Level Generate(GeneratorConfiguration configuration)
    {
        Level level = new Level(configuration.LevelMapWidthHeight);

        // Create the start room.
        Vector2Int startRoomPosition = GenerateStartRoomPosition(level, configuration);
        Room startRoom = roomGenerator.GenerateStartRoom(startRoomPosition, configuration);
        level.AddStartRoom(startRoom);

        // Initialize the agent.
        Stack<Vector2Int> roomPositionsToVisit = new Stack<Vector2Int>();
        HashSet<Vector2Int> roomPositionsVisited = new HashSet<Vector2Int>();
        roomPositionsToVisit.Push(level.StartRoomPosition);

        // Run the agent which will create the next rooms.
        while (roomPositionsToVisit.Count != 0)
        {
            Vector2Int currentRoomPosition = roomPositionsToVisit.Pop();
            Room currentRoom = level.GetRoom(currentRoomPosition);

            HashSet<Vector2Int> nextRoomDirections = GenerateNextRoomDirections(level, currentRoom, configuration);

            nextRoomDirections.ToList().ForEach(nextRoomDirection =>
            {
                Vector2Int nextRoomPosition = new Vector2Int(
                    Mathf.Max(0, Mathf.Min(currentRoom.Position.x + nextRoomDirection.x, level.Rooms.GetLength(0) - 1)),
                    Mathf.Max(0, Mathf.Min(currentRoom.Position.y + nextRoomDirection.y, level.Rooms.GetLength(1) - 1)));

                // Ignore next rooms with the same position than the current one.
                if (nextRoomPosition == currentRoom.Position) return;

                Room existingRoom = level.GetRoom(nextRoomPosition);

                if (existingRoom == null)
                {
                    Room nextRoom = GenerateNextRoom(level, currentRoom, nextRoomPosition, configuration);

                    AddExitToCurrentRoom(currentRoom, nextRoom, nextRoomDirection);
                    AddExitToNextRoom(currentRoom, nextRoom, nextRoomDirection);

                    level.UpdateRoom(currentRoom);
                    level.AddRoom(nextRoom);

                    // Add the next room to the stack of rooms to visit, if it has not been visited yet.
                    roomPositionsVisited.Add(currentRoomPosition);
                    if (!roomPositionsVisited.Contains(nextRoom.Position)) roomPositionsToVisit.Push(nextRoom.Position);
                }
                else
                {
                    // Ignore next rooms already existing with a different section than the current one.
                    if (existingRoom.Section != currentRoom.Section) return;

                    AddExitToCurrentRoom(currentRoom, existingRoom, nextRoomDirection);
                    AddExitToNextRoom(currentRoom, existingRoom, nextRoomDirection);

                    level.UpdateRoom(currentRoom);
                    level.UpdateRoom(existingRoom);
                }
            });
        }

        // Create the end room if possible, with a dedicated section to put it behind a locked door.
        (Vector2Int endRoomPosition, Room parentToEndRoom, Vector2Int parentToEndRoomDirection) = GenerateEndRoomPosition(level);
        Room endRoom = roomGenerator.GenerateEndRoom(endRoomPosition, level.GetHigherSection() + 1, configuration);
        level.AddEndRoom(endRoom);

        AddExitToCurrentRoom(parentToEndRoom, endRoom, parentToEndRoomDirection);
        AddExitToNextRoom(parentToEndRoom, endRoom, parentToEndRoomDirection);
        AddExtraDoorToEndRoom(endRoom);

        level.UpdateRoom(parentToEndRoom);
        level.UpdateRoom(endRoom);

        // Add objects in appropriate rooms.
        AddKeysAndMap(level, configuration);

        return level;
    }

    private Vector2Int GenerateStartRoomPosition(Level level, GeneratorConfiguration configuration)
    {
        return new Vector2Int(
            Random.Range(configuration.LevelStartRoomMargin, level.Rooms.GetLength(0) - configuration.LevelStartRoomMargin),
            Random.Range(configuration.LevelStartRoomMargin, level.Rooms.GetLength(1) - configuration.LevelStartRoomMargin));
    }

    private HashSet<Vector2Int> GenerateNextRoomDirections(Level level, Room currentRoom,
        GeneratorConfiguration configuration)
    {
        int minNumberOfNextRoomDirections;
        int maxNumberOfNextRoomDirections;

        // For the start room, ensure that we will have exactly 1 next room.
        if (currentRoom.Position == level.StartRoomPosition)
        {
            minNumberOfNextRoomDirections = 1;
            maxNumberOfNextRoomDirections = 1;
        }
        else
        {
            int numberOfRoomsInSection = level.GetNumberOfRoomsInSection(currentRoom.Section);

            // If there are not enough rooms in the current section, ensure that there will be at least 1 next room. 
            if (numberOfRoomsInSection <= configuration.LevelNumberOfRoomsInSectionLowThreshold)
            {
                minNumberOfNextRoomDirections = 1;
                maxNumberOfNextRoomDirections = 4;
            }
            // If there are too many rooms in the current section, reduce the probability of having a high number of next rooms.
            else if (numberOfRoomsInSection >= configuration.LevelNumberOfRoomsInSectionHighThreshold)
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
        while (nextRoomDirections.Count != numberOfNextRoomDirections) nextRoomDirections.Add(NextRoomDirectionChoices[Random.Range(0, NextRoomDirectionChoices.Length)]);

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
        int numberOfRoomsInSection = level.GetNumberOfRoomsInSection(currentRoom.Section);

        // If there are not enough rooms in the current section, do not change section.
        if (numberOfRoomsInSection < configuration.LevelNumberOfRoomsInSectionHighThreshold * configuration.LevelNumberOfRoomsInSectionThresholdRatio) return currentRoom.Section;

        if (Random.Range(0, 1 + 1) == 1) return level.GetHigherSection() + 1;

        return currentRoom.Section;
    }

    private void AddExitToCurrentRoom(Room currentRoom, Room nextRoom, Vector2Int nextRoomDirection)
    {
        currentRoom.Exits.Add(nextRoomDirection);

        if (currentRoom.Section == nextRoom.Section)
            currentRoom.Doors.Add(nextRoomDirection);
        else
            currentRoom.LockedDoors.Add(nextRoomDirection);
    }

    private void AddExitToNextRoom(Room currentRoom, Room nextRoom, Vector2Int nextRoomDirection)
    {
        nextRoom.Exits.Add(-nextRoomDirection);

        if (nextRoom.Section == currentRoom.Section)
            nextRoom.Doors.Add(-nextRoomDirection);
        else
            nextRoom.LockedDoors.Add(-nextRoomDirection);
    }

    private void AddExtraDoorToEndRoom(Room endRoom)
    {
        endRoom.Doors.Add(endRoom.Exits.First());
    }

    private void AddKeysAndMap(Level level, GeneratorConfiguration configuration)
    {
        // Sort rooms by their number of exits (lower to higher) to prioritize those with lower exits for keys addition.
        // Also, count the number of locked doors by section to evaluate the number of keys to add in each section.
        Dictionary<int, List<Room>> roomsBySection = new Dictionary<int, List<Room>>();
        Dictionary<int, int> numberOfLockedDoorsBySection = new Dictionary<int, int>();

        for (int x = 0; x < level.Rooms.GetLength(0); x++)
        for (int y = 0; y < level.Rooms.GetLength(1); y++)
        {
            // Prevent adding keys in the start room.
            if (level.Rooms[x, y] == null || (x == level.StartRoomPosition.x && y == level.StartRoomPosition.y)) continue;

            Room room = level.Rooms[x, y];

            List<Room> roomsByCurrentSection = roomsBySection.GetValueOrDefault(room.Section, new List<Room>());
            roomsByCurrentSection.Add(room);
            roomsByCurrentSection.Sort((a, b) => a.Exits.Count.CompareTo(b.Exits.Count));
            roomsBySection[room.Section] = roomsByCurrentSection;

            numberOfLockedDoorsBySection[room.Section] =
                numberOfLockedDoorsBySection.GetValueOrDefault(room.Section, 0) + room.LockedDoors.Count();
        }

        // Ensure that the number of keys added by section is equal to the number of locked doors it contains.
        for (int i = 0; i < level.GetHigherSection(); i++)
        {
            int numberOfKeysToAdd;

            if (i == 0)
                numberOfKeysToAdd = numberOfLockedDoorsBySection[i];
            else
                // One locked door must be opened to enter into any of the non starting sections.
                numberOfKeysToAdd = numberOfLockedDoorsBySection[i] - 1;

            for (int j = 0; j < numberOfKeysToAdd; j++)
            {
                Room room = roomsBySection[i][j % roomsBySection[i].Count()];
                roomGenerator.AddKey(room, configuration);
            }
        }

        // Add a map item in one of the first sections.
        int mapRoomSection = Random.Range(0, level.GetHigherSection());
        Room mapRoom = roomsBySection[mapRoomSection][Random.Range(0, roomsBySection[mapRoomSection].Count())];
        roomGenerator.AddMap(mapRoom, configuration);

        // Add a triple attack item in one of the first sections.
        int tripleAttackRoomSection = Random.Range(0, level.GetHigherSection());
        Room tripleAttackRoom = roomsBySection[tripleAttackRoomSection][Random.Range(0, roomsBySection[tripleAttackRoomSection].Count())];
        roomGenerator.AddTripleBullet(tripleAttackRoom, configuration);
    }

    private (Vector2Int, Room, Vector2Int) GenerateEndRoomPosition(Level level)
    {
        int higherSection = level.GetHigherSection();

        // Extract all rooms of the higher section.
        List<Room> higherSectionRooms = new List<Room>();
        for (int x = 0; x < level.Rooms.GetLength(0); x++)
        for (int y = 0; y < level.Rooms.GetLength(1); y++)
            if (level.Rooms[x, y] != null && level.Rooms[x, y].Section == higherSection)
                higherSectionRooms.Add(level.Rooms[x, y]);

        // Choose the first room with an available exit.
        for (int i = 0; i < higherSectionRooms.Count; i++)
        {
            Room room = higherSectionRooms[i];

            if (!room.Exits.Contains(Vector2Int.up) && level.Rooms[room.Position.x, Mathf.Min(room.Position.y + 1, level.Rooms.GetLength(1) - 1)] == null)
                return (new Vector2Int(room.Position.x, Mathf.Min(room.Position.y + 1, level.Rooms.GetLength(1) - 1)), room, Vector2Int.up);
            if (!room.Exits.Contains(Vector2Int.right) && level.Rooms[Mathf.Min(room.Position.x + 1, level.Rooms.GetLength(0) - 1), room.Position.y] == null)
                return (new Vector2Int(Mathf.Min(room.Position.x + 1, level.Rooms.GetLength(0) - 1), room.Position.y), room, Vector2Int.right);
            if (!room.Exits.Contains(Vector2Int.down) && level.Rooms[room.Position.x, Mathf.Max(0, room.Position.y - 1)] == null)
                return (new Vector2Int(room.Position.x, Mathf.Max(0, room.Position.y - 1)), room, Vector2Int.down);
            if (!room.Exits.Contains(Vector2Int.left) && level.Rooms[Mathf.Max(0, room.Position.x - 1), room.Position.y] == null)
                return (new Vector2Int(Mathf.Max(0, room.Position.x - 1), room.Position.y), room, Vector2Int.left);
        }

        // Throw an exception if no appropriate room has been found.
        throw new GeneratorException("Cannot generate end room position");
    }
}
