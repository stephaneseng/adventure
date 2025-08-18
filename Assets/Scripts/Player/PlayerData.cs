using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int health = 5;
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private Attack attack;

    public int Health => health;

    public float Speed => speed;

    public Attack Attack => attack;
}
