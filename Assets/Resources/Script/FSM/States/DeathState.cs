using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    public DeathState(Player playerController) : base(playerController) { }

    public override void OnStateEnter()
    {
        playerAnimator.SetBool("death", true);
        playerAnimator.SetBool("idle", false);
        playerAnimator.SetBool("attack", false);
    }

    public override void OnStateExit()
    {
        playerAnimator.SetBool("death", false);
    }

    public override void OnStateUpdate()
    {
        
    }
}
