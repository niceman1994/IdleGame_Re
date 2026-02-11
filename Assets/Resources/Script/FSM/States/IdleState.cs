using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(Player playerController) : base(playerController) { }

    public override void OnStateEnter()
    {
        playerAnimator.SetBool("idle", true);
        playerAnimator.SetBool("attack", false);
        playerAnimator.SetBool("death", false);
    }

    public override void OnStateExit()
    {
        playerAnimator.SetBool("idle", false);
    }

    public override void OnStateUpdate()
    {
        
    }
}
