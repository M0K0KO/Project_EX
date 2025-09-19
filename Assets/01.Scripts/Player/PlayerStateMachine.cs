using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerManager player;
    
    public StateMachine mainStateMachine { get; private set; }
    
    public GroundedState groundedState { get; private set; }
    public AirborneState airborneState { get; private set; }

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        mainStateMachine = new StateMachine(player);

        RegisterStates();
    }

    private void Start()
    {
        mainStateMachine.Initialize(groundedState);
    }

    private void Update()
    {
        mainStateMachine.Update();
    }

    private void FixedUpdate()  
    {
        mainStateMachine.FixedUpdate();

    }

    private void RegisterStates()
    {
        groundedState = new GroundedState(mainStateMachine, this);
        airborneState = new AirborneState(mainStateMachine, this);
    }
}