using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;


    public List<InventoryItem> inventory;      // 用于存放
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;     // 用于快速查找

    public List<InventoryItem> stash;      // 用于存放
    public Dictionary<ItemData, InventoryItem> stashDictionary;     // 用于快速查找

    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;


    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        // 注意，获取多个相同组件，Components 是复数形式！
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for(int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        // 通过物品信息查找对应物品，而物品数量由每个物品的实例自己维护，同一种物品只需存在一个实例。数量是其内部的值。
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            // 如果没有，则通过物品信息实例化一种新的物品，然后存放之。
            InventoryItem newItem = new InventoryItem(_item);
            // 物品列表中存放实例。
            inventory.Add(newItem);
            // 字典中存放指向该物品的指针，通过键值对可以快速查找其所在位置。
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // 当该物品仅有一个时，调用本方法直接将 List 中的实例移除。同时移除字典存放的指针。
            if(value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            // 否则，仅调用该物品实例，移除一个数量。
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            // 当该物品仅有一个时，调用本方法直接将 List 中的实例移除。同时移除字典存放的指针。
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            // 否则，仅调用该物品实例，移除一个数量。
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

}
