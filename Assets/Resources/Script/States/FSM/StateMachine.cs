using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle, Run, Attack, Death
}

/// <summary>
/// <see cref="Player"/>의 상태를 바꿔줄 스크립트
/// </summary>
public class StateMachine
{
    private BaseState currentState;

    public IdleState IdleState { get; private set; }
    public RunState RunState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; }
    public PlayerStateType CurrentStateType { get; private set; }

    public StateMachine(Player player)
    {
        IdleState = new IdleState(player);
        RunState = new RunState(player);
        AttackState = new AttackState(player);
        DeathState = new DeathState(player);
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
