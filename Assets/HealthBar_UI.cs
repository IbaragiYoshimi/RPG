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

        entity.OnFlipped += FlipUI;     /* ί�У�����˵����ָ�룿���� OnFlipped �¼������� FlipUI���� Entity ��ת���� OnFlipped��һ������ŵ��� FlipUI�� */

        myTransform = GetComponent<RectTransform>();
    }

    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
}
