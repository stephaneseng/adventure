using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int MapWidthHeight = 10;
    private static int StartRoomMargin = 2;
    private static int NumberOfRoomsInSectionLowThreshold = 3;
    private static int NumberOfRoomsInSectionHighThreshold = 8;
    private static float NumberOfRoomsInSectionThresholdRatio = 0.9f;

    private static int RoomWidthHeight = 10;
    private static int BlockSpawnMargin = 1;
    private static int MinNumberOfBlocks = 5;
    private static int MaxNumberOfBlocks = 10;
    private static int EnemySpawnMargin = 2;
    private static int MinNumberOfEnemies = 0;
    private static int MaxNumberOfEnemies = 5;

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
