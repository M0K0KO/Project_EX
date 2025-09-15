using UnityEngine;

public class StateMachine
{
    public BaseState currentState { get; private set; }
    public PlayerManager player { get; }

    public StateMachine(PlayerManager player)
    {
        this.player = player;
    }

    public void Initialize(BaseState initialState)
    {
        currentState = initialState;
        initialState.OnEnter();
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }

    public void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public void TransitionTo(BaseState nextState)
    {
        currentState?.OnExit();
        
        currentState = nextState;
        currentState.OnEnter();
    }
}
