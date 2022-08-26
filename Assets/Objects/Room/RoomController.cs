using System.Collections;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private static float EnemyRoomColliderSize = 4.0f;
    private static float TransitionPlayerScrollDistance = 0.5f;
    private static float TransitionCameraScrollDistance = 11.0f;
    private static float TransitionDurationInSeconds = 1.0f;

    public GameObject upRoom;
    public GameObject rightRoom;
    public GameObject downRoom;
    public GameObject leftRoom;

    private new Camera camera;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        camera = Camera.main;
        boxCollider2D = GetComponent<BoxCollider2D>();

        InstantiateEnemyRoomCollider();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            PlayerController playerController = other.GetComponent<PlayerController>();

            Vector2 contactVector = new Vector2(other.bounds.center.x - boxCollider2D.bounds.center.x,
                other.bounds.center.y - boxCollider2D.bounds.center.y);

            if (Vector2.Dot(contactVector, Vector2.up) > 1) {
                StartCoroutine(RoomTransition(playerController, Vector2.up, gameObject, upRoom));
            } else if (Vector2.Dot(contactVector, Vector2.right) > 1) {
                StartCoroutine(RoomTransition(playerController, Vector2.right, gameObject, rightRoom));
            } else if (Vector2.Dot(contactVector, Vector2.down) > 1) {
                StartCoroutine(RoomTransition(playerController, Vector2.down, gameObject, downRoom));
            } else {
                StartCoroutine(RoomTransition(playerController, Vector2.left, gameObject, leftRoom));
            }
        }
    }

    /// Creates a collider to avoid enemies leaving the room.
    /// This collider uses the position of the room BoxCollider2D, as the position of the room itself is less relevant.
    private void InstantiateEnemyRoomCollider()
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

    /// Translates the player (0.5 unit) and the camera (11 units) in the specified direction.
    private IEnumerator RoomTransition(PlayerController playerController, Vector2 transitionDirection, GameObject fromRoom,
        GameObject toRoom)
    {
        toRoom.SetActive(true);
        playerController.LockMove();

        Vector3 playerFromPosition = playerController.transform.position;
        Vector3 cameraFromPosition = camera.transform.position;

        Vector3 playerToPosition = playerFromPosition + (Vector3) transitionDirection * TransitionPlayerScrollDistance;
        Vector3 cameraToPosition = cameraFromPosition + (Vector3) transitionDirection * TransitionCameraScrollDistance;

        for (float t = 0.0f; t < TransitionDurationInSeconds ; t += Time.deltaTime) {
            playerController.transform.position = Vector3.Lerp(playerFromPosition, playerToPosition,
                t / TransitionDurationInSeconds);
            camera.transform.position = Vector3.Lerp(cameraFromPosition, cameraToPosition,
                t / TransitionDurationInSeconds);

            yield return 0;
        }

        playerController.transform.position = playerToPosition;
        camera.transform.position = cameraToPosition;

        fromRoom.SetActive(false);
        playerController.UnlockMove();
    }
}
