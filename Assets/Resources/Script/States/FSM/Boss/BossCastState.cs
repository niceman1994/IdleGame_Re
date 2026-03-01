using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCastState : BaseState
{
    public event Action onCheckRecastState;

    public BossCastState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("attack", false);
        objectAnimator.SetBool("cast", true);
    }

    public override void OnStateExit()
    {
        objectAnimator.SetBool("attack", true);
        objectAnimator.SetBool("cast",  false);
    }

    public override void OnStateUpdate()
    {
        
    }

    public void CheckRecast()
    {
        onCheckRecastState?.Invoke();
    }
}
