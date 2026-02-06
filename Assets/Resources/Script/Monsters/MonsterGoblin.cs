using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGoblin : Object
{
    protected override void Start()
    {
        giveGold += Random.Range(14, 18);
        base.Start();
    }

    private void Update()
    {
        CheckState();
    }

    public override void CheckState()
    {
        if (hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                EnemyDetect();
            else if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                AttackState();
        }
        else if (IsObjectAnimComplete("Death"))
            StartCoroutine(OnMonsterDeathComplete());
    }

    private void EnemyDetect()
    {
        if (detectCollider.IsDetectEnemyCollider("Player"))
            objectAnimator.SetBool("attack", true);
    }

    private void AttackState()
    {
        // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
        /* normalizedTimeInProcess만 있으면 0.85f 이상부터는 계속 데미지를 계산해 한 번의 공격에 플레이어가 죽게 되고
        normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
        if (AttackStateProcess() > 0.85f && AttackStateTime() > atkLoop)
        {
            ClearAttackTarget();
            PlayAttackSound(attackSound, 3);
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
        TextPoolManager.Instance.ShowDamageText(dmg, textPos);
        hp -= dmg;

        if (hp <= 0)
            Death(() => PlayDeadSound(deadSound, 3));
    }

    public override float CurrentHp()
    {
        return hp;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 함수
    public override float HpUp(float addHp)
    {
        hp += addHp;
        defaultHp += addHp;
        return hp;
    }

    public override void CurrentHpChange(float value) { }
}
