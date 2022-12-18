using System.Collections.Generic;
using UnityEngine;

public class RoomDefinition
{
    public Vector2Int position;

    public bool upExit;
    public bool rightExit;
    public bool downExit;
    public bool leftExit;

    public bool upDoor;
    public bool rightDoor;
    public bool downDoor;
    public bool leftDoor;

    public List<BlockDefinition> blockDefinitions = new List<BlockDefinition>();

    public List<EnemyDefinition> enemyDefinitions = new List<EnemyDefinition>();
}
