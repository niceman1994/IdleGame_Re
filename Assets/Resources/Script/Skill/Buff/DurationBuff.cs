using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationBuff : Buff
{
    private float tickTimer;
    private float tickInterval = 1.0f;

    public DurationBuff(IBuffEffect buffEffect, float duration)
    {
        this.buffEffect = buffEffect;
        this.duration = duration;
    }

    public void Update()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            buffEffect.Apply(targetObject);
            tickTimer = 0.0f;
        }
    }
}
