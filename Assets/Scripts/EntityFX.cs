using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private float flashDuration;       // 受击闪烁动画持续时间。
    [SerializeField] private Material hitMat;           // 角色受击后替换的材质
    private Material originalMat;      // 角色原始材质

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        // 使用协程？做到类似于 Sleep 的效果。
        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }
}
