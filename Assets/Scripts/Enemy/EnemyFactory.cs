using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static string EnemyResourcesFolder = "Enemy";

    public void Instantiate(EnemyType enemyType, Vector2 position, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(EnemyResourcesFolder + "/" + enemyType.ToString()), position,
            Quaternion.identity, room.transform);
    }
}
