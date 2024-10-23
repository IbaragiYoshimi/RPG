using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;      // ֻ�������������ڼ������룬���������� + 1��

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;     // �����޸�������������⡣

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        #region Choose attack direction
        // ������ʱûѡ����WASD������Ĭ��ʹ�� facingDir������ʹ��ѡ���ķ���
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        #endregion

        /* ͨ�� Vector2 ���飬�ý�ɫ�ڽ��в�ͬ����ʱ��΢λ��һ�β�ͬ���룬�����������ع������ָУ��������λ��С���ص�λ�ƴ�����Ҳ������������ʱ˲�ƵĲ�����ȫ��������á�
         * ���� y ������ù���ʱһ���̶ȵ�С��һ�¡�
         */
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;       // ����ʱ������������ɫ����΢�е���Ե���ǰ��һ�£���ͣ����������
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            //player.ZeroVelocity ();   // ��֪Ϊ�β���������������ᱨʵ��δ�󶨵����⣬�� PlayerState �����У�player �Ǳ� rb �����ʼ����ѽ������ rb ���ԣ�player ���У�
            rb.velocity = new Vector2(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
}
