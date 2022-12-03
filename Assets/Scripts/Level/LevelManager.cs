using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static float RoomTransitionPlayerScrollDistance = 1.0f;
    private static float RoomTransitionCameraScrollDistance = 11.0f;
    private static float RoomTransitionDurationInSeconds = 1.0f;

    public GameObject startRoom;
    public Vector2 cameraStartPosition;
    public Vector2 playerStartPosition;

    private new Camera camera;
    private GameObject player;

    void Awake()
    {
        camera = Camera.main;
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        InitializeRooms();
        InitializeCamera();
        InitializePlayer();
    }

    private void InitializeRooms()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < rooms.Length; i++) {
            rooms[i].gameObject.SetActive(false);
        }

        startRoom.SetActive(true);
        startRoom.GetComponent<RoomController>().InitializeDoors();
    }

    private void InitializeCamera()
    {
        camera.transform.position = (Vector3) cameraStartPosition
            + new Vector3(0.0f, 0.0f, camera.transform.position.z);
    }

    private void InitializePlayer()
    {
        player.transform.position = (Vector3) playerStartPosition
            + new Vector3(0.0f, 0.0f, player.transform.position.z);
    }

    public void SwitchRoom(PlayerController playerController, Vector2 transitionDirection,
        GameObject fromRoom, GameObject toRoom)
    {
        Debug.Log("SwitchRoom: " + transitionDirection);
        StartCoroutine(PlayRoomTransitionAnimation(playerController, transitionDirection, fromRoom, toRoom));
    }

    /// Translates the player and the camera in the specified direction.
    private IEnumerator PlayRoomTransitionAnimation(PlayerController playerController, Vector2 transitionDirection,
        GameObject fromRoom, GameObject toRoom)
    {
        toRoom.SetActive(true);
        toRoom.GetComponent<BoxCollider2D>().enabled = false;
        playerController.LockMove();

        Vector3 playerStartPosition = playerController.transform.position;
        Vector3 cameraStartPosition = camera.transform.position;

        Vector3 playerTargetPosition = playerStartPosition
            + (Vector3) transitionDirection * RoomTransitionPlayerScrollDistance;
        Vector3 cameraTargetPosition = cameraStartPosition
            + (Vector3) transitionDirection * RoomTransitionCameraScrollDistance;

        Debug.Log("Player start: " + playerStartPosition + ", Player target: " + playerTargetPosition);
        Debug.Log("Camera start: " + cameraStartPosition + ", Camera target: " + cameraTargetPosition);

        for (float t = 0.0f; t < RoomTransitionDurationInSeconds ; t += Time.deltaTime) {
            playerController.transform.position = Vector3.Lerp(playerStartPosition, playerTargetPosition,
                t / RoomTransitionDurationInSeconds);
            camera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition,
                t / RoomTransitionDurationInSeconds);

            yield return 0;
        }

        playerController.transform.position = playerTargetPosition;
        camera.transform.position = cameraTargetPosition;

        toRoom.GetComponent<BoxCollider2D>().enabled = true;
        toRoom.GetComponent<RoomController>().InitializeDoors();

        fromRoom.SetActive(false);
        playerController.UnlockMove();
    }
}
