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
            GameObject room = roomFactory.Instantiate(roomConfiguration, new Vector3(
                roomConfiguration.position.x * RoomController.RoomSize,
                roomConfiguration.position.y * RoomController.RoomSize, 0.0f),
                level.GetComponentInChildren<Grid>().transform);
            room.name = "Room(" + roomConfiguration.position.x + "," + roomConfiguration.position.y + ")";

            // Disable rooms at their instantiation to avoid triggering colliders too early.
            room.SetActive(false);
        });
    }
}
