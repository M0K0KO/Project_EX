using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseState
{
    private GroundedState superState;
    
    public IdleState(StateMachine stateMachine, GroundedState superState) : base(stateMachine)
    {
        this.superState = superState;
    }
    
    public override void OnEnter()
    {
        Debug.Log("GroundedState/IdleState");

        player.animController.PlayAnimation("Player_Idle_01");
        
        player.motor.ResetPlayerVelocity(false);
        MotorSettings settings = new MotorSettings
        {
            moveSpeed = 0f,
            rotationSpeed = player.config.rotateSpeed,
            canMove = false,
            canRotate = true,
            canDash = true,
            canJump = true,
            useGravity = true,
        };
        player.motor.SetMotorSettings(settings);
    }

    public override void OnUpdate()
    {
        player.motor.ResetPlayerVelocity(true);
        
        if (PlayerInputManager.instance.moveInput != Vector2.zero)
        {
            if (PlayerInputManager.instance.sprintInput)
            {
                stateMachine.TransitionTo(superState.sprintState);
            }
            else
            {
                stateMachine.TransitionTo(superState.runState);
            }
        }
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        
    }
}
