using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleSMB : StateMachineBehaviour
{
    [SerializeField] Object monster;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monster == null)
            monster = animator.GetComponent<Object>();
    }
}
