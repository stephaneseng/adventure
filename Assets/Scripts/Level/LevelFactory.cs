using System.Collections.Generic;
using UnityEngine;

public class LevelFactory : MonoBehaviour
{

    private GameObject levelPrefab;
    private RoomFactory roomFactory;

    void Awake()
    {
        levelPrefab = Resources.Load<GameObject>("Level");
        roomFactory = GetComponentInChildren<RoomFactory>();
    }

    public GameObject Instantiate(LevelConfiguration levelConfiguration)
    {
        GameObject level = Instantiate(levelPrefab);

        LevelController levelController = level.GetComponent<LevelController>();
        levelController.levelConfiguration = levelConfiguration;

        InstantiateRooms(levelConfiguration.roomConfigurations, level);

        return level;
    }

    private void InstantiateRooms(List<RoomConfiguration> roomConfigurations, GameObject level)
    {
        roomConfigurations.ForEach(roomConfiguration =>
        {
            roomFactory.Instantiate(roomConfiguration, level.GetComponentInChildren<Grid>().transform);
        });
    }
}
