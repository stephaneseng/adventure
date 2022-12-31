using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    public static float RoomSize = 11.0f;
    public static float RoomHalfSize = RoomSize / 2.0f;

    private static string DoorResourcesFolder = "Room";
    private static string DoorResourceName = "Door";

    public RoomData roomData;

    private GameObject doorPrefab;
    private LevelController levelController;
    private BoxCollider2D boxCollider2D;
    private Tilemap tilemap;

    public bool visited;

    private List<GameObject> doors = new List<GameObject>();

    void Awake()
    {
        doorPrefab = Resources.Load<GameObject>(DoorResourcesFolder + "/" + DoorResourceName);
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
    }

    private void InitializeWalls()
    {
        if (roomData.exits.Contains(Vector2Int.up))
        {
            tilemap.SetTile(new Vector3Int(-1, Mathf.FloorToInt(RoomHalfSize)), null);
            tilemap.SetTile(new Vector3Int(0, Mathf.FloorToInt(RoomHalfSize)), null);
        }
        if (roomData.exits.Contains(Vector2Int.right))
        {
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(RoomHalfSize), 0), null);
            tilemap.SetTile(new Vector3Int(Mathf.FloorToInt(RoomHalfSize), -1), null);
        }
        if (roomData.exits.Contains(Vector2Int.down))
        {
            tilemap.SetTile(new Vector3Int(-1, -Mathf.FloorToInt(RoomHalfSize) - 1), null);
            tilemap.SetTile(new Vector3Int(0, -Mathf.FloorToInt(RoomHalfSize) - 1), null);
        }
        if (roomData.exits.Contains(Vector2Int.left))
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

        if (roomData.doors.Contains(Vector2Int.up))
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(0.0f, RoomHalfSize),
                Quaternion.identity, transform));
        }
        if (roomData.doors.Contains(Vector2Int.right))
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(RoomHalfSize, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
        }
        if (roomData.doors.Contains(Vector2Int.down))
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(0.0f, -RoomHalfSize),
                Quaternion.identity, transform));
        }
        if (roomData.doors.Contains(Vector2Int.left))
        {
            doors.Add(Instantiate(doorPrefab, boxCollider2DCenter + (Vector3)new Vector2(-RoomHalfSize, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
        }

        doors.ForEach(door => door.SetActive(false));
    }

    public void StartEnterRoomTransition()
    {
        gameObject.SetActive(true);
        boxCollider2D.enabled = false;
        doors.ForEach(door => door.SetActive(false));

        GetComponentsInChildren<Transform>().Where(transform => transform.CompareTag("Enemy")).ToList()
            .ForEach(transform =>
            {
                transform.gameObject.GetComponent<EnemyController>().Freeze();
            });
    }

    public void EnterRoom()
    {
        gameObject.SetActive(true);
        boxCollider2D.enabled = true;
        doors.ForEach(door => door.SetActive(true));

        GetComponentsInChildren<Transform>().Where(transform => transform.CompareTag("Enemy")).ToList()
            .ForEach(transform =>
            {
                transform.gameObject.GetComponent<EnemyController>().StopFreeze();
            });

        visited = true;
    }

    public void ExitRoom()
    {
        gameObject.SetActive(false);
    }
}
