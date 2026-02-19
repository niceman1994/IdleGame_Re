using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Object
{
    [Header("Object를 상속받은 Boss 스크립트")]
    [SerializeField] MonsterStatSO bossData;
    [SerializeField] bool isSpellCast;
    [SerializeField] BossSpell spell;
    [SerializeField] AudioSource castSound;

    private int lastSpellAttackCount;
    private int currentSpellAttackCount;
    private float spellAttackValue;
    private Queue<BossSpell> bossSpellQueue = new Queue<BossSpell>();
    // 플레이어가 죽어서 스테이지가 초기화됐을 때 BossSpell이 회수되지 않을 수 있는걸 방지하기 위한 변수
    private List<BossSpell> activeBossSpells = new List<BossSpell>();

    private void OnDisable()
    {
        for (int i = 0; i < activeBossSpells.Count; i++)
            bossSpellQueue.Enqueue(activeBossSpells[i]);

        activeBossSpells.Clear();
        isSpellCast = false;
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

        healthSystem.onDamagedTaken += TextPoolManager.Instance.ShowDamageText;
        spellAttackValue = 0.75f;
        lastSpellAttackCount = 0;
        currentSpellAttackCount = 0;
        isSpellCast = false;
        giveGold = bossData.giveGold;
        PrepareSpell();
    }

    public override void CheckState()
    {
        if (runtimeStats.hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                EnemyDetect();
            else
            {
                if (objectAnimator.GetFloat("spellAttack") > spellAttackValue && !isSpellCast)
                    objectAnimator.SetBool("cast", isSpellCast = true);
                else if (isSpellCast)
                    CastState();
                else
                    AttackState();
            }
        }
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

    private void EnemyDetect()
    {
        if (detectCollider.IsDetectEnemy("Player"))
            objectAnimator.SetBool("attack", true);
    }

    private void AttackState()
    {
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (AttackStateProcess() > 0.625f && AttackStateTime() > attackLoop)
            {
                PlayAttackSound(bossData.objectStats.attackClip);
                objectAnimator.SetFloat("spellAttack", Random.Range(0.25f, 1.0f));
            }
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

        // atkLoop를 0으로 초기화하지 않으면 보스가 공격해도 AttackState의 normalizedTime만큼 소리가 나지 않음
        attackLoop = 0;

        bossSpell.spellEnqueueAction += () =>
        {
            bossSpellQueue.Enqueue(bossSpell);
            activeBossSpells.Remove(bossSpell);
        };

        objectAnimator.SetBool("cast", isSpellCast = false);
        objectAnimator.SetFloat("spellAttack", Random.Range(0.25f, 1.0f));

        // spellAttack 값이 연속으로 spellAttackRandomValue 보다 크게 나오면 Cast 애니메이션을 다시 실행시킴
        if (objectAnimator.GetFloat("spellAttack") > spellAttackValue)
            objectAnimator.SetTrigger("recast");
    }

    protected override void Death()
    {
        base.Death();
        GameManager.Instance.goldManager.curGold[0] += giveGold;
        isSpellCast = false;
        PlayDeadSound(bossData.objectStats.deadClip);
        healthSystem.NotifyDeath();
    }

    public override void CurrentAtk(float addAtk)
    {
        runtimeStats.attack += addAtk;
    }

    public override void GetAttackDamage(float dmg)
    {
        runtimeStats.hp -= dmg;
        healthSystem.ShowDamageText(dmg, textPos);

        if (runtimeStats.hp <= 0)
            Death();
    }

    public override float CurrentHp()
    {
        return runtimeStats.hp;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 함수
    public override void HpUp(float addHp)
    {
        runtimeStats.hp += addHp;
        runtimeStats.maxHp += addHp;
    }

    public override void AddBuff(Buff buff) { }
    public override void CurrentHpChange(float value) { }
}
