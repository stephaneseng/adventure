using UnityEngine;

public class RoomFactory : MonoBehaviour
{

    private GameObject roomPrefab;

    void Awake()
    {
        roomPrefab = Resources.Load<GameObject>("Room");
    }

    public void Instantiate(RoomConfiguration roomConfiguration, Transform parent)
    {
        GameObject room = Instantiate(roomPrefab,
            new Vector3(roomConfiguration.position.x * RoomController.RoomSize,
            roomConfiguration.position.y * RoomController.RoomSize, 0.0f), Quaternion.identity, parent);
        room.name = "Room(" + roomConfiguration.position.x + "," + roomConfiguration.position.y + ")";
        room.GetComponent<RoomController>().roomConfiguration = roomConfiguration;
        room.SetActive(false);
    }
}
