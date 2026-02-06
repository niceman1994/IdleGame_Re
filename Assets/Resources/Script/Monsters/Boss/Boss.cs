using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Object
{
    [Header("Object를 상속받은 Boss 스크립트")]
    [SerializeField] bool isSpellCast;
    [SerializeField] BossSpell spell;
    [SerializeField] AudioSource castSound;

    private int lastSpellAttackCount;
    private int currentSpellAttackCount;
    private float spellAttackValue;
    private Queue<BossSpell> bossSpellQueue = new Queue<BossSpell>();
    // 플레이어가 죽어서 스테이지 초기화됐을 때 BossSpell이 회수되지 않을 수 있는걸 방지하기 위한 변수
    private List<BossSpell> activeBossSpells = new List<BossSpell>();

    private void OnDisable()
    {
        for (int i = 0; i < activeBossSpells.Count; i++)
            bossSpellQueue.Enqueue(activeBossSpells[i]);

        activeBossSpells.Clear();
        isSpellCast = false;
    }

    protected override void Start()
    {
        InitBoss();
        base.Start();
    }

    private void Update()
    {
        CheckState();
    }

    private void InitBoss()
    {
        spellAttackValue = 0.75f;
        lastSpellAttackCount = 0;
        currentSpellAttackCount = 0;
        isSpellCast = false;
        giveGold += 500;
        PrepareSpell();
    }

    public override void CheckState()
    {
        if (hp > 0)
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
        else if (IsObjectAnimComplete("Death"))
            StartCoroutine(OnMonsterDeathComplete());
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
        if (detectCollider.IsDetectEnemyCollider("Player"))
            objectAnimator.SetBool("attack", true);
    }

    private void AttackState()
    {
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (AttackStateProcess() > 0.625f && AttackStateTime() > atkLoop)
            {
                PlayAttackSound(attackSound, 1);
                ClearAttackTarget();
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
        castSound.clip = SoundManager.Instance.attackSounds[6].audioClip;
        castSound.Play();

        var bossSpell = bossSpellQueue.Dequeue();
        activeBossSpells.Add(bossSpell);
        bossSpell.gameObject.SetActive(true);
        bossSpell.transform.position = new Vector3(transform.position.x - 0.11f, spell.transform.position.y, spell.transform.position.z);

        // atkLoop를 0으로 초기화하지 않으면 보스가 공격해도 AttackState의 normalizedTime만큼 소리가 나지 않음
        atkLoop = 0;

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
        {
            isSpellCast = false;
            Death(() => PlayDeadSound(deadSound, 1));
        }
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
