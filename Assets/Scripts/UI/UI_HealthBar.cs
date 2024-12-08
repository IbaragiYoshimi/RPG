using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;


    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        /* ί�У�����˵����ָ�룿���� OnFlipped �¼������� FlipUI���� Entity ��ת���� OnFlipped��һ������ŵ��� FlipUI��
         * ���������������ô����
         * Entity �� HealthBar_UI ��������������ͬ�Ľű��࣬Entity ��Ҫ���� HealthBar_UI �Ļ������ͱ���Ҫ�Ȼ�ȡ�������Ӷ����ϵ� HealthBar_UI�������ͻᵼ���˷����ܡ�
         * ����ʵĿǰ����ʵ�ַ�����һ��Ҫ��ȡ����ֻ�������� HealthBar_UI ����ȡ��Ȼ��� FlipUI �ĺ���ָ��ί�и� OnFlipped�������ڷ�ת��ʱ���ٽ���һ�η�ת��
         * ˵���׸�ԭ������ģ�Entity ��ȡ HealthBar_UI���� Entity ��תʱ���ú��ߵ� FlipUI ��ûʲô����ġ�
         * ��Ҫ������ǣ�Entity ���ǳ�������������Ļ��࣬ĳЩ���߱�Ѫ��������ڷ�תʱ��OnFlipped ����ͼѰ��������� FlipUI���Ҳ����ͻᱨ������� Object reference not set to an instance��*/
        entity.OnFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxhealthValue();      // �������ֵ
        slider.value = myStats.currentHealth;          // ��ǰ����ֵ
    }

    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        // ȡ�����ġ�
        entity.OnFlipped -= FlipUI;     
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
