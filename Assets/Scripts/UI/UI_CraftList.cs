using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField]
    private List<ItemData_Equipment>
        craftEquipment; // The item you can create even though you don't have enough materials.

    [SerializeField] private List<UI_CraftSlot> craftSlots;

    private void Start()
    {
        craftSlots = new List<UI_CraftSlot>();
    }

    private void OnEnable()
    {
        Inventory.instance.OnMaterialsChanged += SetupCraftList;
    }

    private void OnDisable()
    {
        Inventory.instance.OnMaterialsChanged -= SetupCraftList;
    }

    public void SetupCraftList()
    {
        if (craftSlots != null)
        {
            Debug.Log("Clearing craft slots");
            for (int i = craftSlots.Count - 1; i >= 0; i--)
            {
                Destroy(craftSlots[i].gameObject);
            }
            
            craftSlots.Clear();
        }

        for (int i = 0; i < craftEquipment.Count; i++) // Create a new craft slot for each item in the craft list.
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);

            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);

            if (craftSlots != null) craftSlots.Add(newSlot.GetComponent<UI_CraftSlot>());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}