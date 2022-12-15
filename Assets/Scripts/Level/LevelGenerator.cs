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

    private static RoomConfiguration.RoomType[] NextRoomTypeChoices = new RoomConfiguration.RoomType[] {
        RoomConfiguration.RoomType.Room03_Empty,
        RoomConfiguration.RoomType.Room0301_Plus
    };

    private static int MaxNumberOfEnemies = 3;

    private static EnemyType[] EnemyTypeChoices = new EnemyType[] {
        EnemyType.Enemy
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
            GenerateStartRoomConfiguration(startRoomPosition);
        roomConfigurations[endRoomPosition.x, endRoomPosition.y] =
            GenerateEndRoomConfiguration(endRoomPosition);

        do
        {
            // Generate a next room for the start room cluster.
            (RoomConfiguration clusterRoomConfiguration, RoomConfiguration nextRoomConfiguration) =
                GenerateNextRoomConfiguration(startRoomCluster, roomConfigurations);

            startRoomCluster.Add(nextRoomConfiguration.position);

            roomConfigurations[clusterRoomConfiguration.position.x, clusterRoomConfiguration.position.y] =
                clusterRoomConfiguration;
            roomConfigurations[nextRoomConfiguration.position.x, nextRoomConfiguration.position.y] =
                nextRoomConfiguration;

            // Generate a next room for the end room cluster.
            (clusterRoomConfiguration, nextRoomConfiguration) =
                GenerateNextRoomConfiguration(endRoomCluster, roomConfigurations);

            endRoomCluster.Add(nextRoomConfiguration.position);

            roomConfigurations[clusterRoomConfiguration.position.x, clusterRoomConfiguration.position.y] =
                clusterRoomConfiguration;
            roomConfigurations[nextRoomConfiguration.position.x, nextRoomConfiguration.position.y] =
                nextRoomConfiguration;
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

    private RoomConfiguration GenerateStartRoomConfiguration(Vector2Int startRoomPosition)
    {
        RoomConfiguration roomConfiguration = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration.type = RoomConfiguration.RoomType.Room01_Start;
        roomConfiguration.position = startRoomPosition;
        return roomConfiguration;
    }

    private RoomConfiguration GenerateEndRoomConfiguration(Vector2Int endRoomPosition)
    {
        RoomConfiguration roomConfiguration = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration.type = RoomConfiguration.RoomType.Room02_End;
        roomConfiguration.position = endRoomPosition;
        return roomConfiguration;
    }

    /// Returns the room configurations of a chosen room of the cluster and of the next room, adjacent to it
    private (RoomConfiguration, RoomConfiguration) GenerateNextRoomConfiguration(HashSet<Vector2Int> roomCluster,
        RoomConfiguration[,] roomConfigurations)
    {
        Vector2Int clusterRoomPosition = GenerateRandomClusterRoomPosition(roomCluster);
        Vector2Int nextRoomDirection = GenerateNextRoomDirection();
        Vector2Int nextRoomPosition = GenerateNextRoomPosition(clusterRoomPosition, nextRoomDirection);

        if (nextRoomPosition != clusterRoomPosition)
        {
            return (
                UpdateClusterRoomConfiguration(roomConfigurations[clusterRoomPosition.x, clusterRoomPosition.y],
                    nextRoomDirection),
                GenerateOrUpdateNextRoomConfiguration(roomConfigurations[nextRoomPosition.x, nextRoomPosition.y],
                    nextRoomDirection, nextRoomPosition));
        }
        else
        {
            // No modifications.
            return (
                roomConfigurations[clusterRoomPosition.x, clusterRoomPosition.y],
                roomConfigurations[nextRoomPosition.x, nextRoomPosition.y]);
        }
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
            roomConfiguration.upDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.right)
        {
            roomConfiguration.rightExit = true;
            roomConfiguration.rightDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.down)
        {
            roomConfiguration.downExit = true;
            roomConfiguration.downDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.left)
        {
            roomConfiguration.leftExit = true;
            roomConfiguration.leftDoor = true;
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
            roomConfiguration.type = NextRoomTypeChoices[Random.Range(0, NextRoomTypeChoices.Length)];
            roomConfiguration.position = nextRoomPosition;
            roomConfiguration.enemyTypes = GenerateEnemyTypes();
        }

        if (nextRoomDirection == Vector2Int.up)
        {
            roomConfiguration.downExit = true;
            roomConfiguration.downDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.right)
        {
            roomConfiguration.leftExit = true;
            roomConfiguration.leftDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.down)
        {
            roomConfiguration.upExit = true;
            roomConfiguration.upDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.left)
        {
            roomConfiguration.rightExit = true;
            roomConfiguration.rightDoor = true;
        }

        return roomConfiguration;
    }

    private List<EnemyType> GenerateEnemyTypes()
    {
        List<EnemyType> enemyTypes = new List<EnemyType>();

        int numberOfEnemies = Random.Range(0, MaxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemyTypes.Add(EnemyTypeChoices[Random.Range(0, EnemyTypeChoices.Length)]);
        }

        return enemyTypes;
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
