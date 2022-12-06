using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelFactory levelFactory;

    void Awake()
    {
        levelFactory = GetComponentInChildren<LevelFactory>();
    }

    void Start()
    {
        if (!GameObject.FindGameObjectWithTag("Level"))
        {
            InstantiateLevel();
        }
    }

    private void InstantiateLevel()
    {
        LevelConfiguration levelConfiguration = ScriptableObject.CreateInstance<LevelConfiguration>();

        List<RoomConfiguration> roomConfigurations = new List<RoomConfiguration>();

        RoomConfiguration roomConfiguration10 = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration10.position = new Vector2Int(1, 0);
        roomConfiguration10.upExit = true;
        roomConfigurations.Add(roomConfiguration10);

        RoomConfiguration roomConfiguration11 = ScriptableObject.CreateInstance<RoomConfiguration>();
        roomConfiguration11.position = new Vector2Int(1, 1);
        roomConfiguration11.downExit = true;
        roomConfigurations.Add(roomConfiguration11);

        levelConfiguration.roomConfigurations = roomConfigurations;

        levelConfiguration.startRoomPosition = new Vector2Int(1, 0);

        levelFactory.Instantiate(levelConfiguration);
    }
}
