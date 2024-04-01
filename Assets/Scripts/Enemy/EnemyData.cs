using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyBrain enemyBrain;

    public int health;

    public float speed;

    public Attack attack;

    public List<GameObject> droppedItems;

    public EnemyData()
    {
        droppedItems = new List<GameObject>();
    }
}
