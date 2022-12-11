using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    private static string MiniMapResourcesFolder = "MiniMap";

    private GameObject miniMapRoomPrefab;
    private GameObject miniMapActiveRoomPrefab;
    private GameObject miniMapRoomExitPrefab;
    private GameObject level;

    private List<GameObject> miniMapRoomAndExits = new List<GameObject>();

    void Awake()
    {
        miniMapRoomPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapRoom");
        miniMapActiveRoomPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapActiveRoom");
        miniMapRoomExitPrefab = Resources.Load<GameObject>(MiniMapResourcesFolder + "/MiniMapRoomExit");
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

                if (!roomController.visited)
                {
                    continue;
                }

                RoomConfiguration roomConfiguration = roomController.roomConfiguration;

                Vector3 roomPosition = new Vector3(roomConfiguration.position.x, roomConfiguration.position.y, 0.0f);
                Vector2Int activeRoomPosition = level.GetComponent<LevelController>().activeRoomPosition;

                if (roomPosition.x == activeRoomPosition.x && roomPosition.y == activeRoomPosition.y)
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapActiveRoomPrefab, roomPosition + transform.position,
                        Quaternion.identity, transform));
                }
                else
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomPrefab, roomPosition + transform.position,
                        Quaternion.identity, transform));
                }

                if (roomConfiguration.upExit)
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                        Quaternion.identity, transform));
                }
                if (roomConfiguration.rightExit)
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                        Quaternion.Euler(0.0f, 0.0f, 270.0f), transform));
                }
                if (roomConfiguration.downExit)
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                        Quaternion.Euler(0.0f, 0.0f, 180.0f), transform));
                }
                if (roomConfiguration.leftExit)
                {
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position,
                        Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
                }
            }
        }
    }
}