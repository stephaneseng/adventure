using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyBrain enemyBrain;

    public int health;

    public float speed;

    public GameObject bullet;

    public GameObject[] droppedItems;
}
