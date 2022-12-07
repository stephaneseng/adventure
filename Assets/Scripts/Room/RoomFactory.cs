using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    private static string RoomResourcesFolder = "Room";

    private EnemyFactory enemyFactory;

    void Awake()
    {
        enemyFactory = GetComponentInChildren<EnemyFactory>();
    }

    public GameObject Instantiate(RoomConfiguration roomConfiguration, Vector3 position, Transform parent)
    {
        GameObject room = Instantiate(
            Resources.Load<GameObject>(RoomResourcesFolder + "/" + roomConfiguration.type.ToString()),
            position, Quaternion.identity, parent);
        room.GetComponent<RoomController>().roomConfiguration = roomConfiguration;

        InstantiateEnemies(roomConfiguration.enemyConfigurations, room);

        return room;
    }

    private void InstantiateEnemies(List<EnemyConfiguration> enemyConfigurations, GameObject room)
    {
        for (int i = 0; i < enemyConfigurations.Count && i < room.GetComponent<RoomController>().enemySpawnPositions.Length; i++)
        {
            enemyFactory.Instantiate(enemyConfigurations[i],
                room.transform.position + (Vector3)room.GetComponent<RoomController>().enemySpawnPositions[i], room);
        }
    }
}
