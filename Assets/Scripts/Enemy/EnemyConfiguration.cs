using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration")]
public class EnemyConfiguration : ScriptableObject
{
    public int health;

    public float speed;

    public GameObject bullet;

    public GameObject[] droppedItems;
}
