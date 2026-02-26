using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStateType
{
    Idle, Attack, Cast, Death
}

public class BossStateMachine
{
    private BaseState currentState;

    public BossIdleState IdleState { get; private set; }
    public BossAttackState AttackState { get; private set; }
    public BossCastState CastState { get; private set; }
    public BossDeathState DeathState { get; private set; }
    public BossStateType CurrentStateType { get; private set; }

    public event Action onCheckRecastState;     // 특수 공격 이후에 특수 공격과 일반 공격 중 어떤 공격으로 할지 예약하는 변수

    public BossStateMachine(Object objectController)
    {
        IdleState = new BossIdleState(objectController);
        AttackState = new BossAttackState(objectController);
        CastState = new BossCastState(objectController);
        DeathState = new BossDeathState(objectController);
    }

    public void ChangeState(BaseState nextState, BossStateType bossState)
    {
        if (nextState == currentState)
            return;

        if (currentState != null)
            currentState.OnStateExit();

        currentState = nextState;
        currentState.OnStateEnter();
        CurrentStateType = bossState;
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.OnStateUpdate();
    }

    public void OnCheckRecastState()
    {
        onCheckRecastState?.Invoke();
    }
}
