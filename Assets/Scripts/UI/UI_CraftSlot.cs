using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if(_data == null) return;
        
        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 20)
        {
            itemText.fontSize *= 0.5f;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
        
        ui.itemTooltip.HideTooltip();
    }
}
