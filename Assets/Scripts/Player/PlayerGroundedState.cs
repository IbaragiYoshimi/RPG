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

        // ע�⣺�ٽ���׼��ʱ��û�д�������ʵ�壬�����Ŷ�����ֻ���ɿ��Ҽ��󣬲Ŵ�������ʵ�塣���Կ��Խ� HasNoSword ���� Update ���⡣
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSword);

        if (Input.GetKey(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttack);

        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        // ������ڿ����޲���ʱ����Ҫ���� idleState������Ӧ�ý��� airState������ Fall ������
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
