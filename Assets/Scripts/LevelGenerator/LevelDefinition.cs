using System.Collections.Generic;
using UnityEngine;

public class LevelDefinition
{
    public List<RoomDefinition> roomDefinitions = new List<RoomDefinition>();

    public Vector2Int startRoomPosition;
    public Vector2Int endRoomPosition;
}
