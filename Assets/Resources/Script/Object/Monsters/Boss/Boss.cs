using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Object
{
    [Header("Object를 상속받은 Boss 스크립트")]
    [SerializeField] MonsterStatSO bossData;
    [SerializeField] BossSpell spell;
    [SerializeField] AudioSource castSound;

    private int lastSpellAttackCount;
    private int currentSpellAttackCount;
    private float spellAttackValue;
    private BossStateMachine bossStateMachine;          // Boss의 상태 머신 변수

    private Queue<BossSpell> bossSpellQueue = new Queue<BossSpell>();
    // 플레이어가 죽어서 스테이지가 초기화됐을 때 BossSpell이 회수되지 않을 수 있는걸 방지하기 위한 변수
    private List<BossSpell> activeBossSpells = new List<BossSpell>();

    private void OnDisable()
    {
        for (int i = 0; i < activeBossSpells.Count; i++)
            bossSpellQueue.Enqueue(activeBossSpells[i]);

        activeBossSpells.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        InitBoss();
    }

    private void Update()
    {
        CheckState();
    }

    private void InitBoss()
    {
        SetDefaultStats(bossData.objectStats.baseHp, bossData.objectStats.baseAttack, bossData.objectStats.baseAttackSpeed);
        PrepareSpell();

        bossStateMachine = new BossStateMachine(this);
        ChangeState(BossStateType.Idle);
        bossStateMachine.onCheckRecastState += OnCheckRecastState;

        detectCollider.onEnemyDetected += ObjectStateChange;
        spellAttackValue = 0.75f;
        lastSpellAttackCount = 0;
        currentSpellAttackCount = 0;
        giveGold = bossData.giveGold;
    }

    private void PrepareSpell()
    {
        for (int i = 0; i < 5; i++)
        {
            BossSpell spellObject = Instantiate(spell, transform);
            spellObject.gameObject.SetActive(false);
            spellObject.transform.localScale = new Vector3(spellObject.transform.localScale.x / transform.localScale.x,
            spellObject.transform.localScale.y / transform.localScale.y, 1.0f);
            spellObject.name = $"{spell.name}_{i}";
            bossSpellQueue.Enqueue(spellObject);
        }
    }

    public override void CheckState()
    {
        ChangeState(bossStateMachine.CurrentStateType);

        if (runtimeStats.hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (objectAnimator.GetFloat("spellAttack") > spellAttackValue)
                    ChangeState(BossStateType.Cast);
                else
                    AttackState();
            }
        }
    }

    private void ChangeState(BossStateType bossState)
    {
        switch (bossState)
        {
            case BossStateType.Idle:
                bossStateMachine.ChangeState(bossStateMachine.IdleState, bossState);
                break;
            case BossStateType.Attack:
                bossStateMachine.ChangeState(bossStateMachine.AttackState, bossState);
                break;
            case BossStateType.Cast:
                bossStateMachine.ChangeState(bossStateMachine.CastState, bossState);
                CastState();
                break;
            case BossStateType.Death:
                bossStateMachine.ChangeState(bossStateMachine.DeathState, bossState);
                break;
        }
    }

    protected override void ObjectStateChange(Object detectedEnemy)
    {
        if (detectedEnemy != null)
        {
            RegisterEnemyDeathCallback();
            ChangeState(BossStateType.Attack);
        }
    }

    private void AttackState()
    {
        if (AttackStateProcess() > 0.625f && AttackStateTime() > attackLoop)
        {
            PlayAttackSound(bossData.objectStats.attackClip);
            objectAnimator.SetFloat("spellAttack", Random.Range(0.3f, 1.0f));
        }
    }

    private void CastState()
    {
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Cast") &&
            objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (lastSpellAttackCount != currentSpellAttackCount)
            {
                lastSpellAttackCount = currentSpellAttackCount;
                SummonSpell();
            }
        }
    }

    public void AddCurrentSpellAttackCount()
    {
        currentSpellAttackCount++;
    }

    private void SummonSpell()
    {
        castSound.clip = SoundManager.Instance.castSounds[0].audioClip;
        castSound.Play();

        var bossSpell = bossSpellQueue.Dequeue();
        activeBossSpells.Add(bossSpell);
        bossSpell.gameObject.SetActive(true);
        bossSpell.transform.position = new Vector3(transform.position.x - 0.11f, spell.transform.position.y, spell.transform.position.z);

        // atkLoop를 0으로 초기화하지 않으면 보스가 공격해도 공격 루프 애니메이션이 진행된 시간만큼(normalizedTime) 공격 효과음이 나오지 않음
        attackLoop = 0;

        bossSpell.spellEnqueueAction += () =>
        {
            bossSpellQueue.Enqueue(bossSpell);
            activeBossSpells.Remove(bossSpell);
        };
        bossStateMachine.OnCheckRecastState();
    }

    private void OnCheckRecastState()
    {
        objectAnimator.SetFloat("spellAttack", Random.Range(0.3f, 1.0f));

        // spellAttack 값이 연속으로 spellAttackValue 보다 크게 나오면 특수 공격을 다시 실행시킴
        if (objectAnimator.GetFloat("spellAttack") > spellAttackValue)
            objectAnimator.SetTrigger("recast");
        else
            ChangeState(BossStateType.Attack);
    }

    public override float CurrentHp()
    {
        return runtimeStats.hp;
    }

    public override void GetAttackDamage(float dmg)
    {
        runtimeStats.hp -= dmg;
        TextPoolManager.Instance.ShowDamageText(dmg, textPos);

        if (runtimeStats.hp <= 0)
            Death();
    }

    public override void ResetAttackState()
    {
        base.ResetAttackState();
        ChangeState(BossStateType.Idle);
    }

    protected override void Death()
    {
        base.Death();
        ChangeState(BossStateType.Death);
        GameManager.Instance.gameGold.AddGold(giveGold);
        PlayDeadSound(bossData.objectStats.deadClip);
        healthSystem.NotifyDeath();
    }

    public override void CurrentAtk(float addAtk)
    {
        runtimeStats.attack += addAtk;
    }

    public override void HpUp(float addHp)
    {
        runtimeStats.hp += addHp;
        runtimeStats.maxHp += addHp;
    }

    public override void AddBuff(Buff buff) { }
    public override void CurrentHpChange(float value) { }
}
