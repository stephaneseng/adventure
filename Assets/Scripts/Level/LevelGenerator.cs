using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private static Vector2Int[] NextRoomChoices = new Vector2Int[] {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    private GameObject level;
    private RoomFactory roomFactory;

    void Awake()
    {
        level = GameObject.FindGameObjectWithTag("Level");
        roomFactory = GetComponentInChildren<RoomFactory>();
    }

    public void Generate()
    {
        Vector2Int startRoomPosition = GenerateRandomRoomPosition();
        Vector2Int endRoomPosition = GenerateEndRoomPosition(startRoomPosition);

        List<RoomConfiguration> roomConfigurations = GenerateRoomConfigurations(startRoomPosition, endRoomPosition);

        LevelConfiguration levelConfiguration = GenerateLevelConfiguration(startRoomPosition, endRoomPosition,
            roomConfigurations);
        level.GetComponent<LevelController>().levelConfiguration = levelConfiguration;

        InstantiateRooms(levelConfiguration.roomConfigurations, level);
    }

    private Vector2Int GenerateRandomRoomPosition()
    {
        return new Vector2Int(Random.Range(0, LevelController.MapWidth), Random.Range(0, LevelController.MapHeight));
    }

    private Vector2Int GenerateEndRoomPosition(Vector2Int startRoomPosition)
    {
        Vector2Int endRoomPosition;

        do
        {
            endRoomPosition = GenerateRandomRoomPosition();
        } while (endRoomPosition == startRoomPosition);

        return endRoomPosition;
    }

    /// Returns a collection of room positions chosen so that a path exists between the start and the end rooms.
    private List<RoomConfiguration> GenerateRoomConfigurations(Vector2Int startRoomPosition,
        Vector2Int endRoomPosition)
    {
        RoomConfiguration[,] roomConfigurations = new RoomConfiguration[LevelController.MapWidth,
            LevelController.MapHeight];

        HashSet<Vector2Int> startRoomCluster = new HashSet<Vector2Int>() { startRoomPosition };
        HashSet<Vector2Int> endRoomCluster = new HashSet<Vector2Int>() { endRoomPosition };

        roomConfigurations[startRoomPosition.x, startRoomPosition.y] =
            GenerateStartAndEndRoomConfiguration(startRoomPosition);
        roomConfigurations[endRoomPosition.x, endRoomPosition.y] =
            GenerateStartAndEndRoomConfiguration(endRoomPosition);

        Vector2Int clusterRoomPosition;
        Vector2Int nextRoomDirection;
        Vector2Int nextRoomPosition;

        do
        {
            // Generate a next room for the start room cluster.
            clusterRoomPosition = GenerateRandomClusterRoomPosition(startRoomCluster);
            nextRoomDirection = GenerateNextRoomDirection();
            nextRoomPosition = GenerateNextRoomPosition(clusterRoomPosition, nextRoomDirection);

            if (nextRoomPosition != clusterRoomPosition)
            {
                startRoomCluster.Add(nextRoomPosition);

                roomConfigurations[clusterRoomPosition.x, clusterRoomPosition.y] = UpdateClusterRoomConfiguration(
                    roomConfigurations[clusterRoomPosition.x, clusterRoomPosition.y], nextRoomDirection);
                roomConfigurations[nextRoomPosition.x, nextRoomPosition.y] = GenerateOrUpdateNextRoomConfiguration(
                    roomConfigurations[nextRoomPosition.x, nextRoomPosition.y], nextRoomDirection, nextRoomPosition);
            }

            // Generate a next room for the end room cluster.
            clusterRoomPosition = GenerateRandomClusterRoomPosition(endRoomCluster);
            nextRoomDirection = GenerateNextRoomDirection();
            nextRoomPosition = GenerateNextRoomPosition(clusterRoomPosition, nextRoomDirection);

            if (nextRoomPosition != clusterRoomPosition)
            {
                endRoomCluster.Add(nextRoomPosition);

                roomConfigurations[clusterRoomPosition.x, clusterRoomPosition.y] = UpdateClusterRoomConfiguration(
                    roomConfigurations[clusterRoomPosition.x, clusterRoomPosition.y], nextRoomDirection);
                roomConfigurations[nextRoomPosition.x, nextRoomPosition.y] = GenerateOrUpdateNextRoomConfiguration(
                    roomConfigurations[nextRoomPosition.x, nextRoomPosition.y], nextRoomDirection, nextRoomPosition);
            }
        } while (!startRoomCluster.Overlaps(endRoomCluster));

        List<RoomConfiguration> roomConfigurationsAsList = new List<RoomConfiguration>();

        for (int x = 0; x < roomConfigurations.GetLength(0); x++)
        {
            for (int y = 0; y < roomConfigurations.GetLength(1); y++)
            {
                if (roomConfigurations[x, y] == null)
                {
                    continue;
                }

                roomConfigurationsAsList.Add(roomConfigurations[x, y]);
            }
        }

        return roomConfigurationsAsList;
    }

    private RoomConfiguration GenerateStartAndEndRoomConfiguration(Vector2Int roomPosition)
    {
        RoomConfiguration roomConfiguration = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration.type = RoomConfiguration.RoomType.Room;
        roomConfiguration.position = roomPosition;
        return roomConfiguration;
    }

    private Vector2Int GenerateRandomClusterRoomPosition(HashSet<Vector2Int> cluster)
    {
        return cluster.ElementAt(Random.Range(0, cluster.Count));
    }

    private Vector2Int GenerateNextRoomDirection()
    {
        return NextRoomChoices[Random.Range(0, NextRoomChoices.Length)];
    }

    private Vector2Int GenerateNextRoomPosition(Vector2Int roomPosition, Vector2Int nextRoomDirection)
    {
        return new Vector2Int(
            Mathf.Min(Mathf.Max(0, roomPosition.x + nextRoomDirection.x), LevelController.MapWidth - 1),
            Mathf.Min(Mathf.Max(0, roomPosition.y + nextRoomDirection.y), LevelController.MapHeight - 1));
    }

    /// Updates the existing room to ensure it has an exit leading to the next room.
    private RoomConfiguration UpdateClusterRoomConfiguration(RoomConfiguration roomConfiguration,
        Vector2Int nextRoomDirection)
    {
        if (nextRoomDirection == Vector2Int.up)
        {
            roomConfiguration.upExit = true;
        }
        else if (nextRoomDirection == Vector2Int.right)
        {
            roomConfiguration.rightExit = true;
        }
        else if (nextRoomDirection == Vector2Int.down)
        {
            roomConfiguration.downExit = true;
        }
        else if (nextRoomDirection == Vector2Int.left)
        {
            roomConfiguration.leftExit = true;
        }

        return roomConfiguration;
    }

    /// Creates or updates the next room and ensures it has an exit leading to the existing room.
    private RoomConfiguration GenerateOrUpdateNextRoomConfiguration(RoomConfiguration roomConfiguration,
        Vector2Int nextRoomDirection, Vector2Int nextRoomPosition)
    {
        if (roomConfiguration == null)
        {
            roomConfiguration = ScriptableObject.CreateInstance<RoomConfiguration>();
            roomConfiguration.type = RoomConfiguration.RoomType.Room;
            roomConfiguration.position = nextRoomPosition;
        }

        if (nextRoomDirection == Vector2Int.up)
        {
            roomConfiguration.downExit = true;
        }
        else if (nextRoomDirection == Vector2Int.right)
        {
            roomConfiguration.leftExit = true;
        }
        else if (nextRoomDirection == Vector2Int.down)
        {
            roomConfiguration.upExit = true;
        }
        else if (nextRoomDirection == Vector2Int.left)
        {
            roomConfiguration.rightExit = true;
        }

        return roomConfiguration;
    }

    private LevelConfiguration GenerateLevelConfiguration(Vector2Int startRoomPosition, Vector2Int endRoomPosition,
        List<RoomConfiguration> roomConfigurations)
    {
        LevelConfiguration levelConfiguration = ScriptableObject.CreateInstance<LevelConfiguration>();
        levelConfiguration.roomConfigurations = roomConfigurations;
        levelConfiguration.startRoomPosition = startRoomPosition;
        levelConfiguration.endRoomPosition = endRoomPosition;

        return levelConfiguration;
    }

    private void InstantiateRooms(List<RoomConfiguration> roomConfigurations, GameObject level)
    {
        roomConfigurations.ForEach(roomConfiguration =>
        {
            GameObject room = roomFactory.Instantiate(roomConfiguration, new Vector3(
                roomConfiguration.position.x * RoomController.RoomSize,
                roomConfiguration.position.y * RoomController.RoomSize, 0.0f),
                level.transform);
            room.name = "Room(" + roomConfiguration.position.x + "," + roomConfiguration.position.y + ")";
        });
    }
}
