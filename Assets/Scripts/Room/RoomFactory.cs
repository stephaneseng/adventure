using System.Collections.Generic;
using System.Linq;
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

        InstantiateEnemies(roomConfiguration.enemyTypes, room);

        return room;
    }

    private void InstantiateEnemies(List<EnemyType> enemyTypes, GameObject room)
    {
        List<Vector2> shuffledEnemySpawnPositions = room.GetComponent<RoomController>().enemySpawnPositions
            .OrderBy(enemySpawnPosition => Random.value).ToList();

        for (int i = 0; i < enemyTypes.Count && i < shuffledEnemySpawnPositions.Count; i++)
        {
            enemyFactory.Instantiate(enemyTypes[i], room.transform.position + (Vector3)shuffledEnemySpawnPositions[i],
                room);
        }
    }
}
