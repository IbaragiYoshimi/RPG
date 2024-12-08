using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ע�� ItemObjec ���� InventoryItem ��Ĳ�ͬ��ǰ���ǹ����� GameObject �ϵģ��̳��� Monobehaviour�����ڼ����ײ��
 * �������ǿ��������ڰ�װ�ɿ����������͵� ItemData����Ҫ��ά�������������Ϣ�����ں�̨������ʹ�ã������������ Scene �С��ʲ���Ҫ�̳� Monobehaviour�� */
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    // �ú��������ú��������޸� gameObject ʱ���Զ����ã�������֤��ȷ�ԡ�
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.name;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
