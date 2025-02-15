using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize = 0;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
