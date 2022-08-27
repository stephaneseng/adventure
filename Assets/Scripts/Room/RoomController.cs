using UnityEngine;

public class RoomController : MonoBehaviour
{
    private static float EnemyRoomColliderSize = 4.0f;

    public GameObject upRoom;
    public GameObject rightRoom;
    public GameObject downRoom;
    public GameObject leftRoom;

    private LevelManager levelManager;
    private new Camera camera;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        camera = Camera.main;
        boxCollider2D = GetComponent<BoxCollider2D>();

        InstantiateEnemyRoomCollider();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            PlayerController playerController = other.GetComponent<PlayerController>();

            Vector2 contactVector = new Vector2(other.bounds.center.x - boxCollider2D.bounds.center.x,
                other.bounds.center.y - boxCollider2D.bounds.center.y);

            if (Vector2.Dot(contactVector, Vector2.up) > 1) {
                levelManager.SwitchRoom(playerController, Vector2.up, gameObject, upRoom);
            } else if (Vector2.Dot(contactVector, Vector2.right) > 1) {
                levelManager.SwitchRoom(playerController, Vector2.right, gameObject, rightRoom);
            } else if (Vector2.Dot(contactVector, Vector2.down) > 1) {
                levelManager.SwitchRoom(playerController, Vector2.down, gameObject, downRoom);
            } else {
                levelManager.SwitchRoom(playerController, Vector2.left, gameObject, leftRoom);
            }
        }
    }

    /// Creates a collider to avoid enemies leaving the room.
    /// This collider uses the position of the room BoxCollider2D, as the position of the room itself is less relevant.
    private void InstantiateEnemyRoomCollider()
    {
        GameObject enemyRoomColliderGameObject = new GameObject();
        enemyRoomColliderGameObject.transform.SetParent(this.transform);
        enemyRoomColliderGameObject.layer = LayerMask.NameToLayer("EnemyForeground");

        Vector3 boxCollider2DCenter = boxCollider2D.transform.position + (Vector3) boxCollider2D.offset;

        EdgeCollider2D edgeRoomCollider = enemyRoomColliderGameObject.AddComponent<EdgeCollider2D>();
        edgeRoomCollider.points = new Vector2[5] {
            (Vector2) boxCollider2DCenter + new Vector2(-EnemyRoomColliderSize, EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(EnemyRoomColliderSize, EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(EnemyRoomColliderSize, -EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(-EnemyRoomColliderSize, -EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(-EnemyRoomColliderSize, EnemyRoomColliderSize)
        };
    }
}
