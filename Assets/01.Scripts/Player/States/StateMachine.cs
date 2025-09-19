using UnityEngine;

public class StateMachine
{
    public BaseState currentState { get; private set; }
    public PlayerManager player { get; }

    public StateMachine(PlayerManager player)
    {
        this.player = player;
    }

    public void Initialize(BaseState initialState, object payload = null)
    {
        currentState = initialState;
        initialState.OnEnter(payload);
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }

    public void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public void TransitionTo(BaseState nextState, object payload = null)
    {
        currentState?.OnExit();
        
        currentState = nextState;
        currentState.OnEnter(payload);
    }
}
