using System;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private PlayerManager player;

    [Header("Ground Detection")]
    public bool checkGround = true;
    [SerializeField] private Transform groundCheckOrigin;
    [SerializeField] private LayerMask groundMask = -1;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Jump Settings")] 
    private bool isJumping = false;
    private float jumpStartTime;
    private float jumpIgnoreGroundTime = 0.3f;

    [Header("Slope Constraints")]
    [SerializeField] private float maxSlopeAngle = 60f;
    
    [Header("Physics Settings")]
    public float gravityForce = 20f;

    
    public RaycastHit groundHit;
    public bool isGrounded { get; private set; }
    public Vector3 groundNormal { get; private set; }
    public float groundAngle { get; private set; }
    public bool isOnSlope { get; private set; }
    public bool isOnClimbableSlope { get; private set; }
    public Vector3 relativeInputDirection { get; private set; }
    public Vector3 adjustedForwardDirection { get; private set; }
    public Vector3 adjustedDownwardDirection { get; private set; }

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        rb = player.rb;
        InitializeRigidbody();
    }

    private void InitializeRigidbody()
    {
        rb.freezeRotation = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void FixedUpdate()
    {
        if (checkGround)
        {
            GroundCheck();
            AdjustDirections();   
        }
    }

    private void GroundCheck()
    {
        if (isJumping && Time.time - jumpStartTime < jumpIgnoreGroundTime)
        {
            isGrounded = false;
            groundNormal = Vector3.up;
            groundAngle = 0f;
            return;
        }
        
        Ray ray = new Ray(groundCheckOrigin.position, Vector3.down);
        
        if (Physics.SphereCast(ray, groundCheckRadius, out groundHit, groundCheckDistance, groundMask))
        {
            groundNormal = groundHit.normal;
            groundAngle = Vector3.Angle(Vector3.up, groundNormal);

            isGrounded = groundAngle < maxSlopeAngle;
        }
        else // in air
        {
            groundNormal = Vector3.up;
            groundAngle = 0;        
            isGrounded = false;
        }

        isOnSlope = !Mathf.Approximately(groundAngle, 0f);
        isOnClimbableSlope = isOnSlope && isGrounded;
    }

    private void AdjustDirections()
    {
        Vector2 moveInput = PlayerInputManager.instance.moveInput;
        Vector3 moveDirection = moveInput.y * player.playerCam.transform.forward +
                                moveInput.x * player.playerCam.transform.right;
        relativeInputDirection = moveDirection.normalized;

        if (groundAngle < maxSlopeAngle)
        {
            adjustedForwardDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal).normalized;
            adjustedDownwardDirection = -groundNormal.normalized;
        }
        else
        {
            adjustedForwardDirection = relativeInputDirection;
            adjustedDownwardDirection = Vector3.down;
        }
    }
    
    public void StartJump()
    {
        isJumping = true;
        jumpStartTime = Time.time;
        isGrounded = false;
    }
    

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Ground detection visualization
            Gizmos.color = isGrounded ? Color.green : Color.red;
            if (isGrounded)
            {
                Gizmos.DrawWireSphere(groundHit.point, groundCheckRadius);
                Gizmos.DrawLine(groundCheckOrigin.position, groundHit.point);
            }
            else
            {
                Gizmos.DrawWireSphere(groundCheckOrigin.position + Vector3.down * groundCheckDistance,
                    groundCheckRadius);
                Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
            }

            // Direction visualization
            Vector3 gizmoCenter = transform.position + Vector3.up * 0.5f;
            
            // Adjusted Forward Direction (Blue)
            if (adjustedForwardDirection != Vector3.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(gizmoCenter, adjustedForwardDirection);
                Gizmos.DrawWireCube(gizmoCenter + adjustedForwardDirection, Vector3.one * 0.1f);
            }
            
            // Adjusted Downward Direction (Yellow)
            if (adjustedDownwardDirection != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(gizmoCenter, adjustedDownwardDirection);
                Gizmos.DrawWireCube(gizmoCenter + (adjustedDownwardDirection), Vector3.one * 0.1f);
            }
            
            // Ground Normal Direction (Cyan)
            if (isGrounded && groundNormal != Vector3.zero)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(gizmoCenter, groundNormal);
                Gizmos.DrawWireCube(gizmoCenter + groundNormal, Vector3.one * 0.1f);
            }
        }
    }
    #endif
}
