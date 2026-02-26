using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : BaseState
{
    public PlayerDeathState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("death", true);
        objectAnimator.SetBool("idle", false);
        objectAnimator.SetBool("attack", false);
    }

    public override void OnStateExit()
    {
        objectAnimator.SetBool("death", false);
    }

    public override void OnStateUpdate()
    {
        
    }
}
