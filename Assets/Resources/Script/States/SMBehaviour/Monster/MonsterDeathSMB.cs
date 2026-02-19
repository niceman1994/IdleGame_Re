using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathSMB : StateMachineBehaviour
{
    [SerializeField] Object monster;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monster == null)
            monster = animator.GetComponent<Object>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monster.IsObjectAnimComplete("Death"))
            monster.StartCoroutine(monster.OnMonsterDeathComplete());
    }
}
