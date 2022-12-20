using System.Collections.Generic;
using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    private static string BlockResourcesFolder = "Room";

    public void InstantiateBlocks(List<BlockDefinition> blockDefinitions, GameObject room)
    {
        blockDefinitions.ForEach(blockDefinition =>
        {
            InstantiateBlock(blockDefinition, room);
        });
    }

    private void InstantiateBlock(BlockDefinition blockDefinition, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(BlockResourcesFolder + "/Block"),
            room.transform.position - new Vector3(RoomController.RoomHalfSize, RoomController.RoomHalfSize, 0.0f)
            + new Vector3(blockDefinition.position.x, blockDefinition.position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
