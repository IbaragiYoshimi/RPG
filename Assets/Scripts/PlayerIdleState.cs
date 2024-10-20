using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.ZeroVelocity();      //不知为何不能用这个函数，会报实例未绑定的问题，但 PlayerState 基类中，player 是比 rb 更早初始化的呀。调用 rb 可以，player 不行？
        rb.velocity = new Vector2(0, 0);    // 为了解决滑墙时跳跃，落地后会继续向前滑行的 bug。
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        // 这里不知有什么用，教程还未讲到，先写上吧。
        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if(xInput != 0)     // 如果 x 轴的输入不为 0，则转换到 moveState。
            stateMachine.ChangeState(player.moveState);
    }
}
