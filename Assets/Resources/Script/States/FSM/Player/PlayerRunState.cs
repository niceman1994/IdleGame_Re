using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : BaseState
{
    public PlayerRunState(Object objectController) : base(objectController) { }

    public override void OnStateEnter()
    {
        objectAnimator.SetBool("attack", false);
        objectAnimator.SetBool("death", false);
        objectAnimator.SetBool("idle", false);
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}
