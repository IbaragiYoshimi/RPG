using System;

/* 注意 ItemObjec 类与 InventoryItem 类的不同。前者是挂载在 GameObject 上的，继承自 Monobehaviour，用于检测碰撞。
 * 后者则是库存对象，用于包装可看作基本类型的 ItemData，主要是维护库存数量等信息，仅在后台计算中使用，并不会出现在 Scene 中。故不需要继承 Monobehaviour。 */
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize = 0;

    // 实例化时，必定是有一个数量。
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
