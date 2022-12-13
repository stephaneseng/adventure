using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfiguration", menuName = "ScriptableObjects/RoomConfiguration")]
public class RoomConfiguration : ScriptableObject
{
    public enum RoomType
    {
        Room01_Start,
        Room02_End,
        Room03_Empty,
        Room0301_Plus
    }

    public RoomType type;

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

    public List<EnemyConfiguration> enemyConfigurations = new List<EnemyConfiguration>();
}
