using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상호작용하는 오브젝트에 붙여서 사용하는 스크립트
/// </summary>
public abstract class Object : MonoBehaviour, IObject
{
    [SerializeField] protected float hp;
    [SerializeField] protected float atk;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected AudioSource attackSound;
    [SerializeField] protected AudioSource deadSound;
    [SerializeField] protected Transform textPos;
    [SerializeField] protected BoxCollider2D destroyOwnCollider;
    [SerializeField] protected DetectCollider detectCollider;

    protected int atkLoop;
    protected int giveGold;
    protected float maxHp;
    protected float defaultAtk;
    protected Animator objectAnimator;

    private void Start()
    {
        maxHp = hp;
        defaultAtk = atk;
        atkLoop = 0;
    }

    protected void ChangeDefault()
    {
        if (hp > maxHp && hp < 100000)
            maxHp = hp;

        if (atk > defaultAtk && atk < 10000)
            defaultAtk = atk;
    }

    public void Death()
    {
        hp = 0;
        atkLoop = 0;
        destroyOwnCollider.enabled = false;
        StartCoroutine(ResetObject());

        if (gameObject.CompareTag("Monster") || gameObject.CompareTag("Boss"))
        {
            objectAnimator.SetBool("attack", false);
            objectAnimator.SetBool("death", true);

            if (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0f)
            {
                GameManager.Instance.gameGold.curGold[0] += giveGold;
                deadSound.Play();

                if (gameObject.CompareTag("Monster"))
                    ItemManager.Instance.SpawnItem(transform.position);
            }
        }
    }

    private IEnumerator ResetObject()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        if (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (objectAnimator.CompareTag("Monster"))
            {
                yield return waitForSeconds;
                hp = maxHp;
                ObjectSpawn.Instance.PullObject(gameObject);
            }
            else if (objectAnimator.CompareTag("Boss"))
            {
                yield return waitForSeconds;
                hp = maxHp;
                ObjectSpawn.Instance.ReturnPoolingBoss(gameObject);
            }
            else if (objectAnimator.CompareTag("Player"))
            {
                yield return waitForSeconds;
                hp = maxHp;
            }
        }
    }

    protected void PlayAttackSound(AudioSource attackAudio, int soundIndex)
    {
        atkLoop += 1;
        detectCollider.getTargetAttack.GetAttackDamage(atk); // detectCollider 체력을 공격력만큼 깎는 메서드를 호출
        attackAudio.clip = SoundManager.Instance.attackSounds[soundIndex].audioClip;
        attackAudio.Play();
    }

    protected float AttackStateTime()
    {
        // 애니메이션이 루프일 경우 1이상의 시간을 가짐
        return objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    protected float AttackStateProcess()
    {
        // normalizedTime에서 소수점을 버린 Mathf.Floor(normalizedTime)을 빼면 소수점만 남음
        return AttackStateTime() - Mathf.Floor(AttackStateTime());
    }

    protected void ClearAttackTarget()
    {
        if (detectCollider.getTargetObject.CurrentHp() <= 0)
        {
            detectCollider.DeleteCurrentCollider2D();
            objectAnimator.SetBool("attack", false);
            atkLoop = 0;
        }
    }

    public abstract void CheckState();
    public abstract float CurrentHp();
    public abstract void CurrentHp(float currentHp);
    public abstract float HpUp(float addHp);
    public abstract float CurrentAtk();
    public abstract float CurrentAtk(float addAtk);
    public abstract void GetAttackDamage(float dmg);
}
