using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    public RoomData roomData;

    private LevelController levelController;
    private BoxCollider2D boxCollider2D;
    private Tilemap tilemap;
    public Transform spawnableOrigin;

    private Dictionary<Vector2Int, GameObject> doors = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> lockedDoors = new Dictionary<Vector2Int, GameObject>();
    public bool visited;

    void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelController>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        tilemap = GetComponentInChildren<Tilemap>();
        spawnableOrigin = transform.Find("SpawnableOrigin");
    }

    void Update()
    {
        if (doors.Count() > 0)
        {
            // Destroy all doors if all enemies have been destroyed.
            if (GetComponentsInChildren<Transform>().Where(transform => transform.CompareTag("Enemy")).Count() == 0)
            {
                doors.Values.ToList().ForEach(door =>
                {
                    Destroy(door);
                });

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
                levelController.SwitchRoom(playerController, Vector2Int.up);
            }
            else if (Vector2.Dot(contactVector, Vector2.right) > 1)
            {
                levelController.SwitchRoom(playerController, Vector2Int.right);
            }
            else if (Vector2.Dot(contactVector, Vector2.down) > 1)
            {
                levelController.SwitchRoom(playerController, Vector2Int.down);
            }
            else
            {
                levelController.SwitchRoom(playerController, Vector2Int.left);
            }
        }
    }

    public void Initialize()
    {
        InitializeWalls();
        InitializeDoors();
        InitializeLockedDoors();
    }

    private void InitializeWalls()
    {
        int roomHalfWidthHeight = Mathf.FloorToInt(roomData.roomWidthHeight / 2.0f);

        if (roomData.exits.Contains(Vector2Int.up))
        {
            tilemap.SetTile(new Vector3Int(-1, roomHalfWidthHeight), null);
            tilemap.SetTile(new Vector3Int(0, roomHalfWidthHeight), null);
        }
        if (roomData.exits.Contains(Vector2Int.right))
        {
            tilemap.SetTile(new Vector3Int(roomHalfWidthHeight, -1), null);
            tilemap.SetTile(new Vector3Int(roomHalfWidthHeight, 0), null);
        }
        if (roomData.exits.Contains(Vector2Int.down))
        {
            tilemap.SetTile(new Vector3Int(-1, -roomHalfWidthHeight - 1), null);
            tilemap.SetTile(new Vector3Int(0, -roomHalfWidthHeight - 1), null);
        }
        if (roomData.exits.Contains(Vector2Int.left))
        {
            tilemap.SetTile(new Vector3Int(-roomHalfWidthHeight - 1, -1), null);
            tilemap.SetTile(new Vector3Int(-roomHalfWidthHeight - 1, 0), null);
        }
    }

    private void InitializeDoors()
    {
        doors[Vector2Int.up] = transform.Find("Doors/DoorUp").gameObject;
        doors[Vector2Int.right] = transform.Find("Doors/DoorRight").gameObject;
        doors[Vector2Int.down] = transform.Find("Doors/DoorDown").gameObject;
        doors[Vector2Int.left] = transform.Find("Doors/DoorLeft").gameObject;

        doors.Keys.ToList().ForEach(direction =>
        {
            if (!roomData.doors.Contains(direction))
            {
                Destroy(doors[direction]);
                doors.Remove(direction);
            }
        });

        doors.Values.ToList().ForEach(door => door.SetActive(false));
    }

    private void InitializeLockedDoors()
    {
        lockedDoors[Vector2Int.up] = transform.Find("Doors/DoorLockedUp").gameObject;
        lockedDoors[Vector2Int.right] = transform.Find("Doors/DoorLockedRight").gameObject;
        lockedDoors[Vector2Int.down] = transform.Find("Doors/DoorLockedDown").gameObject;
        lockedDoors[Vector2Int.left] = transform.Find("Doors/DoorLockedLeft").gameObject;

        lockedDoors.Keys.ToList().ForEach(direction =>
        {
            if (!roomData.lockedDoors.Contains(direction))
            {
                Destroy(lockedDoors[direction]);
                lockedDoors.Remove(direction);
            }
        });

        lockedDoors.Values.ToList().ForEach(lockedDoor => lockedDoor.SetActive(false));
    }

    public void StartEnterRoomTransition()
    {
        gameObject.SetActive(true);
        boxCollider2D.enabled = false;
        doors.Values.ToList().ForEach(door => door.SetActive(false));
        lockedDoors.Values.ToList().ForEach(lockedDoor => lockedDoor.SetActive(false));

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
        doors.Values.ToList().ForEach(door => door.SetActive(true));
        lockedDoors.Values.ToList().ForEach(lockedDoor => lockedDoor.SetActive(true));

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

    public void UnlockDoor(Vector2Int doorDirection)
    {
        Destroy(lockedDoors[doorDirection]);
        lockedDoors.Remove(doorDirection);
    }
}
