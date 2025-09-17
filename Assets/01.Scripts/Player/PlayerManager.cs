using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")] 
    public PlayerAnimatorController animController { get; private set; }
    public PlayerPhysics physics { get; private set; }
    public PlayerMotor motor { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody rb { get; private set; }

    [Header("External Refs")] 
    public Camera playerCam {get; private set;}

    [Header("Player Config")] 
    [SerializeField] private PlayerConfig configRef;
    public PlayerConfig config {get; private set;}
    

    private void Awake()
    {
        animController = GetComponent<PlayerAnimatorController>();
        physics = GetComponent<PlayerPhysics>();
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        playerCam = Camera.main;
        
        config = Instantiate(configRef);
    }
}
