using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    // 点击装备格子时，将装备卸下，返回库存中。
    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        CleanUpSlot();
    }
}
