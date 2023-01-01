using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private static EnemyType[] EnemyTypeChoices = new EnemyType[] {
        EnemyType.EnemyPlus,
        EnemyType.EnemyTriangle
    };

    public Room GenerateStartRoom(Vector2Int position, GeneratorConfiguration configuration)
    {
        Room startRoom = new Room(configuration.roomWidthHeight);

        startRoom.position = position;
        startRoom.section = 0;

        return startRoom;
    }

    public Room GenerateEndRoom(Vector2Int position, int section, GeneratorConfiguration configuration)
    {
        Room endRoom = new Room(configuration.roomWidthHeight);

        endRoom.position = position;
        endRoom.section = section;

        return endRoom;
    }

    public Room Generate(Vector2Int position, int section, GeneratorConfiguration configuration)
    {
        Room room = new Room(configuration.roomWidthHeight);

        room.position = position;
        room.section = section;

        GenerateBlocks(room, configuration);
        GenerateEnemies(room, configuration);

        return room;
    }

    private void GenerateBlocks(Room room, GeneratorConfiguration configuration)
    {
        int numberOfBlocks = Random.Range(configuration.minNumberOfBlocks, configuration.maxNumberOfBlocks + 1);

        for (int i = 0; i < numberOfBlocks; i++)
        {
            Block block = new Block();
            block.position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.blockSpawnMargin,
                true);

            room.AddSpawnable(block);
        }
    }

    private void GenerateEnemies(Room room, GeneratorConfiguration configuration)
    {
        int numberOfEnemies = Random.Range(configuration.minNumberOfEnemies, configuration.maxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Enemy enemy = new Enemy();
            enemy.position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.enemySpawnMargin,
                false);
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

    public void AddKey(Room room, GeneratorConfiguration configuration)
    {
        Item item = new Item();
        item.position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.blockSpawnMargin,
            false);

        room.AddSpawnable(item);
    }
}
