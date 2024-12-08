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

        /* 委托（或者说函数指针？）在 OnFlipped 事件中增加 FlipUI，当 Entity 翻转触发 OnFlipped，一定会接着调用 FlipUI。
         * 如果不这样做会怎么样？
         * Entity 和 HealthBar_UI 本质上是两个不同的脚本类，Entity 需要调用 HealthBar_UI 的话，他就必须要先获取挂载在子对象上的 HealthBar_UI，这样就会导致浪费性能。
         * 但其实目前这种实现方法，一样要获取对象，只不过是由 HealthBar_UI 来获取。然后把 FlipUI 的函数指针委托给 OnFlipped。让其在翻转的时候再进行一次翻转。
         * 说到底跟原本设想的，Entity 获取 HealthBar_UI，在 Entity 翻转时调用后者的 FlipUI 是没什么区别的。
         * 需要主义的是，Entity 算是场景中所有人物的基类，某些不具备血条的人物，在翻转时，OnFlipped 会试图寻找其子类的 FlipUI，找不到就会报错，经典的 Object reference not set to an instance。*/
        entity.OnFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxhealthValue();      // 最大生命值
        slider.value = myStats.currentHealth;          // 当前生命值
    }

    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        // 取消订阅。
        entity.OnFlipped -= FlipUI;     
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
