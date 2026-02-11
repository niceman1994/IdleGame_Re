using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태를 바꿔줄 스크립트<para/>
/// <see cref="Player"/> 에 상태 변수에 대한 코드를 줄이기 위한 목적도 있음(시험용)
/// </summary>
public class StateMachine
{
    private BaseState currentState;

    public IdleState IdleState { get; private set; }
    public RunState RunState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeathState DeathState { get; private set; } 

    public StateMachine(Player player)
    {
        IdleState = new IdleState(player);
        RunState = new RunState(player);
        AttackState = new AttackState(player);
        DeathState = new DeathState(player);
    }

    public void ChangeState(BaseState nextState)
    {
        if (nextState == currentState)
        {
            Debug.LogWarning($"이미 {currentState.GetType().Name} 입니다.");
            return;
        }

        if (currentState != null)
            currentState.OnStateExit();

        currentState = nextState;
        currentState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.OnStateUpdate();
    }

    public void GetStateTypeName()
    {
        Debug.Log($"현재 상태 : {currentState.GetType().Name}");
    }
}
