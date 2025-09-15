using UnityEngine;

public class SprintState : BaseState
{
    private GroundedState superState;
    
    public SprintState(StateMachine stateMachine, GroundedState superState) : base(stateMachine)
    {
        this.superState = superState;
    }

    public override void OnEnter()
    {
        Debug.Log("GroundedState/SprintState");
        
        player.animController.PlayAnimation("Player_Sprint_Loop_F_01");
        
        MotorSettings settings = new MotorSettings
        {
            moveSpeed = player.config.sprintSpeed,
            rotationSpeed = player.config.rotateSpeed,
            canMove = true,
            canRotate = true,
            canDash = true,
            canJump = true,
            useGravity = true,
        };
        player.motor.SetMotorSettings(settings);
    }

    public override void OnUpdate()
    {
        if (PlayerInputManager.instance.moveInput == Vector2.zero)
        {
            stateMachine.TransitionTo(superState.idleState);
        }
        else if (!PlayerInputManager.instance.sprintInput)
        {
            stateMachine.TransitionTo(superState.runState);
        }
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        
    }
}
