using System.Collections;
using UnityEngine;

public class DoubleJumpState : BaseState
{
    private AirborneState superState;
    
    public DoubleJumpState(StateMachine stateMachine, AirborneState superState) : base(stateMachine)
    {
        this.superState = superState;
    }
    
    public override void OnEnter(object payload = null)
    {
        superState.jumpCount++;
        Debug.Log("AirborneState/DoubleJumpState");

        MotorSettings settings = new MotorSettings
        {
            moveSpeed = player.config.sprintSpeed,
            rotationSpeed = 0f,
            canMove = true,
            canRotate = false,
            canDash = true,
            canJump = false, // [TODO] check if player can jump (doubleJump Feature)
            useGravity = true,
        };
        player.motor.SetMotorSettings(settings);
        player.animController.useRootMotion = false;
        
        Vector3 jumpDirection;
        if (player.physics.adjustedForwardDirection != Vector3.zero)
        {
            jumpDirection = player.physics.adjustedForwardDirection;
        }
        else
        {
            // 입력이 없으면 현재 바라보는 방향으로 더블 점프
            jumpDirection = player.transform.forward;
        }
    
        // 2. 먼저 회전부터 수행 (canRotate 설정 전에)
        if (jumpDirection != Vector3.zero)
        {
            Vector3 horizontalDirection = new Vector3(jumpDirection.x, 0, jumpDirection.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);
            player.transform.rotation = targetRotation;  // 직접 회전 설정
        }
        
        player.motor.storedHorizontalVelocity = player.physics.adjustedForwardDirection * player.config.sprintSpeed;
        
        player.physics.StartJump();
        player.animController.PlayAnimation("Player_Double_Jump_01");
        player.motor.executeJump = true;
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        player.physics.checkGround = true;
    }

    private IEnumerator StopCheckingGroundForSeconds(float seconds)
    {
        player.physics.checkGround = false;
        yield return new WaitForSeconds(seconds);
        player.physics.checkGround = true;
    }
}