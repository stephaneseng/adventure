using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData")]
public class RoomData : ScriptableObject
{
    public Vector2Int position;

    [Header("Exits")]
    public bool upExit;
    public bool rightExit;
    public bool downExit;
    public bool leftExit;

    [Header("Doors")]
    public bool upDoor;
    public bool rightDoor;
    public bool downDoor;
    public bool leftDoor;
}
