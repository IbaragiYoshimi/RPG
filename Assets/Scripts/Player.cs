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
    [SerializeField] private LayerMask whatIsGround;        // ��ײ���ʱ��Ҫ֪����Щ���㡰�����ˡ���

    public int facingDir { get; private set; } = 1;         // ע�⣬facingDir �ĳ�ʼֵһ��Ҫ���ã���ȻĬ�Ͼ��� 0���ᵼ�³��ʱ�ٶȱ�Ϊ 0��
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
        stateMachine = new PlayerStateMachine();        // ע��˳����ʵ���� stateMachine��

        /*ʵ���� idleState ʱ�������乹�캯������ this ָ�봫�룬��ͨ�� this ָ�����Լ��Ľű��ڹ���� player��
         * ��ͨ�����Լ��� player ����������ű��е� anim��
         * ��Ϊ idleState ����δ�������κγ��������У�������Ҫ�� Player �ű�������ͨ�����ű��еĶ����� setBool��
         * */
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");      // ע�⣬airState һ���� jump ״̬������ Unity Animator �еĲ��� Jump�����Ը��Ĳ���Ҳ�� Jump��
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");    // ��ǽ����Ҳ��ʹ����Ծ������

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
    // �������ý�ɫ�ٶ�Ϊ(0, 0)
    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);
    

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);     // ����תͼ��Ŀ������������������ update ѭ�����������¡����� SetVelocity ���������ٶȵģ��߼���˵��ͨ��
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);     // ������㣬���߷������߳��ȣ������յ��־��
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
        facingDir = facingDir * -1;     // ����������־λ��ת��
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);    // ��ͼ�� y �ᷴת��
    }

    /* 
     * ���û�㶮��Ϊʲô��Ҫ FlipController ������ͼ��ת�أ����Ƕ��һ�١�
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
