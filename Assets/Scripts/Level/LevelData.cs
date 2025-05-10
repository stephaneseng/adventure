using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int mapWidthHeight;
    [SerializeField] private Vector2Int startRoomPosition;
    [SerializeField] private Vector2Int endRoomPosition;

    public int MapWidthHeight => mapWidthHeight;

    public Vector2Int StartRoomPosition => startRoomPosition;

    public Vector2Int EndRoomPosition => endRoomPosition;

    public void Initialize(Level level)
    {
        mapWidthHeight = level.Rooms.GetLength(0);
        startRoomPosition = level.StartRoomPosition;
        endRoomPosition = level.EndRoomPosition;
    }
}
