using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int health = 5;

    public float speed = 4.0f;

    public GameObject bullet;
}
