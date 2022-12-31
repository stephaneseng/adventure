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
    private GameObject upDoor;
    private GameObject rightDoor;
    private GameObject downDoor;
    private GameObject leftDoor;

    public bool visited;

    private List<GameObject> doors = new List<GameObject>();

    void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelController>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        tilemap = GetComponentInChildren<Tilemap>();
        spawnableOrigin = transform.Find("SpawnableOrigin");
        upDoor = transform.Find("Door/UpDoor").gameObject;
        rightDoor = transform.Find("Door/RightDoor").gameObject;
        downDoor = transform.Find("Door/DownDoor").gameObject;
        leftDoor = transform.Find("Door/LeftDoor").gameObject;
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

    /// The position of the room BoxCollider2D is used, because the position of the room itself is not relevant.
    private void InitializeDoors()
    {
        if (roomData.doors.Contains(Vector2Int.up))
        {
            doors.Add(upDoor);
        }
        else
        {
            Destroy(upDoor);
        }

        if (roomData.doors.Contains(Vector2Int.right))
        {
            doors.Add(rightDoor);
        }
        else
        {
            Destroy(rightDoor);
        }

        if (roomData.doors.Contains(Vector2Int.down))
        {
            doors.Add(downDoor);
        }
        else
        {
            Destroy(downDoor);
        }

        if (roomData.doors.Contains(Vector2Int.left))
        {
            doors.Add(leftDoor);
        }
        else
        {
            Destroy(leftDoor);
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
