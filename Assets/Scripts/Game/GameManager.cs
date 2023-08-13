using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly int MapWidthHeight = 10;
    private static readonly int StartRoomMargin = 2;
    private static readonly int NumberOfRoomsInSectionLowThreshold = 3;
    private static readonly int NumberOfRoomsInSectionHighThreshold = 8;
    private static readonly float NumberOfRoomsInSectionThresholdRatio = 0.9f;

    private static readonly int RoomWidthHeight = 10;
    private static readonly int BlockSpawnMargin = 1;
    private static readonly int MinNumberOfBlocks = 5;
    private static readonly int MaxNumberOfBlocks = 10;
    private static readonly int EnemySpawnMargin = 2;
    private static readonly int MinNumberOfEnemies = 0;
    private static readonly int MaxNumberOfEnemies = 5;

    private GameObject levelGameObject;
    private LevelGenerator levelGenerator;
    private RoomFactory roomFactory;

    void Awake()
    {
        levelGameObject = GameObject.FindGameObjectWithTag("Level");
        levelGenerator = GetComponentInChildren<LevelGenerator>();
        roomFactory = GetComponentInChildren<RoomFactory>();
    }

    void Start()
    {
        // To simplify room mechanics debugging, the level generator is not called if a room is already in the scene.
        if (!GameObject.FindGameObjectWithTag("Room"))
        {
            GeneratorConfiguration configuration = new GeneratorConfiguration(MapWidthHeight, StartRoomMargin,
                NumberOfRoomsInSectionLowThreshold, NumberOfRoomsInSectionHighThreshold,
                NumberOfRoomsInSectionThresholdRatio, RoomWidthHeight, BlockSpawnMargin, MinNumberOfBlocks,
                MaxNumberOfBlocks, EnemySpawnMargin, MinNumberOfEnemies, MaxNumberOfEnemies);

            Level level;

            do
            {
                try
                {
                    level = levelGenerator.Generate(configuration);
                }
                catch (GeneratorException)
                {
                    level = null;
                }
            }
            while (!(level != null && level.GetHigherSection() > 1));

            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.Initialize(level);
            levelGameObject.GetComponent<LevelController>().levelData = levelData;

            roomFactory.InstantiateRooms(level.rooms, levelGameObject);
        }

        levelGameObject.GetComponent<LevelController>().Initialize();

        levelGameObject.GetComponent<LevelController>().EnterStartRoom();
    }
}
