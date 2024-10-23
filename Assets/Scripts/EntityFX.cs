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
}
