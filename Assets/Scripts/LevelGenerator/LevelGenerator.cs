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

    private static int SpawnAreaWidthHeight = 8;

    private static int SpawnAreaWidthHeightOffset = 2;

    private static int MinNumberOfBlocks = 5;

    private static int MaxNumberOfBlocks = 10;

    private static int MaxNumberOfEnemies = 10;

    private static EnemyType[] EnemyTypeChoices = new EnemyType[] {
        EnemyType.Enemy
    };

    public LevelDefinition Generate()
    {
        Vector2Int startRoomPosition = GenerateRandomRoomPosition();
        Vector2Int endRoomPosition = GenerateEndRoomPosition(startRoomPosition);

        List<RoomDefinition> roomDefinitions = GenerateRoomDefinitions(startRoomPosition, endRoomPosition);

        return GenerateLevelDefinition(startRoomPosition, endRoomPosition, roomDefinitions);
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

    /// Returns a collection of rooms chosen so that a path exists between the start and the end rooms.
    private List<RoomDefinition> GenerateRoomDefinitions(Vector2Int startRoomPosition, Vector2Int endRoomPosition)
    {
        RoomDefinition[,] roomDefinitions = new RoomDefinition[LevelController.MapWidth, LevelController.MapHeight];

        HashSet<Vector2Int> startRoomCluster = new HashSet<Vector2Int>() { startRoomPosition };
        HashSet<Vector2Int> endRoomCluster = new HashSet<Vector2Int>() { endRoomPosition };

        roomDefinitions[startRoomPosition.x, startRoomPosition.y] =
            GenerateStartAndEndRoomDefinition(startRoomPosition);
        roomDefinitions[endRoomPosition.x, endRoomPosition.y] =
            GenerateStartAndEndRoomDefinition(endRoomPosition);

        do
        {
            // Generate a next room for the start room cluster.
            (RoomDefinition roomInClusterDefinition, RoomDefinition nextRoomDefinition) =
                GenerateNextRoomDefinition(startRoomCluster, roomDefinitions, endRoomPosition);

            startRoomCluster.Add(nextRoomDefinition.position);

            roomDefinitions[roomInClusterDefinition.position.x, roomInClusterDefinition.position.y] = roomInClusterDefinition;
            roomDefinitions[nextRoomDefinition.position.x, nextRoomDefinition.position.y] = nextRoomDefinition;

            // Generate a next room for the end room cluster.
            (roomInClusterDefinition, nextRoomDefinition) =
                GenerateNextRoomDefinition(endRoomCluster, roomDefinitions, endRoomPosition);

            endRoomCluster.Add(nextRoomDefinition.position);

            roomDefinitions[roomInClusterDefinition.position.x, roomInClusterDefinition.position.y] = roomInClusterDefinition;
            roomDefinitions[nextRoomDefinition.position.x, nextRoomDefinition.position.y] = nextRoomDefinition;
        } while (!startRoomCluster.Overlaps(endRoomCluster));

        List<RoomDefinition> roomDefinitionsAsList = new List<RoomDefinition>();

        for (int x = 0; x < roomDefinitions.GetLength(0); x++)
        {
            for (int y = 0; y < roomDefinitions.GetLength(1); y++)
            {
                if (roomDefinitions[x, y] == null)
                {
                    continue;
                }

                roomDefinitionsAsList.Add(roomDefinitions[x, y]);
            }
        }

        return roomDefinitionsAsList;
    }

    private RoomDefinition GenerateStartAndEndRoomDefinition(Vector2Int startRoomPosition)
    {
        RoomDefinition roomDefinition = new RoomDefinition();
        roomDefinition.position = startRoomPosition;
        return roomDefinition;
    }

    /// Returns the room definitions of a chosen room of the cluster and of the next room, adjacent to it.
    private (RoomDefinition, RoomDefinition) GenerateNextRoomDefinition(HashSet<Vector2Int> roomCluster,
        RoomDefinition[,] roomDefinitions, Vector2Int endRoomPosition)
    {
        Vector2Int roomInClusterPosition = GenerateRandomClusterRoomPosition(roomCluster);
        Vector2Int nextRoomDirection = GenerateNextRoomDirection();
        Vector2Int nextRoomPosition = GenerateNextRoomPosition(roomInClusterPosition, nextRoomDirection);

        if (nextRoomPosition != roomInClusterPosition
            && ((roomInClusterPosition != endRoomPosition && nextRoomPosition != endRoomPosition) || CheckIfEndRoomHasNoExit(roomDefinitions, endRoomPosition)))
        {
            return (
                UpdateClusterRoomDefinition(roomDefinitions[roomInClusterPosition.x, roomInClusterPosition.y],
                    nextRoomDirection),
                GenerateOrUpdateNextRoomDefinition(roomDefinitions[nextRoomPosition.x, nextRoomPosition.y],
                    nextRoomDirection, nextRoomPosition));
        }
        else
        {
            // No modifications so return the room definition of the room in the cluster twice as we are sure it exists.
            return (roomDefinitions[roomInClusterPosition.x, roomInClusterPosition.y],
                roomDefinitions[roomInClusterPosition.x, roomInClusterPosition.y]);
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

    private bool CheckIfEndRoomHasNoExit(RoomDefinition[,] roomDefinitions, Vector2Int endRoomPosition)
    {
        RoomDefinition endRoomDefinition = roomDefinitions[endRoomPosition.x, endRoomPosition.y];

        return !endRoomDefinition.upExit && !endRoomDefinition.rightExit && !endRoomDefinition.downExit
            && !endRoomDefinition.leftExit;
    }

    /// Updates the existing room to ensure it has an exit leading to the next room.
    private RoomDefinition UpdateClusterRoomDefinition(RoomDefinition roomDefinition, Vector2Int nextRoomDirection)
    {
        if (nextRoomDirection == Vector2Int.up)
        {
            roomDefinition.upExit = true;
            roomDefinition.upDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.right)
        {
            roomDefinition.rightExit = true;
            roomDefinition.rightDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.down)
        {
            roomDefinition.downExit = true;
            roomDefinition.downDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.left)
        {
            roomDefinition.leftExit = true;
            roomDefinition.leftDoor = true;
        }

        return roomDefinition;
    }

    /// Creates or updates the next room and ensures it has an exit leading to the existing room.
    private RoomDefinition GenerateOrUpdateNextRoomDefinition(RoomDefinition roomDefinition,
        Vector2Int nextRoomDirection, Vector2Int nextRoomPosition)
    {
        if (roomDefinition == null)
        {
            roomDefinition = new RoomDefinition();
            roomDefinition.position = nextRoomPosition;
            roomDefinition.blockDefinitions = GenerateBlockDefinitions();
            roomDefinition.enemyDefinitions = GenerateEnemyDefinitions(roomDefinition.blockDefinitions);
        }

        if (nextRoomDirection == Vector2Int.up)
        {
            roomDefinition.downExit = true;
            roomDefinition.downDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.right)
        {
            roomDefinition.leftExit = true;
            roomDefinition.leftDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.down)
        {
            roomDefinition.upExit = true;
            roomDefinition.upDoor = true;
        }
        else if (nextRoomDirection == Vector2Int.left)
        {
            roomDefinition.rightExit = true;
            roomDefinition.rightDoor = true;
        }

        return roomDefinition;
    }

    private List<BlockDefinition> GenerateBlockDefinitions()
    {
        List<BlockDefinition> blockDefinitions = new List<BlockDefinition>();

        int numberOfBlocks = Random.Range(MinNumberOfBlocks, MaxNumberOfBlocks + 1);

        for (int i = 0; i < numberOfBlocks; i++)
        {
            BlockDefinition blockDefinition = new BlockDefinition();
            blockDefinition.position = GenerateSpawnPosition(blockDefinitions);

            blockDefinitions.Add(blockDefinition);
        }

        return blockDefinitions;
    }

    private Vector2Int GenerateSpawnPosition(IEnumerable<SpawnDefinition> spawnDefinitions)
    {
        Vector2Int spawnPosition;

        do
        {
            spawnPosition = new Vector2Int(
                Random.Range(SpawnAreaWidthHeightOffset, SpawnAreaWidthHeightOffset + SpawnAreaWidthHeight),
                Random.Range(SpawnAreaWidthHeightOffset, SpawnAreaWidthHeightOffset + SpawnAreaWidthHeight));
        } while (spawnDefinitions.Any(spawnDefinition => spawnDefinition.position == spawnPosition));

        return spawnPosition;
    }

    private List<EnemyDefinition> GenerateEnemyDefinitions(IEnumerable<SpawnDefinition> spawnDefinitions)
    {
        List<EnemyDefinition> enemyDefinitions = new List<EnemyDefinition>();

        int numberOfEnemies = Random.Range(0, MaxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            EnemyDefinition enemyDefinition = new EnemyDefinition();
            enemyDefinition.position = GenerateSpawnPosition(spawnDefinitions.Union(enemyDefinitions).ToList());
            enemyDefinition.enemyType = EnemyTypeChoices[Random.Range(0, EnemyTypeChoices.Length)];

            enemyDefinitions.Add(enemyDefinition);
        }

        return enemyDefinitions;
    }

    private LevelDefinition GenerateLevelDefinition(Vector2Int startRoomPosition, Vector2Int endRoomPosition,
        List<RoomDefinition> roomDefinitions)
    {
        LevelDefinition levelDefinition = new LevelDefinition();
        levelDefinition.roomDefinitions = roomDefinitions;
        levelDefinition.startRoomPosition = startRoomPosition;
        levelDefinition.endRoomPosition = endRoomPosition;

        return levelDefinition;
    }
}
