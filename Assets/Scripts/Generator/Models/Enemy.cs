using UnityEngine;

public class Enemy : Spawnable
{
    private readonly EnemyType enemyType;

    public Enemy(Vector2Int position, EnemyType enemyType) : base(position)
    {
        this.enemyType = enemyType;
    }

    public EnemyType EnemyType => enemyType;
}
