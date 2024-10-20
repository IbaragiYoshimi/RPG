using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] attackMovement;

    [Header("Move info")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] private float dashCoolDown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;        // 碰撞检测时需要知道哪些层算“碰上了”。

    public int facingDir { get; private set; } = 1;         // 注意，facingDir 的初始值一定要设置，不然默认就是 0。会导致冲刺时速度变为 0。
    private bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; } 
    public PlayerPrimaryAttackState primaryAttack { get; private set; }

    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();        // 注意顺序，先实例化 stateMachine。

        /*实例化 idleState 时，调用其构造函数，将 this 指针传入，它通过 this 指针在自己的脚本内构造出 player，
         * 并通过它自己的 player 来控制这个脚本中的 anim。
         * 因为 idleState 本身并未附着在任何场景对象中，所以需要与 Player 脚本关联，通过本脚本中的对象来 setBool。
         * */
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");      // 注意，airState 一样是 jump 状态，关联 Unity Animator 中的参数 Jump，所以给的参数也是 Jump。
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");    // 在墙上跳也是使用跳跃动画。

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        stateMachine.Initialize(idleState);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        CheckForDashInput();

    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;


            stateMachine.ChangeState(dashState);
        }
    }

    #region Velocity
    // 快速设置角色速度为(0, 0)
    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);
    

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);     // 将反转图像的控制器放在这里，避免主 update 循环中杂乱无章。反正 SetVelocity 就是设置速度的，逻辑上说得通。
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);     // 射线起点，射线方向，射线长度，射线终点标志。
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDir = facingDir * -1;     // 将这两个标志位反转。
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);    // 将图像按 y 轴反转。
    }

    /* 
     * 这个没搞懂，为什么需要 FlipController 来控制图像反转呢，岂不是多此一举。
     */
    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion
}
