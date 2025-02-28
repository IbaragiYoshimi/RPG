using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _player.GetComponent<Rigidbody2D>();
    }

    public virtual void Enter()
    {
        //Debug.Log("I enter " + animBoolName);

        player.anim.SetBool(animBoolName, true);
        //rb = player.rb;
        triggerCalled = false;

    }

    public virtual void Update()
    {
        //Debug.Log("I'm in " + animBoolName);

        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.velocity.y);   // Blend tree gains y axis's velocity from rigidbody. Then play corresponding animation.
    }

    public virtual void Exit()
    {
        //Debug.Log("I exit " + animBoolName);

        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
