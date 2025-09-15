using UnityEngine;

public class BaseState
{
    protected readonly StateMachine stateMachine;
    protected readonly PlayerManager player;

    public BaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.player = stateMachine.player;
    }
    
    public virtual void OnEnter() {}
    public virtual void OnUpdate() {}
    public virtual void OnFixedUpdate() {}
    public virtual void OnExit() {}
}
