using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    private PlayerInput playerInput;

    [Header("Locomotion Inputs")] 
    public Vector2 moveInput;
    public bool sprintInput;

    [Header("Move Action Inputs")]
    private bool jumpInput;
    //public bool dashInput;

    [Header("Attack Action Inputs")] 
    private bool lightAttackInput;
    private bool heavyAttackInput;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    private void OnEnable()
    {
        if (playerInput == null) playerInput = new PlayerInput();
        
        playerInput.Enable();

        playerInput.Locomotion.Move.performed += HandleMoveInput;
        playerInput.Locomotion.Sprint.performed += HandleSprintPerformedInput;
        playerInput.Locomotion.Sprint.canceled += HandleSprintCanceledInput;

        playerInput.Action.Jump.performed += ctx => jumpInput = true;
        
        playerInput.Attack.LightAttack.performed += ctx => lightAttackInput = true;
        playerInput.Attack.HeavyAttack.performed += ctx => heavyAttackInput = true;
    }

    private void OnDisable()
    {
        playerInput.Locomotion.Move.performed -= HandleMoveInput;
        playerInput.Locomotion.Sprint.performed -= HandleSprintPerformedInput;
        playerInput.Locomotion.Sprint.canceled -= HandleSprintCanceledInput;
        
        playerInput.Disable();
    }

    private void Update()
    {
        HandleLightAttackInput();
        HandleHeavyAttackInput();
    }

    private void HandleMoveInput(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

    private void HandleSprintPerformedInput(InputAction.CallbackContext context) => sprintInput = true;

    private void HandleSprintCanceledInput(InputAction.CallbackContext context) => sprintInput = false;

    public bool ConsumeJumpInput()
    {
        bool value = jumpInput;
        jumpInput = false;
        return value;
    }

    private void HandleLightAttackInput()
    {
        // [TO DO]
        // COUPLING WITH COMBOMANAGER
        // if (attackActionInputQueue.Count < queueCapacity && CanAttack())
    }

    private void HandleHeavyAttackInput()
    {
        // [TO DO]
        // COUPLING WITH COMBOMANAGER
        // if (attackActionInputQueue.Count < queueCapacity && CanAttack())
    }
}