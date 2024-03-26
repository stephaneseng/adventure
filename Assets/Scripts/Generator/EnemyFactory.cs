using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private static readonly string EnemyResourcesFolder = "Enemy";

    public void InstantiateEnemies(Spawnable[,] spawnables, GameObject room)
    {
        for (int x = 0; x < spawnables.GetLength(0); x++)
        {
            for (int y = 0; y < spawnables.GetLength(1); y++)
            {
                if (spawnables[x, y] == null || spawnables[x, y] is not Enemy)
                {
                    continue;
                }

                InstantiateEnemy((Enemy)spawnables[x, y], room);
            }
        }
    }

    private void InstantiateEnemy(Enemy enemy, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(EnemyResourcesFolder + "/" + enemy.enemyType.ToString()),
            room.GetComponent<RoomController>().spawnableOrigin.position
            + new Vector3(enemy.position.x, enemy.position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
