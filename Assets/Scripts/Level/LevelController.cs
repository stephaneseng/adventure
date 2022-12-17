using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static int MapWidth = 10;
    public static int MapHeight = 10;

    private static float RoomTransitionPlayerScrollDistance = 1.0f;
    private static float RoomTransitionDurationInSeconds = 1.0f;

    public LevelData levelData;

    private new Camera camera;
    private GameObject player;
    private GameObject miniMap;

    public GameObject[,] rooms;
    public Vector2Int currentRoomPosition;

    void Awake()
    {
        camera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        miniMap = GameObject.FindGameObjectWithTag("MiniMap");
    }

    public void Initialize()
    {
        InitializeRooms();
        InitializeCamera();
        InitializePlayer();

        EnterRoom(levelData.startRoomPosition);
    }

    private void InitializeRooms()
    {
        rooms = new GameObject[LevelController.MapWidth, LevelController.MapHeight];

        GetComponentsInChildren<Transform>(true).Where(transform => transform.CompareTag("Room")).ToList()
            .ForEach(transform =>
        {
            GameObject room = transform.gameObject;
            RoomController roomController = room.GetComponent<RoomController>();

            rooms[roomController.roomData.position.x, roomController.roomData.position.y] = room;

            roomController.Initialize();
            room.SetActive(false);
        });
    }

    private void InitializeCamera()
    {
        camera.transform.position = new Vector3(levelData.startRoomPosition.x * RoomController.RoomSize,
            levelData.startRoomPosition.y * RoomController.RoomSize, camera.transform.position.z);
    }

    private void InitializePlayer()
    {
        player.transform.position = new Vector3(levelData.startRoomPosition.x * RoomController.RoomSize,
            levelData.startRoomPosition.y * RoomController.RoomSize, player.transform.position.z);
    }

    private void EnterRoom(Vector2Int roomPosition)
    {
        currentRoomPosition = roomPosition;

        rooms[roomPosition.x, roomPosition.y].GetComponent<RoomController>().EnterRoom();

        miniMap.GetComponent<MiniMapController>().UpdateMiniMap();
    }

    private void ExitRoom(Vector2Int roomPosition)
    {
        rooms[roomPosition.x, roomPosition.y].GetComponent<RoomController>().ExitRoom();
    }

    public void SwitchRoom(PlayerController playerController, Vector2 transitionDirection)
    {
        StartCoroutine(PlayRoomTransitionAnimation(playerController, transitionDirection));
    }

    /// Translates the player and the camera in the specified direction.
    private IEnumerator PlayRoomTransitionAnimation(PlayerController playerController, Vector2 transitionDirection)
    {
        Vector2Int targetRoomPosition = currentRoomPosition + new Vector2Int(Mathf.FloorToInt(transitionDirection.x),
            Mathf.FloorToInt(transitionDirection.y));

        GameObject fromRoom = rooms[currentRoomPosition.x, currentRoomPosition.y];
        GameObject toRoom = rooms[targetRoomPosition.x, targetRoomPosition.y];

        playerController.LockMove();

        toRoom.GetComponent<RoomController>().StartEnterRoomTransition();

        Vector3 playerStartPosition = playerController.transform.position;
        Vector3 cameraStartPosition = camera.transform.position;

        Vector3 playerTargetPosition = playerStartPosition
            + (Vector3)transitionDirection * RoomTransitionPlayerScrollDistance;
        Vector3 cameraTargetPosition = cameraStartPosition
            + (Vector3)transitionDirection * RoomController.RoomSize;

        for (float t = 0.0f; t < RoomTransitionDurationInSeconds; t += Time.deltaTime)
        {
            playerController.transform.position = Vector3.Lerp(playerStartPosition, playerTargetPosition,
                t / RoomTransitionDurationInSeconds);
            camera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition,
                t / RoomTransitionDurationInSeconds);

            yield return 0;
        }

        playerController.transform.position = playerTargetPosition;
        camera.transform.position = cameraTargetPosition;

        ExitRoom(currentRoomPosition);
        EnterRoom(targetRoomPosition);

        playerController.UnlockMove();
    }
}
