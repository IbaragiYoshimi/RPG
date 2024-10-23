using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;      // 只有在连击窗口期继续输入，才能让连击 + 1。

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;     // 用于修复攻击方向的问题。

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        #region Choose attack direction
        // 若攻击时没选择方向（WASD），则默认使用 facingDir，否则使用选定的方向。
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        #endregion

        /* 通过 Vector2 数组，让角色在进行不同连击时稍微位移一段不同距离，可以提升轻重攻击的手感，例如轻的位移小，重的位移大。甚至也可以做到攻击时瞬移的操作，全看如何配置。
         * 加入 y 轴可以让攻击时一定程度地小跳一下。
         */
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;       // 奔跑时按攻击键，角色会稍微有点惯性地向前滑一下，再停下来攻击。
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
            //player.ZeroVelocity ();   // 不知为何不能用这个函数，会报实例未绑定的问题，但 PlayerState 基类中，player 是比 rb 更早初始化的呀。调用 rb 可以，player 不行？
            rb.velocity = new Vector2(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

    }
}
