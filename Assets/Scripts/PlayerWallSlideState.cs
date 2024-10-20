using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        // 若滑墙时按下与墙面相反方向的按钮“A”“D”，则退出滑墙状态，直接跳下墙。
        if(xInput != 0 && player.facingDir != xInput) 
            stateMachine.ChangeState(player.idleState);

        // 由玩家决定是否慢速滑墙。如果在墙上按“S”，就原速滑下；反之则慢速滑下。
        if(yInput < 0) 
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y * .7f); 

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
