using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static float RoomTransitionPlayerScrollDistance = 1.2f;
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
    }

    private void InitializeRooms()
    {
        rooms = new GameObject[levelData.mapWidthHeight, levelData.mapWidthHeight];

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

    public void EnterStartRoom()
    {
        EnterRoom(levelData.startRoomPosition);

        GameObject startRoom = rooms[levelData.startRoomPosition.x, levelData.startRoomPosition.y];
        RoomData roomData = startRoom.GetComponent<RoomController>().roomData;

        player.transform.position = new Vector3(startRoom.transform.position.x, startRoom.transform.position.y,
            player.transform.position.z);
        camera.transform.position = new Vector3(startRoom.transform.position.x, startRoom.transform.position.y,
            camera.transform.position.z);
    }

    private void EnterRoom(Vector2Int roomPosition)
    {
        currentRoomPosition = roomPosition;

        rooms[roomPosition.x, roomPosition.y].GetComponent<RoomController>().EnterRoom();

        miniMap.GetComponent<MiniMapController>().UpdateMiniMap();
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
        Vector2Int targetRoomPosition = targetRoom.GetComponent<RoomController>().roomData.position;

        playerController.Freeze();

        StartEnterRoomTransition(targetRoomPosition);

        Vector3 playerStartPosition = playerController.transform.position;
        Vector3 cameraStartPosition = camera.transform.position;

        Vector3 playerTargetPosition = playerStartPosition
            + new Vector3(transitionDirection.x, transitionDirection.y, 0.0f) * RoomTransitionPlayerScrollDistance;
        Vector3 cameraTargetPosition = new Vector3(targetRoom.transform.position.x, targetRoom.transform.position.y,
            camera.transform.position.z);

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

        playerController.StopFreeze();
    }

    public void UnlockDoor(Vector2Int doorDirection)
    {
        rooms[currentRoomPosition.x, currentRoomPosition.y].GetComponent<RoomController>().UnlockDoor(doorDirection);
        rooms[currentRoomPosition.x + doorDirection.x, currentRoomPosition.y + doorDirection.y]
            .GetComponent<RoomController>().UnlockDoor(-doorDirection);
    }
}
