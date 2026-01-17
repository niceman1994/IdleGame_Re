using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGoblin : Object
{
    private void Start()
    {
        giveGold += Random.Range(14, 18);
        objectAnimator = GetComponent<Animator>();
        objectAnimator.SetFloat("attackspeed", attackSpeed);
    }

    private void Update()
    {
        CheckState();
        ChangeDefault();
    }

    public override void CheckState()
    {
        if (hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                EnemyDetect();

            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                AttackState();
        }
        else Death();
    }

    private void EnemyDetect()
    {
        if (detectCollider.getEnemyCollider != null && detectCollider.getEnemyCollider.CompareTag("Player"))
            objectAnimator.SetBool("attack", true);
    }

    private void AttackState()
    {
        // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
        /* normalizedTimeInProcess만 있으면 0.85f 이상부터는 계속 데미지를 계산해 한 번의 공격에 플레이어가 죽게 되고
        normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
        if (AttackStateProcess() > 0.85f && AttackStateTime() > atkLoop)
        {
            PlayAttackSound(attackSound, 3);
            ClearAttackTarget();
        }
    }

    public override float CurrentAtk()
    {
        return atk;
    }

    public override float CurrentAtk(float addAtk)
    {
        atk += addAtk;
        return atk;
    }

    public override void GetAttackDamage(float dmg)
    {
        ObjectPoolManager.Instance.ShowDamageText(dmg, textPos);
        hp -= dmg;
    }

    // 아무 영향이 없는 현재 체력 함수
    public override float CurrentHp()
    {
        return hp;
    }

    // 데미지를 받은 수치만큼 텍스트를 보여주는 함수
    public override void CurrentHp(float value)
    {
        ObjectPoolManager.Instance.ShowDamageText(value, textPos);
        hp += value;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 함수
    public override float HpUp(float addHp)
    {
        hp += addHp;
        maxHp += addHp;
        return hp;
    }
}
