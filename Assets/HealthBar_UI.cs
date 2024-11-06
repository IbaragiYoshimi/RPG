using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;


    private void Start()
    {
        entity = GetComponentInParent<Entity>();

        entity.OnFlipped += FlipUI;     /* 委托（或者说函数指针？）在 OnFlipped 事件中增加 FlipUI，当 Entity 翻转触发 OnFlipped，一定会接着调用 FlipUI。 */

        myTransform = GetComponent<RectTransform>();
    }

    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
}
