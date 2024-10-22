using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        // 使用Collier2D类型的数组，记录玩家 Attack 区域的所有碰撞。OverlapCircleAll 表示检测圆形区域内所有重叠事件。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            // 如果这些碰撞中有 enemy，则调用 enemy 身上的 Damage 函数，对其血量进行扣减。
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }
}
