using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BaseState
{
    public BossIdleState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("idle", true);
        objectAnimator.SetBool("attack", false);
        objectAnimator.SetBool("cast", false);
        objectAnimator.SetBool("death", false);
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {

    }
}
