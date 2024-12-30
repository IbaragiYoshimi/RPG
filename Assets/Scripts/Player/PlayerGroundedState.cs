using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if(Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackholeState);

        // 注意：举剑瞄准的时候并没有创建剑的实体，仅播放动画，只有松开右键后，才创建剑的实体。所以可以将 HasNoSword 放在 Update 里检测。
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSword);

        if (Input.GetKey(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttack);

        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        // 当玩家在空中无操作时，不要进入 idleState，而是应该进入 airState，播放 Fall 动画。
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
