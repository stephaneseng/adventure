using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    public static float RoomSize = 11.0f;
    public static float RoomHalfSize = RoomSize / 2.0f;

    private static float EnemyRoomColliderSize = 4.0f;

    public RoomConfiguration roomConfiguration;
    public Vector2 playerSpawnPosition;
    public Vector2[] enemySpawnPositions;

    private GameObject doorPrefab;
    private LevelController levelController;
    private BoxCollider2D boxCollider2D;
    private Tilemap tilemap;

    private List<GameObject> doors = new List<GameObject>();

    void Awake()
    {
        doorPrefab = Resources.Load<GameObject>("Door");
        levelController = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelController>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        tilemap = GetComponentInChildren<Tilemap>();
    }

    void Update()
    {
        if (doors.Count() > 0)
        {
            // Destroy all doors if all enemies have been destroyed.
            if (GetComponentsInChildren<Transform>().Where(transform => transform.CompareTag("Enemy")).Count() == 0)
            {
                for (int i = doors.Count() - 1; i >= 0; i--)
                {
                    Destroy(doors.ElementAt(i));
                }

                doors.Clear();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable the collider to avoid triggering it multiple times.
            boxCollider2D.enabled = false;

            PlayerController playerController = other.GetComponent<PlayerController>();

            Vector2 contactVector = new Vector2(other.bounds.center.x - boxCollider2D.bounds.center.x,
                other.bounds.center.y - boxCollider2D.bounds.center.y);

            if (Vector2.Dot(contactVector, Vector2.up) > 1)
            {
                levelController.SwitchRoom(playerController, Vector2.up);
            }
            else if (Vector2.Dot(contactVector, Vector2.right) > 1)
            {
                levelController.SwitchRoom(playerController, Vector2.right);
            }
            else if (Vector2.Dot(contactVector, Vector2.down) > 1)
            {
                levelController.SwitchRoom(playerController, Vector2.down);
            }
            else
            {
                levelController.SwitchRoom(playerController, Vector2.left);
            }
        }
    }

    public void Initialize()
    {
        InitializeWalls();
        InitializeDoors();
        InitializeEnemyRoomCollider();
    }

    private void InitializeWalls()
    {
        if (roomConfiguration.upExit)
        {
            tilemap.SetTile(new Vector3Int(-1, Mathf.FloorToInt(RoomHalfSize)), null);
            tilemap.SetTile(new Vector3Int(0, Mathf.FloorToInt(RoomHalfSize)), null);
        }
        if (roomConfiguration.rightExit)
        {
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(RoomHalfSize), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(RoomHalfSize), -1), null);
        }
        if (roomConfiguration.downExit)
        {
            tilemap.SetTile(new Vector3Int(-1, -Mathf.FloorToInt(RoomHalfSize) - 1), null);
            tilemap.SetTile(new Vector3Int(0, -Mathf.FloorToInt(RoomHalfSize) - 1), null);
        }
        if (roomConfiguration.leftExit)
        {
            tilemap.SetTile(new Vector3Int(-Mathf.FloorToInt(RoomHalfSize) - 1, 0), null);
            tilemap.SetTile(new Vector3Int(-Mathf.FloorToInt(RoomHalfSize) - 1, -1), null);
        }
    }

    /// The position of the room BoxCollider2D is used, because the position of the room itself is not relevant.
    private void InitializeDoors()
    {
        if (GetComponentsInChildren<Transform>().Where(transform => transform.CompareTag("Enemy")).Count() == 0)
        {
            return;
        }

        Vector3 boxCollider2DCenter = boxCollider2D.transform.position + (Vector3)boxCollider2D.offset;

        if (roomConfiguration.upDoor)
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(0.0f, RoomHalfSize),
                Quaternion.identity, transform));
        }
        if (roomConfiguration.rightDoor)
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(RoomHalfSize, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
        }
        if (roomConfiguration.downDoor)
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(0.0f, -RoomHalfSize),
                Quaternion.identity, transform));
        }
        if (roomConfiguration.leftDoor)
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(-RoomHalfSize, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
        }

        doors.ForEach(door => door.SetActive(false));
    }

    /// Creates a collider to avoid enemies leaving the room.
    /// The position of the room BoxCollider2D is used, instead of the position of the room itself is not relevant.
    private void InitializeEnemyRoomCollider()
    {
        GameObject enemyRoomCollider = new GameObject();
        enemyRoomCollider.transform.SetParent(this.transform);
        enemyRoomCollider.layer = LayerMask.NameToLayer("EnemyForeground");

        Vector3 boxCollider2DCenter = boxCollider2D.transform.position + (Vector3)boxCollider2D.offset;

        EdgeCollider2D edgeRoomCollider = enemyRoomCollider.AddComponent<EdgeCollider2D>();
        edgeRoomCollider.points = new Vector2[5] {
            (Vector2) boxCollider2DCenter + new Vector2(-EnemyRoomColliderSize, EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(EnemyRoomColliderSize, EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(EnemyRoomColliderSize, -EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(-EnemyRoomColliderSize, -EnemyRoomColliderSize),
            (Vector2) boxCollider2DCenter + new Vector2(-EnemyRoomColliderSize, EnemyRoomColliderSize)
        };
    }

    public void EnterRoom()
    {
        StartEnterRoom();
        EndEnterRoom();
    }

    public void StartEnterRoom()
    {
        gameObject.SetActive(true);
        boxCollider2D.enabled = false;
        doors.ForEach(door => door.SetActive(false));
    }

    public void EndEnterRoom()
    {
        boxCollider2D.enabled = true;
        doors.ForEach(door => door.SetActive(true));
    }

    public void ExitRoom()
    {
        gameObject.SetActive(false);
    }
}
