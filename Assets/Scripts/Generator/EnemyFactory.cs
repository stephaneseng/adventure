using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public void InstantiateEnemies(Spawnable[,] spawnables, GameObject room)
    {
        int initialNumberOfEnemies = 0;

        for (int x = 0; x < spawnables.GetLength(0); x++)
        for (int y = 0; y < spawnables.GetLength(1); y++)
        {
            if (spawnables[x, y] == null || spawnables[x, y] is not Enemy) continue;

            InstantiateEnemy((Enemy)spawnables[x, y], room);
            initialNumberOfEnemies++;
        }

        room.GetComponent<RoomController>().InitializeInitialNumberOfEnemies(initialNumberOfEnemies);
    }

    private void InstantiateEnemy(Enemy enemy, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(GameConstants.ResourceEnemyFolder + "/" + enemy.EnemyType),
            room.GetComponent<RoomController>().SpawnableOrigin.position + new Vector3(enemy.Position.x, enemy.Position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
