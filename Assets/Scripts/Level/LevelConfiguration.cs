using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "ScriptableObjects/LevelConfiguration")]
public class LevelConfiguration : ScriptableObject
{
    public List<RoomConfiguration> roomConfigurations = new List<RoomConfiguration>();

    public Vector2Int startRoomPosition;
}
