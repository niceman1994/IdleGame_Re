using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSMB : StateMachineBehaviour
{
    [SerializeField] Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.GetComponent<Player>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.IsObjectAnimComplete("Death"))
            player.OnPlayerDeathComplete();
    }
}
