using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject levelGameObject;
    private LevelGenerator levelGenerator;
    private RoomFactory roomFactory;

    private void Awake()
    {
        levelGameObject = GameObject.FindGameObjectWithTag("Level");
        levelGenerator = GetComponentInChildren<LevelGenerator>();
        roomFactory = GetComponentInChildren<RoomFactory>();
    }

    private void Start()
    {
        // To simplify room mechanics debugging, the level generator is not called if a room is already in the scene.
        if (!GameObject.FindGameObjectWithTag("Room"))
        {
            GeneratorConfiguration configuration = new GeneratorConfiguration(GameConstants.LevelMapWidthHeight, GameConstants.LevelStartRoomMargin,
                GameConstants.LevelNumberOfRoomsInSectionLowThreshold, GameConstants.LevelNumberOfRoomsInSectionHighThreshold,
                GameConstants.LevelNumberOfRoomsInSectionThresholdRatio, GameConstants.RoomWidthHeight, GameConstants.RoomBlockSpawnMargin, GameConstants.RoomMinNumberOfBlocks,
                GameConstants.RoomMaxNumberOfBlocks, GameConstants.RoomEnemySpawnMargin, GameConstants.RoomMinNumberOfEnemies, GameConstants.RoomMaxNumberOfEnemies);

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
            } while (!(level != null && level.GetHigherSection() > 1));

            levelGameObject.GetComponent<LevelController>().InitializeLevelData(level);

            roomFactory.InstantiateRooms(level.Rooms, levelGameObject);
        }

        levelGameObject.GetComponent<LevelController>().InitializeAndEnterStartRoom();
    }
}
