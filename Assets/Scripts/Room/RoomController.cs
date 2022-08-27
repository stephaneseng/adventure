using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private static float RoomSize = 5.5f;
    private static float EnemyRoomColliderSize = 4.0f;

    public GameObject upRoom;
    public bool upRoomDoor;
    public GameObject rightRoom;
    public bool rightRoomDoor;
    public GameObject downRoom;
    public bool downRoomDoor;
    public GameObject leftRoom;
    public bool leftRoomDoor;

    private GameObject door;
    private LevelManager levelManager;
    private new Camera camera;
    private BoxCollider2D boxCollider2D;

    public List<GameObject> doors;

    void Awake()
    {
        door = Resources.Load<GameObject>("Door");
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        camera = Camera.main;
        boxCollider2D = GetComponent<BoxCollider2D>();

        doors = new List<GameObject>();
    }

    void Start()
    {
        InitializeEnemyRoomCollider();
    }

    void Update()
    {
        if (doors.Count() > 0) {

            // Destroy all doors if all enemies have been destroyed.
            if (GetComponentsInChildren<Transform>().Where(gameObject => gameObject.CompareTag("Enemy")).Count() == 0) {

                for (int i = doors.Count() - 1; i >= 0; i--) {
                    Destroy(doors.ElementAt(i));
                }

                doors.Clear();
            }
        }
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
    /// The position of the room BoxCollider2D is used, instead of the position of the room itself is not relevant.
    private void InitializeEnemyRoomCollider()
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

    /// The position of the room BoxCollider2D is used, instead of the position of the room itself is not relevant.
    public void InitializeDoors()
    {
        if (GetComponentsInChildren<Transform>().Where(gameObject => gameObject.CompareTag("Enemy")).Count() == 0) {
            return;
        }

        Vector3 boxCollider2DCenter = boxCollider2D.transform.position + (Vector3) boxCollider2D.offset;

        if (upRoomDoor) {
            doors.Add(Instantiate(door, boxCollider2DCenter + (Vector3) new Vector2(0.0f, RoomSize),
                Quaternion.identity, transform));
        }
        if (rightRoomDoor) {
            doors.Add(Instantiate(door, boxCollider2DCenter + (Vector3) new Vector2(RoomSize, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
        }
        if (downRoomDoor) {
            doors.Add(Instantiate(door, boxCollider2DCenter + (Vector3) new Vector2(0.0f, -RoomSize),
                Quaternion.identity, transform));
        }
        if (leftRoomDoor) {
            doors.Add(Instantiate(door, boxCollider2DCenter + (Vector3) new Vector2(-RoomSize, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
        }
    }
}
