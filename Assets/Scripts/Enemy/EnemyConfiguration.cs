using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration")]
public class EnemyConfiguration : ScriptableObject
{
    public enum EnemyType
    {
        Enemy
    };

    public EnemyConfiguration.EnemyType type;
}
