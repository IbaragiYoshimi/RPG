using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private float flashDuration;       // �ܻ���˸��������ʱ�䡣
    [SerializeField] private Material hitMat;           // ��ɫ�ܻ����滻�Ĳ���
    private Material originalMat;      // ��ɫԭʼ����

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        // ʹ��Э�̣����������� Sleep ��Ч����
        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();

        sr.color = Color.white;
    }
}
