using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    private static string RoomResourcesFolder = "Room";

    private BlockFactory blockFactory;
    private EnemyFactory enemyFactory;

    void Awake()
    {
        blockFactory = GetComponentInChildren<BlockFactory>();
        enemyFactory = GetComponentInChildren<EnemyFactory>();
    }

    public void InstantiateRooms(List<RoomDefinition> roomDefinitions, GameObject level)
    {
        roomDefinitions.ForEach(roomDefinition =>
        {
            GameObject room = InstantiateRoom(roomDefinition, level);
            room.name = "Room(" + roomDefinition.position.x + "," + roomDefinition.position.y + ")";
        });
    }

    private GameObject InstantiateRoom(RoomDefinition roomDefinition, GameObject level)
    {
        GameObject room = Instantiate(Resources.Load<GameObject>(RoomResourcesFolder + "/Room"),
            new Vector3(roomDefinition.position.x * RoomController.RoomSize,
            roomDefinition.position.y * RoomController.RoomSize, 0.0f), Quaternion.identity, level.transform);

        RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
        roomData.position = roomDefinition.position;
        roomData.upExit = roomDefinition.upExit;
        roomData.rightExit = roomDefinition.rightExit;
        roomData.downExit = roomDefinition.downExit;
        roomData.leftExit = roomDefinition.leftExit;
        roomData.upDoor = roomDefinition.upDoor;
        roomData.rightDoor = roomDefinition.rightDoor;
        roomData.downDoor = roomDefinition.downDoor;
        roomData.leftDoor = roomDefinition.leftDoor;
        room.GetComponent<RoomController>().roomData = roomData;

        blockFactory.InstantiateBlocks(roomDefinition.blockDefinitions, room);

        enemyFactory.InstantiateEnemies(roomDefinition.enemyDefinitions, room);

        return room;
    }
}
