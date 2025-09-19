using System;
using UnityEngine;

public class AirborneState : BaseState
{
    protected readonly StateMachine airborneSubStateMachine;

    public PlayerStateMachine playerStateMachine;

    public FallState fallState;
    public JumpState jumpState;
    public DoubleJumpState doubleJumpState;

    public int jumpCount = 0;

    public AirborneState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) : base(stateMachine)
    {
        airborneSubStateMachine = new StateMachine(stateMachine.player);
        this.playerStateMachine = playerStateMachine;
        RegisterStates();
    }


    public override void OnEnter(object payload = null)
    {
        base.OnEnter();
        
        Debug.Log("Airborne");

        BaseState initState = fallState;
        
        if (payload is AirborneEntryType entryType)
        {
            switch (entryType)
            {
                case AirborneEntryType.Jump:
                    Debug.Log("Airborne Entry Type : Jump");
                    initState = jumpState;
                    break;
                case AirborneEntryType.Fall:
                    Debug.Log("Airborne Entry Type : Fall");
                    initState = fallState;
                    break;
            }
        }
        
        airborneSubStateMachine.Initialize(initState);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        
        if (player.physics.isGrounded)
        {
            jumpCount = 0;
            stateMachine.TransitionTo(playerStateMachine.groundedState, GroundedEntryType.Landing);
            return;
        }
        
        
        airborneSubStateMachine.Update();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        airborneSubStateMachine.FixedUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        airborneSubStateMachine.currentState?.OnExit();
    }

    private void RegisterStates()
    {
        fallState = new FallState(airborneSubStateMachine, this);
        jumpState = new JumpState(airborneSubStateMachine, this);
        doubleJumpState = new DoubleJumpState(airborneSubStateMachine, this);
    }
}

public enum AirborneEntryType
{
    Jump,
    Fall
}