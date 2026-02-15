using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFlyingEye : Object
{
    [Header("Object를 상속받은 MonsterFlyingEye 스크립트")]
    [SerializeField] MonsterStatSO monsterData;

    protected override void Awake()
    {
        base.Awake();
        SetDefaultStats(monsterData.monsterStats.baseHp, monsterData.monsterStats.baseAttack, monsterData.monsterStats.baseAttackSpeed);
        giveGold = monsterData.giveGold;
    }

    private void Update()
    {
        CheckState();
    }

    public override void CheckState()
    {
        if (runtimeStats.hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flight"))
                EnemyDetect();
            else if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                AttackState();
        }
    }

    private void EnemyDetect()
    {
        if (detectCollider.IsDetectEnemy("Player"))
            objectAnimator.SetBool("attack", true);
    }

    private void AttackState()
    {
        // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
        /* normalizedTimeInProcess만 있으면 0.7f 이상부터는 계속 데미지를 계산해 한 번의 공격에 플레이어가 죽게 되고
        normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
        if (AttackStateProcess() > 0.7f && AttackStateTime() > attackLoop)
            PlayAttackSound(monsterData.monsterStats.attackClip);
    }

    protected override void Death()
    {
        base.Death();
        GameManager.Instance.gameGold.curGold[0] += giveGold;
        PlayDeadSound(monsterData.monsterStats.deadClip);
        healthSystem.NotifyDeath();
        ItemManager.Instance.SpawnItem(transform.position);
    }

    public override float CurrentAtk(float addAtk)
    {
        runtimeStats.attack += addAtk;
        return runtimeStats.attack;
    }

    public override void GetAttackDamage(float dmg)
    {
        TextPoolManager.Instance.ShowDamageText(dmg, textPos);
        runtimeStats.hp -= dmg;

        if (runtimeStats.hp <= 0)
            Death();
    }

    public override float CurrentHp()
    {
        return runtimeStats.hp;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 함수
    public override float HpUp(float addHp)
    {
        runtimeStats.hp += addHp;
        runtimeStats.maxHp += addHp;
        return runtimeStats.hp;
    }

    public override void CurrentHpChange(float value) { }
}
