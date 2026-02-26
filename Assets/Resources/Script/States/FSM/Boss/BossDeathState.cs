using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BaseState
{
    public BossDeathState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("death", true);
        objectAnimator.SetBool("attack", false);
        objectAnimator.SetBool("cast", false);
    }

    public override void OnStateExit()
    {
        objectAnimator.SetBool("death", false);
    }

    public override void OnStateUpdate()
    {

    }
}
