using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    private GameObject miniMapRoomPrefab;
    private GameObject miniMapUnvisitedRoomPrefab;
    private GameObject miniMapRoomExitPrefab;
    private GameObject miniMapStartRoomMaskPrefab;
    private GameObject miniMapEndRoomMaskPrefab;
    private GameObject miniMapCurrentRoomMaskPrefab;
    private GameObject level;
    private GameObject player;

    private readonly List<GameObject> miniMapRoomAndExits = new();

    private void Awake()
    {
        miniMapRoomPrefab = Resources.Load<GameObject>(GameConstants.ResourceUiFolder + "/" + GameConstants.ResourceUiMiniMapRoomName);
        miniMapUnvisitedRoomPrefab = Resources.Load<GameObject>(GameConstants.ResourceUiFolder + "/" + GameConstants.ResourceUiMiniMapUnvisitedRoomName);
        miniMapRoomExitPrefab = Resources.Load<GameObject>(GameConstants.ResourceUiFolder + "/" + GameConstants.ResourceUiMiniMapRoomExitName);
        miniMapStartRoomMaskPrefab = Resources.Load<GameObject>(GameConstants.ResourceUiFolder + "/" + GameConstants.ResourceUiMiniMapStartRoomMaskName);
        miniMapEndRoomMaskPrefab = Resources.Load<GameObject>(GameConstants.ResourceUiFolder + "/" + GameConstants.ResourceUiMiniMapEndRoomMaskName);
        miniMapCurrentRoomMaskPrefab = Resources.Load<GameObject>(GameConstants.ResourceUiFolder + "/" + GameConstants.ResourceUiMiniMapCurrentRoomMaskName);
        level = GameObject.FindGameObjectWithTag("Level");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UpdateMiniMap()
    {
        ResetMiniMap();
        DrawMiniMap();
    }

    private void ResetMiniMap()
    {
        miniMapRoomAndExits.ForEach(miniMapRoom => { Destroy(miniMapRoom); });
    }

    private void DrawMiniMap()
    {
        Vector2Int startRoomPosition = level.GetComponent<LevelController>().LevelData.StartRoomPosition;
        Vector2Int endRoomPosition = level.GetComponent<LevelController>().LevelData.EndRoomPosition;
        Vector2Int currentRoomPosition = level.GetComponent<LevelController>().CurrentRoomPosition;

        GameObject[,] rooms = level.GetComponent<LevelController>().Rooms;

        for (int x = 0; x < rooms.GetLength(0); x++)
        for (int y = 0; y < rooms.GetLength(1); y++)
        {
            if (rooms[x, y] == null) continue;

            RoomController roomController = rooms[x, y].GetComponent<RoomController>();
            RoomData roomData = roomController.RoomData;

            Vector3 roomPosition = new Vector3(x, y, 0.0f);

            // Add the room to the mini-map, if visited or if it is the end room.
            if (roomController.Visited || (x == endRoomPosition.x && y == endRoomPosition.y))
                miniMapRoomAndExits.Add(Instantiate(miniMapRoomPrefab, roomPosition + transform.position, Quaternion.identity, transform));
            // Else, only add the room to the mini-map if the player has the map.
            else if (player.GetComponent<PlayerController>().Map)
                miniMapRoomAndExits.Add(Instantiate(miniMapUnvisitedRoomPrefab, roomPosition + transform.position, Quaternion.identity, transform));

            // Add the exits to the mini-map, if visited.
            if (roomController.Visited)
            {
                if (roomData.Exits.Contains(Vector2Int.up))
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position, Quaternion.identity, transform));
                if (roomData.Exits.Contains(Vector2Int.right))
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position, Quaternion.Euler(0.0f, 0.0f, 270.0f), transform));
                if (roomData.Exits.Contains(Vector2Int.down))
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position, Quaternion.Euler(0.0f, 0.0f, 180.0f), transform));
                if (roomData.Exits.Contains(Vector2Int.left))
                    miniMapRoomAndExits.Add(Instantiate(miniMapRoomExitPrefab, roomPosition + transform.position, Quaternion.Euler(0.0f, 0.0f, 90.0f), transform));
            }
        }

        // Add the start and end rooms indicator.
        miniMapRoomAndExits.Add(Instantiate(miniMapStartRoomMaskPrefab, new Vector3(startRoomPosition.x, startRoomPosition.y, 0.0f) + transform.position,
            Quaternion.identity, transform));
        miniMapRoomAndExits.Add(Instantiate(miniMapEndRoomMaskPrefab, new Vector3(endRoomPosition.x, endRoomPosition.y, 0.0f) + transform.position,
            Quaternion.identity, transform));

        // Add the current room indicator.
        miniMapRoomAndExits.Add(Instantiate(miniMapCurrentRoomMaskPrefab, new Vector3(currentRoomPosition.x, currentRoomPosition.y, 0.0f) + transform.position,
            Quaternion.identity, transform));
    }
}
