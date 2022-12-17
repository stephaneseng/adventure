using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static string EnemyResourcesFolder = "Enemy";

    public void InstantiateEnemies(List<EnemyDefinition> enemyDefinitions, GameObject room)
    {
        enemyDefinitions.ForEach(enemyDefinition =>
        {
            InstantiateEnemy(enemyDefinition, room);
        });
    }

    private void InstantiateEnemy(EnemyDefinition enemyDefinition, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(EnemyResourcesFolder + "/" + enemyDefinition.enemyType.ToString()),
            room.transform.position - new Vector3(RoomController.RoomHalfSize, RoomController.RoomHalfSize, 0.0f)
            + new Vector3(enemyDefinition.position.x, enemyDefinition.position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
