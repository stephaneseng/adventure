using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    private static string RoomResourcesFolder = "Room";
    private static string RoomResourceName = "Room";

    private BlockFactory blockFactory;
    private EnemyFactory enemyFactory;

    void Awake()
    {
        blockFactory = GetComponentInChildren<BlockFactory>();
        enemyFactory = GetComponentInChildren<EnemyFactory>();
    }

    public void InstantiateRooms(Room[,] rooms, GameObject levelGameObject)
    {
        for (int x = 0; x < rooms.GetLength(0); x++)
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }

                Room room = rooms[x, y];

                GameObject roomGameObject = InstantiateRoom(room, levelGameObject);
                roomGameObject.name = "Room(x" + room.position.x + ",y" + room.position.y + ",s" + room.section + ")";
            }
        }
    }

    private GameObject InstantiateRoom(Room room, GameObject levelGameObject)
    {
        GameObject roomGameObject = Instantiate(Resources.Load<GameObject>(RoomResourcesFolder + "/" + RoomResourceName),
            new Vector3(room.position.x * RoomController.RoomSize, room.position.y * RoomController.RoomSize, 0.0f),
            Quaternion.identity, levelGameObject.transform);

        RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
        roomData.Initialize(room);
        roomGameObject.GetComponent<RoomController>().roomData = roomData;

        blockFactory.InstantiateBlocks(room.spawnables, roomGameObject);
        enemyFactory.InstantiateEnemies(room.spawnables, roomGameObject);

        return roomGameObject;
    }
}
