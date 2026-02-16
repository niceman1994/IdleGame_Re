using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지속성 버프 클래스
/// </summary>
public class DurationBuff : Buff
{
    private float tickTimer;
    private float tickInterval = 1.0f;

    public DurationBuff(IBuff buffEffect, float duration)
    {
        this.buffEffect = buffEffect;
        this.duration = duration;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        tickTimer += Time.deltaTime;

        // 1초에 한 번씩 버프를 부여함
        if (tickTimer >= tickInterval)
        {
            buffEffect.Apply(targetObject);
            tickTimer = 0.0f;
        }
    }

    public override void ExpireBuff()
    {
        base.ExpireBuff();
        tickTimer = 0.0f;
    }
}
