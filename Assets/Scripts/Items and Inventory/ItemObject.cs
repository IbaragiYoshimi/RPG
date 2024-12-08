using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 注意 ItemObjec 类与 InventoryItem 类的不同。前者是挂载在 GameObject 上的，继承自 Monobehaviour，用于检测碰撞。
 * 后者则是库存对象，用于包装可看作基本类型的 ItemData，主要是维护库存数量等信息，仅在后台计算中使用，并不会出现在 Scene 中。故不需要继承 Monobehaviour。 */
public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    // 该函数是内置函数，在修改 gameObject 时会自动调用，用于验证正确性。
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
