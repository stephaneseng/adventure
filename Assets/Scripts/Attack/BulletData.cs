using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    [SerializeField] private float range = 2.0f;

    [SerializeField] private float speed = 16.0f;

    public float Range => range;

    public float Speed => speed;
}
