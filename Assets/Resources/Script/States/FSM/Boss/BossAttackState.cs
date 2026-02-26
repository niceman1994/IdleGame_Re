using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BaseState
{
    public BossAttackState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("idle", false);
        objectAnimator.SetBool("attack", true);
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}
