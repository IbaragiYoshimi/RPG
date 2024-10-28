using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        //player.AnimationTrigger();
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        // 使用Collier2D类型的数组，记录玩家 Attack 区域的所有碰撞。OverlapCircleAll 表示检测圆形区域内所有重叠事件。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            // 如果这些碰撞中有 enemy，则调用 enemy 身上的 Damage 函数，对其血量进行扣减。
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().DamageEffect();
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                // 主角的克隆体需要探测半径25个单位内与所有敌人的碰撞，然后找出最近的敌人。
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                //遍历所有敌人，每次循环保存距离最近的。
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if(closestEnemy != null)
        {
            // 判断在 x 轴上，克隆体与最近的敌人的位置关系，以此改变克隆体的朝向。
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
