using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ע�� ItemObjec ���� InventoryItem ��Ĳ�ͬ��ǰ���ǹ����� GameObject �ϵģ��̳��� Monobehaviour�����ڼ����ײ��
 * �������ǿ��������ڰ�װ�ɿ����������͵� ItemData����Ҫ��ά�������������Ϣ�����ں�̨������ʹ�ã������������ Scene �С��ʲ���Ҫ�̳� Monobehaviour�� */
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;


    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
