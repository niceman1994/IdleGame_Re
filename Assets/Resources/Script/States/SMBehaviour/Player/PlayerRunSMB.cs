using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunSMB : StateMachineBehaviour
{
    [SerializeField] Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.GetComponent<Player>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
