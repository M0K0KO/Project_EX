using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Components")] 
    private Rigidbody rb;
    private PlayerManager player;
    
    [Header("Motor Settings")]
    private float moveSpeed;
    private float rotateSpeed;

    [Header("Motor Flags")]
    private bool canMove;
    private bool canJump;
    private bool canDash;
    private bool canRotate;
    private bool useGravity;

    public bool executeJump;
    
    private Vector3 finalVelocity;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        rb = player.rb;

        MotorSettings settings = new MotorSettings(player.config.runSpeed, player.config.rotateSpeed);
        SetMotorSettings(settings);
    }

    public void SetMotorSettings(MotorSettings settings)
    {
        moveSpeed = settings.moveSpeed;
        rotateSpeed = settings.rotationSpeed;
            
        canMove = settings.canMove;
        canJump = settings.canJump;
        canDash = settings.canDash;
        canRotate = settings.canRotate;
        useGravity = settings.useGravity;
    }

    private void FixedUpdate()
    {
        if (!player.physics.isGrounded)
        {
            Debug.Log("Airborne Input: " + player.physics.relativeInputDirection);
        }
        
        if (canRotate) RotatePlayer();
        
        if (canMove) finalVelocity = CalculateMovementVelocity();
        else finalVelocity = CalculateIdleVelocity();
        
        if (executeJump) ExecuteJump();

        rb.linearVelocity = finalVelocity;
    }

    private Vector3 CalculateMovementVelocity()
    {
        Vector3 calculatedVelocity;

        if (player.physics.isGrounded)
        {
            calculatedVelocity = player.physics.adjustedForwardDirection * moveSpeed;
        }
        else 
        {
            Vector3 horizontalMove = player.physics.relativeInputDirection * moveSpeed;
            calculatedVelocity = new Vector3(horizontalMove.x, rb.linearVelocity.y, horizontalMove.z);

            if (useGravity)
            {
                calculatedVelocity += Vector3.down * (player.physics.gravityForce * Time.fixedDeltaTime);
            }
        }

        return calculatedVelocity;
    }
    
    private Vector3 CalculateIdleVelocity()
    {
        Vector3 calculatedVelocity;

        if (player.physics.isGrounded)
        {
            calculatedVelocity = Vector3.zero;
        }
        else 
        {
            calculatedVelocity = new Vector3(0, rb.linearVelocity.y, 0);

            if (useGravity)
            {
                calculatedVelocity += Vector3.down * (player.physics.gravityForce * Time.fixedDeltaTime);
            }
        }
        return calculatedVelocity;
    }
    
    public void ResetPlayerVelocity(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            rb.linearVelocity = new (0f, rb.linearVelocity.y, 0f);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void ResetVerticalVelocity()
    {
        rb.linearVelocity = new (rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }

    private void RotatePlayer()
    {
        if (player.physics.adjustedForwardDirection == Vector3.zero) return;
        Vector3 horizontalDirection = new Vector3(player.physics.adjustedForwardDirection.x, 0, player.physics.adjustedForwardDirection.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        transform.rotation = smoothedRotation;
    }

    private void ExecuteJump()
    {
        executeJump = false;

        finalVelocity.y = player.config.jumpSpeed;
    }
}

public struct MotorSettings
{
    // speeds
    public float moveSpeed;
    public float rotationSpeed;

    // flags
    public bool canMove;
    public bool canJump;
    public bool canDash;
    public bool canRotate;
    public bool useGravity;

    public MotorSettings(
        float moveSpeed = 0.5f,
        float rotationSpeed = 1f,
        bool canMove = true,
        bool canRotate = true,
        bool canJump = true,
        bool canDash = true,
        bool useGravity = true)
    {
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
        
        this.canMove = canMove;
        this.canJump = canJump;
        this.canDash = canDash;
        this.canRotate = canRotate;
        this.useGravity = useGravity;
    }
}