using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static int MapWidth = 10;
    public static int MapHeight = 10;

    private static float RoomTransitionPlayerScrollDistance = 1.0f;
    private static float RoomTransitionDurationInSeconds = 1.0f;

    public LevelConfiguration levelConfiguration;

    private new Camera camera;
    private GameObject player;
    private Grid grid;

    private GameObject[,] rooms;
    private Vector2Int activeRoomPosition;

    void Awake()
    {
        camera = Camera.main;
        player = GameObject.FindWithTag("Player");
        grid = GetComponentInChildren<Grid>();
    }

    void Start()
    {
        InitializeRooms();
        InitializeCamera();
        InitializePlayer();
    }

    private void InitializeRooms()
    {
        rooms = new GameObject[LevelController.MapWidth, LevelController.MapHeight];

        GetComponentsInChildren<Transform>(true).Where(transform => transform.CompareTag("Room")).ToList()
            .ForEach(transform =>
        {
            GameObject room = transform.gameObject;
            RoomController roomController = room.GetComponent<RoomController>();

            room.SetActive(false);

            rooms[roomController.roomConfiguration.position.x, roomController.roomConfiguration.position.y] = room;
        });

        EnterRoom(levelConfiguration.startRoomPosition);
    }

    private void InitializeCamera()
    {
        camera.transform.position = new Vector3(levelConfiguration.startRoomPosition.x * RoomController.RoomSize,
            levelConfiguration.startRoomPosition.y * RoomController.RoomSize, camera.transform.position.z);
    }

    private void InitializePlayer()
    {
        player.transform.position =
            (Vector3)rooms[activeRoomPosition.x, activeRoomPosition.y].GetComponent<RoomController>().playerSpawnPosition
            + new Vector3(levelConfiguration.startRoomPosition.x * RoomController.RoomSize,
            levelConfiguration.startRoomPosition.y * RoomController.RoomSize, player.transform.position.z);
    }

    private void EnterRoom(Vector2Int roomPosition)
    {
        activeRoomPosition = roomPosition;

        rooms[activeRoomPosition.x, activeRoomPosition.y].GetComponent<RoomController>().EnterRoom();
    }

    public void SwitchRoom(PlayerController playerController, Vector2 transitionDirection)
    {
        StartCoroutine(PlayRoomTransitionAnimation(playerController, transitionDirection));
    }

    /// Translates the player and the camera in the specified direction.
    private IEnumerator PlayRoomTransitionAnimation(PlayerController playerController, Vector2 transitionDirection)
    {
        Vector2Int targetRoomPosition = activeRoomPosition + new Vector2Int(Mathf.FloorToInt(transitionDirection.x),
            Mathf.FloorToInt(transitionDirection.y));

        GameObject fromRoom = rooms[activeRoomPosition.x, activeRoomPosition.y];
        GameObject toRoom = rooms[targetRoomPosition.x, targetRoomPosition.y];

        playerController.LockMove();

        toRoom.SetActive(true);
        toRoom.GetComponent<RoomController>().StartEnterRoom();

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

        toRoom.GetComponent<RoomController>().EndEnterRoom();
        fromRoom.GetComponent<RoomController>().ExitRoom();

        activeRoomPosition = targetRoomPosition;

        playerController.UnlockMove();
    }
}
