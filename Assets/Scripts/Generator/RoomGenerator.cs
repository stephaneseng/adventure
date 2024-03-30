using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private static readonly EnemyType[] EnemyTypeChoices = new EnemyType[] {
        EnemyType.EnemyPlus,
        EnemyType.EnemyTriangle
    };

    private static readonly EnemyType[] BossEnemyTypeChoices = new EnemyType[] {
        EnemyType.EnemyBossTriangle
    };

    public Room GenerateStartRoom(Vector2Int position, GeneratorConfiguration configuration)
    {
        return new Room(configuration.roomWidthHeight)
        {
            position = position,
            section = 0
        };
    }

    public Room Generate(Vector2Int position, int section, GeneratorConfiguration configuration)
    {
        Room room = new Room(configuration.roomWidthHeight)
        {
            position = position,
            section = section
        };

        GenerateBlocks(room, configuration);
        GenerateEnemies(room, configuration);

        return room;
    }

    public Room GenerateEndRoom(Vector2Int position, int section, GeneratorConfiguration configuration)
    {
        Room room = new Room(configuration.roomWidthHeight)
        {
            position = position,
            section = section
        };

        GenerateBossEnemy(room, configuration);

        return room;
    }

    private void GenerateBlocks(Room room, GeneratorConfiguration configuration)
    {
        int numberOfBlocks = Random.Range(configuration.minNumberOfBlocks, configuration.maxNumberOfBlocks + 1);

        for (int i = 0; i < numberOfBlocks; i++)
        {
            room.AddSpawnable(new Block()
            {
                position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.blockSpawnMargin,
                    true)
            });
        }
    }

    private void GenerateEnemies(Room room, GeneratorConfiguration configuration)
    {
        int numberOfEnemies = Random.Range(configuration.minNumberOfEnemies, configuration.maxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            room.AddSpawnable(new Enemy()
            {
                position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.enemySpawnMargin,
                    false),
                enemyType = EnemyTypeChoices[Random.Range(0, EnemyTypeChoices.Length)]
            });
        }
    }

    private void GenerateBossEnemy(Room room, GeneratorConfiguration configuration)
    {
        room.AddSpawnable(new Enemy()
        {
            position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.enemySpawnMargin,
                false),
            enemyType = BossEnemyTypeChoices[Random.Range(0, BossEnemyTypeChoices.Length)]
        });
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
        room.AddSpawnable(new Item()
        {
            itemType = ItemType.ItemKey,
            position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.blockSpawnMargin,
                false)
        });
    }

    public void AddMap(Room room, GeneratorConfiguration configuration)
    {
        room.AddSpawnable(new Item()
        {
            itemType = ItemType.ItemMap,
            position = GenerateSpawnPosition(room, configuration.roomWidthHeight, configuration.blockSpawnMargin,
                false)
        });
    }
}
