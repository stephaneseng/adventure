using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    [SerializeField] private Vector2Int direction;

    private GameObject player;
    private GameObject level;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        level = GameObject.FindGameObjectWithTag("Level");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (player.GetComponent<PlayerController>().Keys > 0)
                UnlockDoor();
    }

    private void UnlockDoor()
    {
        level.GetComponent<LevelController>().UnlockDoor(direction);
        player.GetComponent<PlayerController>().RemoveKey();

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>(GameConstants.ResourceAudioFolder + "/" + GameConstants.ResourceAudioDoorName), transform.position);
    }
}
