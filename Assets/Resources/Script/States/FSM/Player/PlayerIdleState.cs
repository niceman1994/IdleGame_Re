using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public PlayerIdleState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("idle", true);
        objectAnimator.SetBool("attack", false);
        objectAnimator.SetBool("death", false);
    }

    public override void OnStateExit()
    {
        objectAnimator.SetBool("idle", false);
    }

    public override void OnStateUpdate()
    {
        
    }
}
