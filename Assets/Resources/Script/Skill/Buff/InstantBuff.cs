using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 단발성 버프 클래스
/// </summary>
public class InstantBuff : Buff
{
    public InstantBuff(IBuff buffEffect, float duration)
    {
        this.buffEffect = buffEffect;
        this.duration = duration;
    }
}
