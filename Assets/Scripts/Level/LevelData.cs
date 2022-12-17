using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public Vector2Int startRoomPosition;
    public Vector2Int endRoomPosition;
}
