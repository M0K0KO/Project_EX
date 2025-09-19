using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LandingState : BaseState
{
    private GroundedState superState;

    private IEnumerator waitCoroutine;

    private bool canTransition = false;
    
    public LandingState(StateMachine stateMachine, GroundedState superState) : base(stateMachine)
    {
        this.superState = superState;
    }
    
    public override void OnEnter(object payload = null)
    {
        Debug.Log("GroundedState/LandingState");

        player.animController.useRootMotion = false;

        bool canMove = player.animController.GetLandAnimation(out string animationToPlay);
        player.animController.PlayAnimation(animationToPlay, 0.05f);
        if (canMove)
        {
            stateMachine.TransitionTo(superState.sprintState);
        }
        else
        {
            player.motor.ResetPlayerVelocity(false);
            MotorSettings settings = new MotorSettings
            {
                moveSpeed = 0f,
                rotationSpeed = player.config.rotateSpeed,
                canMove = false,
                canRotate = true,
                canDash = true,
                canJump = false,
                useGravity = true,
            };
            player.motor.SetMotorSettings(settings);
        
            waitCoroutine = Utility.WaitThenFireAction(0.3f, () => canTransition = true);
            superState.playerStateMachine.StartCoroutine(waitCoroutine);
        }
    }

    public override void OnUpdate()
    {
        player.motor.ResetPlayerVelocity(true);

        if (canTransition)
        {
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
            else
            {
                stateMachine.TransitionTo(superState.idleState);
            }
        }

    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        if (waitCoroutine != null) superState.playerStateMachine.StopCoroutine(waitCoroutine);
        canTransition = false;
    }

}