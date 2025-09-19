using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class JumpState : BaseState
{
    private AirborneState superState;


    private Coroutine groundCheckCoroutine;
    
    public JumpState(StateMachine stateMachine, AirborneState superState) : base(stateMachine)
    {
        this.superState = superState;
    }
    
    public override void OnEnter(object payload = null)
    {
        Debug.Log("AirborneState/JumpState");
        
        player.animController.useRootMotion = false;

        groundCheckCoroutine = superState.playerStateMachine.StartCoroutine(StopCheckingGroundForSeconds(0.2f));
        player.physics.StartJump();
        
        player.animController.PlayAnimation(player.animController.GetJumpAnimation());

        player.motor.executeJump = true;

        Vector3 jumpVelocity = new Vector3(player.rb.linearVelocity.x, player.config.jumpSpeed, player.rb.linearVelocity.z);
        player.rb.linearVelocity = jumpVelocity;
        
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
        if (player.rb.linearVelocity.y < -5f)
        {
            stateMachine.TransitionTo(superState.fallState);
        }
    }

    public override void OnExit()
    {
        if (groundCheckCoroutine != null) superState.playerStateMachine.StopCoroutine(groundCheckCoroutine);

        player.physics.checkGround = true;
    }

    private IEnumerator StopCheckingGroundForSeconds(float seconds)
    {
        player.physics.checkGround = false;
        yield return new WaitForSeconds(seconds);
        player.physics.checkGround = true;
    }
}