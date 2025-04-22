using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialIcon;
    [SerializeField] private Button craftButton;
    
    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();
        
        if (_data.craftingMaterials.Count > materialIcon.Length)
        {
            Debug.Log("Too many materials for crafting window");
        }
        else if (_data.craftingMaterials.Count <= 0)
        {
            Debug.Log("This item cannot be crafted, it has no material formula.");
            return;
        }
        
        for (int i = 0; i < materialIcon.Length; i++)
        {
            materialIcon[i].color = Color.clear;
            materialIcon[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            materialIcon[i].sprite = _data.craftingMaterials[i].data.icon;
            materialIcon[i].color = Color.white;
            materialIcon[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftingMaterials[i].stackSize.ToString();
            materialIcon[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();
        
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));
    }
}
