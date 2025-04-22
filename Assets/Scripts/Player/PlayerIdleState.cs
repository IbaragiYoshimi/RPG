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

        //player.ZeroVelocity();      // Unity throw a expection (Object not set to an instance) with unkown problem.
        rb.linearVelocity = new Vector2(0, 0);    // Slide on the wall then fall to the ground, can not continue sliding.
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if(xInput != 0)
            stateMachine.ChangeState(player.moveState);
    }
}
