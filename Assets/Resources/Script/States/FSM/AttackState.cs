using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(Player playerController) : base(playerController) { }

    public override void OnStateEnter()
    {
        playerAnimator.SetBool("attack", true);
        playerAnimator.SetBool("idle", false);
        playerAnimator.SetBool("death", false);
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}
