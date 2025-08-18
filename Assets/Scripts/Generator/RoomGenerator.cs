using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private static readonly EnemyType[] EnemyTypeChoices =
    {
        EnemyType.EnemyPlus,
        EnemyType.EnemyTriangle
    };

    private static readonly EnemyType[] BossEnemyTypeChoices =
    {
        EnemyType.EnemyBossTriangle
    };

    public Room GenerateStartRoom(Vector2Int position, GeneratorConfiguration configuration)
    {
        return new Room(position, 0, configuration.RoomWidthHeight);
    }

    public Room Generate(Vector2Int position, int section, GeneratorConfiguration configuration)
    {
        Room room = new Room(position, section, configuration.RoomWidthHeight);

        GenerateBlocks(room, configuration);
        GenerateEnemies(room, configuration);

        return room;
    }

    public Room GenerateEndRoom(Vector2Int position, int section, GeneratorConfiguration configuration)
    {
        Room room = new Room(position, section, configuration.RoomWidthHeight);

        GenerateBossEnemy(room, configuration);

        return room;
    }

    private void GenerateBlocks(Room room, GeneratorConfiguration configuration)
    {
        int numberOfBlocks = Random.Range(configuration.RoomMinNumberOfBlocks, configuration.RoomMaxNumberOfBlocks + 1);

        for (int i = 0; i < numberOfBlocks; i++)
            room.AddSpawnable(new Block(GenerateSpawnPosition(room, configuration.RoomWidthHeight, configuration.RoomBlockSpawnMargin, true)));
    }

    private void GenerateEnemies(Room room, GeneratorConfiguration configuration)
    {
        int numberOfEnemies = Random.Range(configuration.RoomMinNumberOfEnemies, configuration.RoomMaxNumberOfEnemies + 1);

        for (int i = 0; i < numberOfEnemies; i++)
            room.AddSpawnable(new Enemy(GenerateSpawnPosition(room, configuration.RoomWidthHeight, configuration.RoomEnemySpawnMargin, false),
                EnemyTypeChoices[Random.Range(0, EnemyTypeChoices.Length)]));
    }

    private void GenerateBossEnemy(Room room, GeneratorConfiguration configuration)
    {
        room.AddSpawnable(new Enemy(GenerateSpawnPosition(room, configuration.RoomWidthHeight, configuration.RoomEnemySpawnMargin, false),
            BossEnemyTypeChoices[Random.Range(0, BossEnemyTypeChoices.Length)]));
    }

    private Vector2Int GenerateSpawnPosition(Room room, int roomWidthHeight, int spawnMargin, bool checkAdjacentSpawnables)
    {
        Vector2Int spawnPosition;

        do
        {
            spawnPosition = new Vector2Int(Random.Range(spawnMargin, roomWidthHeight - spawnMargin),
                Random.Range(spawnMargin, roomWidthHeight - spawnMargin));
        } while (!(room.GetSpawnable(spawnPosition) == null && (!checkAdjacentSpawnables || !room.HasAdjacentSpawnables(spawnPosition))));

        return spawnPosition;
    }

    public void AddKey(Room room, GeneratorConfiguration configuration)
    {
        room.AddSpawnable(new Item(GenerateSpawnPosition(room, configuration.RoomWidthHeight, configuration.RoomBlockSpawnMargin, false),
            ItemType.ItemKey));
    }

    public void AddMap(Room room, GeneratorConfiguration configuration)
    {
        room.AddSpawnable(new Item(GenerateSpawnPosition(room, configuration.RoomWidthHeight, configuration.RoomBlockSpawnMargin, false),
            ItemType.ItemMap));
    }

    public void AddTripleBullet(Room room, GeneratorConfiguration configuration)
    {
        room.AddSpawnable(new Item(GenerateSpawnPosition(room, configuration.RoomWidthHeight, configuration.RoomBlockSpawnMargin, false),
            ItemType.ItemTripleBulletAttack));
    }
}
