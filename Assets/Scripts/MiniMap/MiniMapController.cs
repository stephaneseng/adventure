using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    private static string MiniMapResourcesFolder = "MiniMap";

    private GameObject miniMapRoomPrefab;
    private GameObject miniMapUnvisitedRoomPrefab;
    private GameObject miniMapRoomExitPrefab;
    private GameObject miniMapStartRoomMaskPrefab;
    private GameObject miniMapEndRoomMaskPrefab;
    private GameObject miniMapActiveRoomMaskPrefab;
    private GameObject level;

    private List<GameObject> miniMapRoomAndExits = new List<GameObject>();

    void Awake()
    {
        miniMapRoomPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapRoom");
        miniMapUnvisitedRoomPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapUnvisitedRoom");
        miniMapRoomExitPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapRoomExit");
        miniMapStartRoomMaskPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapStartRoomMask");
        miniMapEndRoomMaskPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapEndRoomMask");
        miniMapActiveRoomMaskPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapActiveRoomMask");
        level = GameObject.FindGameObjectWithTag("Level");
    }

    public void UpdateMiniMap()
    {
        ResetMiniMap();
        DrawMiniMap();
    }

    private void ResetMiniMap()
    {
        miniMapRoomAndExits.ForEach(miniMapRoom =>
        {
            GameObject.Destroy(miniMapRoom);
        });
    }

    private void DrawMiniMap()
    {
        Vector2Int startRoomPosition = level.GetComponent<LevelController>().levelData.startRoomPosition;
        Vector2Int endRoomPosition = level.GetComponent<LevelController>().levelData.endRoomPosition;
        Vector2Int activeRoomPosition = level.GetComponent<LevelController>().activeRoomPosition;

        GameObject[,] rooms = level.GetComponent<LevelController>().rooms;

        for (int x = 0; x < rooms.GetLength(0); x++)
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }

                RoomController roomController = rooms[x, y].GetComponent<RoomController>();
                RoomData roomData = roomController.roomData;

                Vector3 roomPosition = new Vector3(x, y, 0.0f);

                // Add the room to the mini-map, if visited or if it is the end room.
                if (roomController.visited || (x == endRoomPosition.x && y == endRoomPosition.y))
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomPrefab, roomPosition + transform.position,
                        Quaternion.identity, transform));
                }
                else
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapUnvisitedRoomPrefab, roomPosition + transform.position,
                        Quaternion.identity, transform));
                }

                // Add the exits to the mini-map, if visited.
                if (roomController.visited)
                {
                    if (roomData.upExit)
                    {
                        miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                            Quaternion.identity, transform));
                    }
                    if (roomData.rightExit)
                    {
                        miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                            Quaternion.Euler(0.0f, 0.0f, 270.0f), transform));
                    }
                    if (roomData.downExit)
                    {
                        miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                            Quaternion.Euler(0.0f, 0.0f, 180.0f), transform));
                    }
                    if (roomData.leftExit)
                    {
                        miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                            Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
                    }
                }
            }
        }

        // Add the start and end rooms indicator.
        miniMapRoomAndExits.Add(Instantiate(miniMapStartRoomMaskPrefab,
            new Vector3(startRoomPosition.x, startRoomPosition.y, 0.0f) + transform.position, Quaternion.identity,
            transform));
        miniMapRoomAndExits.Add(Instantiate(miniMapEndRoomMaskPrefab,
            new Vector3(endRoomPosition.x, endRoomPosition.y, 0.0f) + transform.position, Quaternion.identity,
            transform));

        // Add the current room indicator.
        miniMapRoomAndExits.Add(Instantiate(miniMapActiveRoomMaskPrefab,
            new Vector3(activeRoomPosition.x, activeRoomPosition.y, 0.0f) + transform.position, Quaternion.identity,
            transform));
    }
}
