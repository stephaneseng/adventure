using UnityEngine;

public class BlockFactory : MonoBehaviour
{
    private static string BlockResourcesFolder = "Room";
    private static string BlockResourceName = "Block";

    public void InstantiateBlocks(Spawnable[,] spawnables, GameObject room)
    {
        for (int x = 0; x < spawnables.GetLength(0); x++)
        {
            for (int y = 0; y < spawnables.GetLength(1); y++)
            {
                if (spawnables[x, y] == null || spawnables[x, y] is not Block)
                {
                    continue;
                }

                InstantiateBlock((Block)spawnables[x, y], room);
            }
        }
    }

    private void InstantiateBlock(Block block, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(BlockResourcesFolder + "/" + BlockResourceName),
            room.transform.position - new Vector3(RoomController.RoomHalfSize, RoomController.RoomHalfSize, 0.0f)
            + new Vector3(1.0f, 1.0f, 0.0f)
            + new Vector3(block.position.x, block.position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
