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
    [SerializeField] private float gravityForce = 10f;

    [Header("Motor Flags")]
    private bool canMove;
    private bool canJump;
    private bool canDash;
    private bool canRotate;
    private bool useGravity;

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
        if (canMove) MovePlayer(player.physics.adjustedForwardDirection);
        if (canRotate) RotatePlayer(player.physics.relativeInputDirection);
        if (useGravity) ApplyGravity();
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        rb.linearVelocity = moveDirection * moveSpeed;
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

    private void RotatePlayer(Vector3 rotateDirection)
    {
        if (rotateDirection == Vector3.zero) return;
        Vector3 horizontalDirection = new Vector3(rotateDirection.x, 0, rotateDirection.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        transform.rotation = smoothedRotation;
    }

    private void ApplyGravity()
    {
        if (!player.physics.isGrounded)
        {
            rb.linearVelocity += player.physics.adjustedDownwardDirection * gravityForce;
            Mathf.Clamp(rb.linearVelocity.y, -player.physics.maxNegativeVerticalVelocity, player.physics.maxPositiveVerticalVelocity);
        }
        else
        {
            rb.AddForce(player.physics.adjustedDownwardDirection * 2f, ForceMode.Acceleration);
        }
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
        float moveSpeed,
        float rotationSpeed,
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