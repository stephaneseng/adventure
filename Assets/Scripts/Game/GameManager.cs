using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject level;
    private LevelGenerator levelGenerator;
    private RoomFactory roomFactory;

    void Awake()
    {
        level = GameObject.FindGameObjectWithTag("Level");
        levelGenerator = GetComponentInChildren<LevelGenerator>();
        roomFactory = GetComponentInChildren<RoomFactory>();
    }

    void Start()
    {
        // To simplify room mechanics debugging, the level generator is not called if a room has been manually added.
        if (!GameObject.FindGameObjectWithTag("Room"))
        {
            LevelDefinition levelDefinition = levelGenerator.Generate();

            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.startRoomPosition = levelDefinition.startRoomPosition;
            levelData.endRoomPosition = levelDefinition.endRoomPosition;
            level.GetComponent<LevelController>().levelData = levelData;

            roomFactory.InstantiateRooms(levelDefinition.roomDefinitions, level);
        }

        level.GetComponent<LevelController>().Initialize();
    }
}
