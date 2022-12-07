using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static string EnemyResourcesFolder = "Enemy";

    public void Instantiate(EnemyConfiguration enemyConfiguration, Vector2 position, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(EnemyResourcesFolder + "/" + enemyConfiguration.type.ToString()),
            position, Quaternion.identity, room.transform);
    }
}
