using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LandingState : BaseState
{
    private GroundedState superState;

    private IEnumerator waitCoroutine;
    
    public LandingState(StateMachine stateMachine, GroundedState superState) : base(stateMachine)
    {
        this.superState = superState;
    }
    
    public override void OnEnter(object payload = null)
    {
        Debug.Log("GroundedState/LandingState");

        player.animController.useRootMotion = false;
        
        player.animController.PlayAnimation("Player_Jump_End_01");
        
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
        
        waitCoroutine = Utility.WaitThenFireAction(0.3f, () => stateMachine.TransitionTo(superState.idleState));
        superState.playerStateMachine.StartCoroutine(waitCoroutine);
    }

    public override void OnUpdate()
    {
        player.motor.ResetPlayerVelocity(true);

    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        superState.playerStateMachine.StopCoroutine(waitCoroutine);
    }

}