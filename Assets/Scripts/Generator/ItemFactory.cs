using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public void InstantiateItems(Spawnable[,] spawnables, GameObject room)
    {
        for (int x = 0; x < spawnables.GetLength(0); x++)
        for (int y = 0; y < spawnables.GetLength(1); y++)
        {
            if (spawnables[x, y] == null || spawnables[x, y] is not Item) continue;

            InstantiateItem((Item)spawnables[x, y], room);
        }
    }

    private void InstantiateItem(Item item, GameObject room)
    {
        Instantiate(Resources.Load<GameObject>(GameConstants.ResourcesItemFolder + "/" + item.ItemType),
            room.GetComponent<RoomController>().spawnableOrigin.position + new Vector3(item.Position.x, item.Position.y, 0.0f),
            Quaternion.identity, room.transform);
    }
}
