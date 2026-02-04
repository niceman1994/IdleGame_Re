using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스의 특수 공격 애니메이션 진입시 필요한 것들을 처리하기 위해 사용하는 클래스
/// </summary>
public class SpellAttackStateMachine : StateMachineBehaviour
{
    [SerializeField] Boss boss;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<Boss>();

        boss.AddCurrentSpellAttackCount();
    }
}
