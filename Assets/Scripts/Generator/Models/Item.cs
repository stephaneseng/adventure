using UnityEngine;

public class Item : Spawnable
{
    private readonly ItemType itemType;

    public Item(Vector2Int position, ItemType itemType) : base(position)
    {
        this.itemType = itemType;
    }

    public ItemType ItemType => itemType;
}
