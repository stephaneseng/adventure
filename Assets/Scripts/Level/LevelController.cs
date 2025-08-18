using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static readonly float RoomTransitionPlayerScrollDistance = 2.0f;
    private static readonly float RoomTransitionDurationInSeconds = 1.0f;

    [SerializeField] private LevelData levelData;

    private new Camera camera;
    private GameObject player;
    private GameObject miniMap;

    private GameObject[,] rooms;
    private Vector2Int currentRoomPosition;

    public LevelData LevelData => levelData;

    public GameObject[,] Rooms => rooms;

    public Vector2Int CurrentRoomPosition => currentRoomPosition;

    private void Awake()
    {
        camera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        miniMap = GameObject.FindGameObjectWithTag("MiniMap");
    }

    public void InitializeLevelData(Level level)
    {
        levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.Initialize(level);
    }

    public void InitializeAndEnterStartRoom()
    {
        InitializeRooms();
        EnterStartRoom();
    }

    private void InitializeRooms()
    {
        rooms = new GameObject[levelData.MapWidthHeight, levelData.MapWidthHeight];

        GetComponentsInChildren<Transform>(true).Where(transform => transform.CompareTag("Room")).ToList()
            .ForEach(transform =>
            {
                GameObject room = transform.gameObject;
                RoomController roomController = room.GetComponent<RoomController>();

                rooms[roomController.RoomData.Position.x, roomController.RoomData.Position.y] = room;

                roomController.Initialize();
                room.SetActive(false);
            });
    }

    private void EnterStartRoom()
    {
        EnterRoom(levelData.StartRoomPosition);

        GameObject startRoom = rooms[levelData.StartRoomPosition.x, levelData.StartRoomPosition.y];

        player.transform.position = new Vector3(startRoom.transform.position.x, startRoom.transform.position.y, player.transform.position.z);
        camera.transform.position = new Vector3(startRoom.transform.position.x, startRoom.transform.position.y, camera.transform.position.z);
    }

    private void EnterRoom(Vector2Int roomPosition)
    {
        currentRoomPosition = roomPosition;

        rooms[roomPosition.x, roomPosition.y].GetComponent<RoomController>().EnterRoom();

        UpdateMiniMap();
    }

    private void StartEnterRoomTransition(Vector2Int roomPosition)
    {
        rooms[roomPosition.x, roomPosition.y].GetComponent<RoomController>().StartEnterRoomTransition();
    }

    private void ExitRoom(Vector2Int roomPosition)
    {
        rooms[roomPosition.x, roomPosition.y].GetComponent<RoomController>().ExitRoom();
    }

    public void SwitchRoom(PlayerController playerController, Vector2Int transitionDirection)
    {
        StartCoroutine(PlayRoomTransitionAnimation(playerController, transitionDirection));
    }

    /// Translates the player and the camera in the specified direction.
    private IEnumerator PlayRoomTransitionAnimation(PlayerController playerController, Vector2Int transitionDirection)
    {
        GameObject targetRoom = rooms[currentRoomPosition.x + transitionDirection.x, currentRoomPosition.y + transitionDirection.y];
        Vector2Int targetRoomPosition = targetRoom.GetComponent<RoomController>().RoomData.Position;

        playerController.SwitchToFreezeState();

        StartEnterRoomTransition(targetRoomPosition);

        Vector3 playerStartPosition = playerController.transform.position;
        Vector3 cameraStartPosition = camera.transform.position;

        Vector3 playerTargetPosition = playerStartPosition + new Vector3(transitionDirection.x, transitionDirection.y, 0.0f) * RoomTransitionPlayerScrollDistance;
        Vector3 cameraTargetPosition = new Vector3(targetRoom.transform.position.x, targetRoom.transform.position.y, camera.transform.position.z);

        for (float t = 0.0f; t < RoomTransitionDurationInSeconds; t += Time.deltaTime)
        {
            playerController.transform.position = Vector3.Lerp(playerStartPosition, playerTargetPosition, t / RoomTransitionDurationInSeconds);
            camera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, t / RoomTransitionDurationInSeconds);

            yield return 0;
        }

        playerController.transform.position = playerTargetPosition;
        camera.transform.position = cameraTargetPosition;

        ExitRoom(currentRoomPosition);
        EnterRoom(targetRoomPosition);

        playerController.SwitchToIdleState();
    }

    public void UnlockDoor(Vector2Int doorDirection)
    {
        rooms[currentRoomPosition.x, currentRoomPosition.y].GetComponent<RoomController>().UnlockDoor(doorDirection);
        rooms[currentRoomPosition.x + doorDirection.x, currentRoomPosition.y + doorDirection.y].GetComponent<RoomController>().UnlockDoor(-doorDirection);
    }

    public void UpdateMiniMap()
    {
        miniMap.GetComponent<MiniMapController>().UpdateMiniMap();
    }

    public bool IsEndRoom(Vector2Int position)
    {
        return levelData.EndRoomPosition == position;
    }
}
