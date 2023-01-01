using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    public Vector2Int direction;

    private GameObject player;
    private GameObject level;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        level = GameObject.FindGameObjectWithTag("Level");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player.GetComponent<PlayerController>().keys > 0)
            {
                UnlockDoor();
            }
        }
    }

    private void UnlockDoor()
    {
        level.GetComponent<LevelController>().UnlockDoor(direction);
        player.GetComponent<PlayerController>().RemoveKey();
    }
}
