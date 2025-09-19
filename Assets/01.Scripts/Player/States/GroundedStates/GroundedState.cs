using System;
using UnityEngine;

public class GroundedState : BaseState
{
    protected readonly StateMachine groundedSubStateMachine;

    public PlayerStateMachine playerStateMachine;

    public IdleState idleState;
    public RunState runState;
    public SprintState sprintState;
    public LandingState landingState;
    
    public GroundedState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) : base(stateMachine)
    {
        groundedSubStateMachine = new StateMachine(stateMachine.player);
        this.playerStateMachine = playerStateMachine;
        RegisterStates();
    }


    public override void OnEnter(object payload = null)
    {
        base.OnEnter();
        
        Debug.Log("Grounded");
        
        BaseState initState = idleState;
        
        if (payload is GroundedEntryType entryType)
        {
            switch (entryType)
            {
                case GroundedEntryType.Normal:
                    Debug.Log("Grounded Entry Type : Normal");
                    initState = idleState;
                    break;
                case GroundedEntryType.Landing:
                    Debug.Log("Grounded Entry Type : Land");
                    initState = landingState;
                    break;
            }
        }
        
        groundedSubStateMachine.Initialize(initState);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (PlayerInputManager.instance.ConsumeJumpInput())
        {
            stateMachine.TransitionTo(playerStateMachine.airborneState, AirborneEntryType.Jump);
            return;
        }
        
        
        if (!player.physics.isGrounded)
        {
            stateMachine.TransitionTo(playerStateMachine.airborneState, AirborneEntryType.Fall);
            return;
        }
        
        
        groundedSubStateMachine.Update();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        groundedSubStateMachine.FixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        groundedSubStateMachine.currentState?.OnExit();
    }

    private void RegisterStates()
    {
        idleState = new IdleState(groundedSubStateMachine, this);
        runState = new RunState(groundedSubStateMachine, this);
        sprintState = new SprintState(groundedSubStateMachine, this);
        landingState = new LandingState(groundedSubStateMachine, this);
    }
}

public enum GroundedEntryType
{
    Normal,
    Landing,
}
