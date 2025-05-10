using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    private BlockFactory blockFactory;
    private EnemyFactory enemyFactory;
    private ItemFactory itemFactory;

    private void Awake()
    {
        blockFactory = GetComponentInChildren<BlockFactory>();
        enemyFactory = GetComponentInChildren<EnemyFactory>();
        itemFactory = GetComponentInChildren<ItemFactory>();
    }

    public void InstantiateRooms(Room[,] rooms, GameObject levelGameObject)
    {
        for (int x = 0; x < rooms.GetLength(0); x++)
        for (int y = 0; y < rooms.GetLength(1); y++)
        {
            if (rooms[x, y] == null) continue;

            Room room = rooms[x, y];

            GameObject roomGameObject = InstantiateRoom(room, levelGameObject);
            roomGameObject.name = "Room(x" + room.Position.x + ",y" + room.Position.y + ",s" + room.Section + ")";
        }
    }

    private GameObject InstantiateRoom(Room room, GameObject levelGameObject)
    {
        GameObject roomGameObject = Instantiate(Resources.Load<GameObject>(GameConstants.ResourcesRoomFolder + "/" + GameConstants.ResourcesRoomName),
            new Vector3(room.Position.x * (room.Spawnables.GetLength(0) + 2), room.Position.y * (room.Spawnables.GetLength(1) + 2), 0.0f),
            Quaternion.identity, levelGameObject.transform);

        roomGameObject.GetComponent<RoomController>().InitializeRoomData(room);

        blockFactory.InstantiateBlocks(room.Spawnables, roomGameObject);
        enemyFactory.InstantiateEnemies(room.Spawnables, roomGameObject);
        itemFactory.InstantiateItems(room.Spawnables, roomGameObject);

        return roomGameObject;
    }
}
