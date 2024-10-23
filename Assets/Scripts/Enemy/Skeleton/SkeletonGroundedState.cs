using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;

    protected Transform player;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        /*
         * 第二个条件是为了让玩家在 enemy 身后时，enemy 也能进入战斗状态。
         * 提问：为什么进入战斗状态后，enemy 会立刻面向玩家呢？明明我在它身后。还以为它会面向自己前方进入战斗状态。
         */
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
