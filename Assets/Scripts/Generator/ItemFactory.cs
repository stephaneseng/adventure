using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    private static string ItemResourcesFolder = "Item";

    public void InstantiateItems(Spawnable[,] spawnables, GameObject room)
    {
        for (int x = 0; x < spawnables.GetLength(0); x++)
        {
            for (int y = 0; y < spawnables.GetLength(1); y++)
            {
                if (spawnables[x, y] == null || spawnables[x, y] is not Item)
                {
                    continue;
                }

                InstantiateItem((Item)spawnables[x, y], room);
            }
        }
    }

    private void InstantiateItem(Item item, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(ItemResourcesFolder + "/" + item.itemType.ToString()),
            room.GetComponent<RoomController>().spawnableOrigin.position
            + new Vector3(item.position.x, item.position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
