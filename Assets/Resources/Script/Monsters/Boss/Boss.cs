using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Object
{
    [SerializeField] BossSpell spell;
    [SerializeField] AudioSource castSound;

    private float random;
    private Queue<BossSpell> bossSpellQueue = new Queue<BossSpell>();

    private void Start()
    {
        giveGold += 500;
        objectAnimator = GetComponent<Animator>();
        objectAnimator.SetFloat("attackspeed", attackSpeed);
        PrepareSpell();
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
            else
            {
                if (random > 0.85f)
                    CastState();
                else AttackState();
            }
        }
        else
        {
            objectAnimator.SetBool("death", true);
            Death(() => PlayDeadSound(deadSound, 1));
        }
    }

    private void PrepareSpell()
    {
        for (int i = 0; i < 3; i++)
        {
            BossSpell spellObject = Instantiate(spell, transform);
            spellObject.gameObject.SetActive(false);
            spellObject.transform.localScale = new Vector3(spellObject.transform.localScale.x / transform.localScale.x,
            spellObject.transform.localScale.y / transform.localScale.y, 1.0f);
            spellObject.name = spell.name;
            bossSpellQueue.Enqueue(spellObject);
        }
    }

    private void EnemyDetect()
    {
        if (detectCollider.getEnemyCollider != null && detectCollider.getEnemyCollider.CompareTag("Player"))
            objectAnimator.SetBool("attack", true);
    }

    private void AttackState()
    {
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (AttackStateProcess() > 0.625f && AttackStateTime() > atkLoop)
            {
                PlayAttackSound(attackSound, 1);
                random = Random.Range(0.3f, 1.0f);
                ClearAttackTarget();
            }
        }
    }

    private void CastState()
    {
        objectAnimator.SetBool("cast", true);

        /* AttackState와 CastState에 if문을 넣지 않으면 루프가 아닌
         CastState의 normalizedTime의 값이 1.0f보다 커져서 보스가 공격할 때 소리가 나지않는 현상이 발생 */
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (normalizedTime >= 1.0f)
            {
                PlayAttackSound(castSound, 6);
                CastSpell(new Vector3(transform.position.x - 0.11f, spell.transform.position.y, spell.transform.position.z));

                // random 값에 Random.Range를 설정하지 않으면 spellObj가 하나만 만들어지는 것이 아니라 여러개 만들어짐
                random = Random.Range(0.3f, 1.0f);
                objectAnimator.SetBool("cast", false);
                // atkLoop를 0으로 초기화하지 않으면 보스가 공격해도 AttackState의 normalizedTime만큼 소리가 나지 않음
                atkLoop = 0;
            }
        }
    }

    /// <summary>
    /// <see cref="BossSpell.spellEvent"/> 변수를 여기서 사용함
    /// </summary>
    private void CastSpell(Vector3 spellPosition)
    {
        var castSpell = bossSpellQueue.Dequeue();
        castSpell.gameObject.SetActive(true);
        castSpell.transform.position = spellPosition;
        castSpell.spellEvent += () => bossSpellQueue.Enqueue(castSpell);
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
        defaultHp += addHp;
        return hp;
    }
}
