using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;


    public List<InventoryItem> inventory;      // ���ڴ��
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;     // ���ڿ��ٲ���

    public List<InventoryItem> stash;      // ���ڴ��
    public Dictionary<ItemData, InventoryItem> stashDictionary;     // ���ڿ��ٲ���

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

        // ע�⣬��ȡ�����ͬ�����Components �Ǹ�����ʽ��
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
        // ͨ����Ʒ��Ϣ���Ҷ�Ӧ��Ʒ������Ʒ������ÿ����Ʒ��ʵ���Լ�ά����ͬһ����Ʒֻ�����һ��ʵ�������������ڲ���ֵ��
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            // ���û�У���ͨ����Ʒ��Ϣʵ����һ���µ���Ʒ��Ȼ����֮��
            InventoryItem newItem = new InventoryItem(_item);
            // ��Ʒ�б��д��ʵ����
            inventory.Add(newItem);
            // �ֵ��д��ָ�����Ʒ��ָ�룬ͨ����ֵ�Կ��Կ��ٲ���������λ�á�
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // ������Ʒ����һ��ʱ�����ñ�����ֱ�ӽ� List �е�ʵ���Ƴ���ͬʱ�Ƴ��ֵ��ŵ�ָ�롣
            if(value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            // ���򣬽����ø���Ʒʵ�����Ƴ�һ��������
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            // ������Ʒ����һ��ʱ�����ñ�����ֱ�ӽ� List �е�ʵ���Ƴ���ͬʱ�Ƴ��ֵ��ŵ�ָ�롣
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            // ���򣬽����ø���Ʒʵ�����Ƴ�һ��������
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

}
