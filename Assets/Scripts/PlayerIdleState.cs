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

        //player.ZeroVelocity();      //��֪Ϊ�β���������������ᱨʵ��δ�󶨵����⣬�� PlayerState �����У�player �Ǳ� rb �����ʼ����ѽ������ rb ���ԣ�player ���У�
        rb.velocity = new Vector2(0, 0);    // Ϊ�˽����ǽʱ��Ծ����غ�������ǰ���е� bug��
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        // ���ﲻ֪��ʲô�ã��̳̻�δ��������д�ϰɡ�
        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if(xInput != 0)     // ��� x ������벻Ϊ 0����ת���� moveState��
            stateMachine.ChangeState(player.moveState);
    }
}
