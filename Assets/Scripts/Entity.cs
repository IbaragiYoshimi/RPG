using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision info")]
    public Transform attackCheck;           // 攻击范围框。
    public float attackCheckRadius;         // 攻击检查半径。
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;        // 碰撞检测时需要知道哪些层算“碰上了”。

    public int facingDir { get; private set; } = 1;         // 注意，facingDir 的初始值一定要设置，不然默认就是 0。会导致冲刺时速度变为 0。
    protected bool facingRight = true;

    public System.Action OnFlipped;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;     // 被击退后的硬直时间。
    protected bool isKnocked;


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void DamageEffect()
    {
        // 开始协程。
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }


    #region Velocity
    // 快速设置角色速度为(0, 0)
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }


    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        // 玩家被击中后，会有硬直时间。
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);     // 将反转图像的控制器放在这里，避免主 update 循环中杂乱无章。反正 SetVelocity 就是设置速度的，逻辑上说得通。
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);     // 射线起点，射线方向，射线长度，射线终点标志。
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;     // 将这两个标志位反转。
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);    // 将图像按 y 轴反转。

        OnFlipped();
    }

    /* 
     * 这个没搞懂，为什么需要 FlipController 来控制图像反转呢，岂不是多此一举。
     */
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    public virtual void Die()
    {
        
    }
}
