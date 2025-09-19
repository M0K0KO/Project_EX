using UnityEngine;

public class FallState : BaseState
{
    private AirborneState superState;
    
    public FallState(StateMachine stateMachine, AirborneState superState) : base(stateMachine)
    {
        this.superState = superState;
    }
    
    public override void OnEnter(object payload = null)
    {
        Debug.Log("AirborneState/FallState");

        player.animController.useRootMotion = false;
        
        if (!player.animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fall_01"))
            player.animController.PlayAnimation("Player_Fall_01");
        
        MotorSettings settings = new MotorSettings
        {
            canMove = true,
            canRotate = false,
            canDash = true,
            canJump = false, // [TODO] check if player can jump (doubleJump Feature)
            useGravity = true,
        };
        player.motor.SetMotorSettings(settings);
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        
    }
}
