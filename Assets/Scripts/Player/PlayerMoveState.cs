using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        player.SetVelocity(xInput * player.moveSpeed, player.rb.velocity.y);

        if(xInput == 0 || player.IsWallDetected())
            /*由于继承自 PlayerState，可以直接访问 stateMachine，
             * 但 idleState 和 moveState 本身并无联系，需通过 player 才能访问。
             */
            stateMachine.ChangeState(player.idleState);
    }
}
