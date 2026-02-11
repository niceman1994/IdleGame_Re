using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : BaseState
{
    public RunState(Player playerController) : base(playerController) { }

    public override void OnStateEnter()
    {
        playerAnimator.SetBool("attack", false);
        playerAnimator.SetBool("death", false);
        playerAnimator.SetBool("idle", false);
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}
