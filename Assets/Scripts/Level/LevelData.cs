using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int mapWidthHeight;

    public Vector2Int startRoomPosition;

    public Vector2Int endRoomPosition;

    public void Initialize(Level level)
    {
        mapWidthHeight = level.rooms.GetLength(0);
        startRoomPosition = level.startRoomPosition;
        endRoomPosition = level.endRoomPosition;
    }
}
