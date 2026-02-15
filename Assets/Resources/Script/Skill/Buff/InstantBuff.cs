using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantBuff : Buff
{
    public InstantBuff(IBuffEffect buffEffect, float duration)
    {
        this.buffEffect = buffEffect;
        this.duration = duration;
    }
}
