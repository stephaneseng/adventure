using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyBrain enemyBrain;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private Attack attack;
    [SerializeField] private List<GameObject> droppedItems = new();

    public EnemyBrain EnemyBrain => enemyBrain;

    public int Health => health;

    public float Speed => speed;

    public Attack Attack => attack;

    public List<GameObject> DroppedItems => droppedItems;
}
