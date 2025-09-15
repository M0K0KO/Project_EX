using System;
using UnityEngine;

public class GroundedState : BaseState
{
    protected readonly StateMachine subStateMachine;

    public PlayerStateMachine playerStateMachine;

    public IdleState idleState;
    public RunState runState;
    public SprintState sprintState;
    
    public GroundedState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) : base(stateMachine)
    {
        subStateMachine = new StateMachine(stateMachine.player);
        this.playerStateMachine = playerStateMachine;
        RegisterStates();
    }


    public override void OnEnter()
    {
        base.OnEnter();
        
        Debug.Log("Grounded");
        
        subStateMachine.Initialize(idleState);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        
        /*
        if (!player.physics.isGrounded)
        {
            stateMachine.TransitionTo(new AirborneState(stateMachine));
        }
        */
        
        
        subStateMachine.Update();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        subStateMachine.FixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        subStateMachine.currentState?.OnExit();
    }

    private void RegisterStates()
    {
        idleState = new IdleState(subStateMachine, this);
        runState = new RunState(subStateMachine, this);
        sprintState = new SprintState(subStateMachine, this);
    }
}
