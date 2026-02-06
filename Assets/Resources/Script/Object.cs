using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Player, Skeleton, Mushroom, Goblin, FlyingEye, Boss
}

public abstract class Object : MonoBehaviour, IObject
{
    [Header("Object 스크립트")]
    [SerializeField] protected float hp;
    [SerializeField] protected float atk;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected AudioSource attackSound;
    [SerializeField] protected AudioSource deadSound;
    [SerializeField] protected Transform textPos;
    [SerializeField] protected DetectCollider detectCollider;
    [SerializeField] protected ObjectType objectType;

    protected bool isDead;
    protected int atkLoop;
    protected int giveGold;
    protected float defaultHp;              // 체력이 0인 오브젝트의 체력을 재설정해 다시 쓰기 위한 변수
    protected float defaultAtk;
    protected BoxCollider2D ownCollider;
    protected Animator objectAnimator;

    protected virtual void OnEnable()
    {
        ownCollider = GetComponent<BoxCollider2D>();
        objectAnimator = GetComponent<Animator>();
        objectAnimator.Rebind();
        objectAnimator.Update(0.0f);
        objectAnimator.SetFloat("attackSpeed", attackSpeed);
    }

    protected virtual void Start()
    {
        defaultHp = hp;
        defaultAtk = atk;
        atkLoop = 0;
    }

    protected void Death(Action deadAction)
    {
        hp = 0;
        atkLoop = 0;
        ownCollider.enabled = false;
        detectCollider.EmptyDetectCollider2D();
        objectAnimator.SetBool("attack", false);
        objectAnimator.SetBool("death", true);

        if (CompareTag("Monster") || CompareTag("Boss"))
        {
            GameManager.Instance.gameGold.curGold[0] += giveGold;
            deadAction.Invoke();
        }

        if (gameObject.CompareTag("Monster"))
            ItemManager.Instance.SpawnItem(transform.position);
    }

    protected IEnumerator OnMonsterDeathComplete()
    {
        // 오브젝트 풀링으로 죽은 몬스터를 빠르게 회수하면 죽는 소리가 짤려서 잠깐 기다림
        yield return new WaitForSeconds(0.35f);

        ObjectPoolManager.Instance.ReturnPooledObject(this);
        isDead = false;
        hp = defaultHp;
    }

    protected void PlayAttackSound(AudioSource attackAudio, int soundIndex)
    {
        if (detectCollider.getTargetAttack != null)
        {
            atkLoop += 1;
            detectCollider.getTargetAttack.GetAttackDamage(atk); // 공격력만큼 탐지된 적의 체력을 깎는 함수를 호출
            attackAudio.clip = SoundManager.Instance.attackSounds[soundIndex].audioClip;
            attackAudio.Play();
        }
    }

    protected void PlayDeadSound(AudioSource deadAudio, int soundIndex)
    {
        if (deadAudio != null)
        {
            deadAudio.clip = SoundManager.Instance.deadSounds[soundIndex].audioClip;
            deadAudio.Play();
        }
    }

    protected float AttackStateTime()
    {
        // 애니메이션이 종료되지 않으면 normalizedTime 변수가 1이상의 값이 됨
        return objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    protected float AttackStateProcess()
    {
        // normalizedTime에서 소수점을 버린 Mathf.Floor(normalizedTime)을 빼면 소수점만 남음
        return AttackStateTime() - Mathf.Floor(AttackStateTime());
    }

    /// <summary>
    /// 몬스터가 플레이어의 체력을 0으로 만들었을 때 실행되는 함수
    /// </summary>
    protected void ClearAttackTarget()
    {
        if (detectCollider.getTargetObject.CurrentHp() <= 0)
        {
            atkLoop = 0;
            detectCollider.EmptyDetectCollider2D();
            objectAnimator.SetBool("attack", false);
        }
    }

    public bool CompareObjectType(Object compareObject)
    {
        return objectType == compareObject.objectType;
    }

    public void ResetObjectStatus()
    {
        hp = defaultHp;
        atk = defaultAtk;
        atkLoop = 0;
        detectCollider.EmptyDetectCollider2D();
    }

    protected bool IsObjectAnimComplete(string animName)
    {
        bool isDeathComplete = objectAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName) &&
            objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !isDead;

        return isDeathComplete;
    }

    public abstract void CheckState();
    public abstract float CurrentHp();
    public abstract void CurrentHpChange(float currentHp);
    public abstract float HpUp(float addHp);
    public abstract float CurrentAtk();
    public abstract float CurrentAtk(float addAtk);
    public abstract void GetAttackDamage(float dmg);
}
