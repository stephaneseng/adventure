using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private static EnemyType[] EnemyTypeChoices = new EnemyType[] {
        EnemyType.PlusEnemy,
        EnemyType.TriangleEnemy
    };

    public Room GenerateStartRoom(Vector2Int position, int roomWidthHeight)
    {
        Room startRoom = new Room(roomWidthHeight);

        startRoom.position = position;
        startRoom.section = 0;

        return startRoom;
    }

    public Room GenerateEndRoom(Vector2Int position, int section, int roomWidthHeight)
    {
        Room endRoom = new Room(roomWidthHeight);

        endRoom.position = position;
        endRoom.section = section;

        return endRoom;
    }

    public Room Generate(Vector2Int position, int section, int roomWidthHeight, int blockSpawnMargin,
        int minNumberOfBlocks, int maxNumberOfBlocks, int enemySpawnMargin, int minNumberOfEnemies,
        int maxNumberOfEnemies)
    {
        Room room = new Room(roomWidthHeight);

        room.position = position;
        room.section = section;

        GenerateBlocks(room, roomWidthHeight, blockSpawnMargin, minNumberOfBlocks, maxNumberOfBlocks);
        GenerateEnemies(room, roomWidthHeight, enemySpawnMargin, minNumberOfEnemies, maxNumberOfEnemies);

        return room;
    }

    private void GenerateBlocks(Room room, int roomWidthHeight, int blockSpawnMargin, int minNumberOfBlocks,
        int maxNumberOfBlocks)
    {
        int numberOfBlocks = Random.Range(minNumberOfBlocks, maxNumberOfBlocks + 1);

        for (int i = 0; i < numberOfBlocks; i++)
        {
            Block block = new Block();
            block.position = GenerateSpawnPosition(room, roomWidthHeight, blockSpawnMargin, true);

            room.AddSpawnable(block);
        }
    }

    private void GenerateEnemies(Room room, int roomWidthHeight, int enemySpawnMargin, int minNumberOfEnemies,
        int maxNumberOfEnemies)
    {
        int numberOfEnemies = Random.Range(minNumberOfEnemies, maxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Enemy enemy = new Enemy();
            enemy.position = GenerateSpawnPosition(room, roomWidthHeight, enemySpawnMargin, false);
            enemy.enemyType = EnemyTypeChoices[Random.Range(0, EnemyTypeChoices.Length)];

            room.AddSpawnable(enemy);
        }
    }

    private Vector2Int GenerateSpawnPosition(Room room, int roomWidthHeight, int spawnMargin, bool checkAdjacentSpawnables)
    {
        Vector2Int spawnPosition;

        do
        {
            spawnPosition = new Vector2Int(Random.Range(spawnMargin, roomWidthHeight - spawnMargin),
                Random.Range(spawnMargin, roomWidthHeight - spawnMargin));
        }
        while (!(room.GetSpawnable(spawnPosition) == null && (!checkAdjacentSpawnables || !room.HasAdjacentSpawnables(spawnPosition))));

        return spawnPosition;
    }
}
