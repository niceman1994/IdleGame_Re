using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle, Run, Attack, Death
}

public class PlayerStateMachine
{
    private BaseState currentState;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }
    public PlayerStateType CurrentStateType { get; private set; }

    public PlayerStateMachine(Object objectController)
    {
        IdleState = new PlayerIdleState(objectController);
        RunState = new PlayerRunState(objectController);
        AttackState = new PlayerAttackState(objectController);
        DeathState = new PlayerDeathState(objectController);
    }

    public void ChangeState(BaseState nextState, PlayerStateType playerState)
    {
        if (nextState == currentState)
            return;

        if (currentState != null)
            currentState.OnStateExit();

        currentState = nextState;
        currentState.OnStateEnter();
        CurrentStateType = playerState;
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.OnStateUpdate();
    }

    public void GetStateTypeName()
    {
        Debug.Log($"현재 상태 : {CurrentStateType}");
    }
}
