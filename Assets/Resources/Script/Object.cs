using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Player, Skeleton, Mushroom, Goblin, FlyingEye, Boss
}

[Serializable]
public class ObjectStats
{
    public float baseHp;            // 체력이 0인 오브젝트의 체력을 재설정해 다시 쓰기 위한 변수
    public float baseAttack;
    public float baseAttackSpeed;
    public AudioClip attackClip;
    public AudioClip deadClip;
}

[Serializable]
public class ObjectRuntimeStats
{
    public float hp;
    public float maxHp;
    public float attack;
    public float attackSpeed;
}

public abstract class Object : MonoBehaviour, IObject
{
    [SerializeField] protected ObjectRuntimeStats runtimeStats;
    [SerializeField] protected AudioSource attackSound;
    [SerializeField] protected AudioSource deadSound;
    [SerializeField] protected Transform textPos;
    [SerializeField] protected DetectCollider detectCollider;
    [SerializeField] protected ObjectType objectType;

    protected bool isDeathAnimComplete;
    protected int attackLoop;
    protected int giveGold;
    protected BoxCollider2D ownCollider;
    protected Animator objectAnimator;

    public HealthSystem HealthSystem { get; private set; }

    protected virtual void Awake()
    {
        ownCollider = GetComponent<BoxCollider2D>();
        objectAnimator = GetComponent<Animator>();
        HealthSystem = GetComponent<HealthSystem>();
    }

    protected virtual void OnEnable()
    {
        objectAnimator.Rebind();
        objectAnimator.Update(0.0f);
        objectAnimator.SetFloat("attackSpeed", runtimeStats.attackSpeed);
    }

    protected void PlayAttackSound(AudioClip attackClip)
    {
        if (detectCollider.targetAttack != null)
        {
            attackLoop++;
            detectCollider.targetAttack.GetAttackDamage(runtimeStats.attack); // 공격력만큼 탐지된 적의 체력을 깎는 함수를 호출
            attackSound.clip = attackClip;
            attackSound.Play();
        }
    }

    protected void PlayDeadSound(AudioClip deadClip)
    {
        if (deadSound != null)
        {
            deadSound.clip = deadClip;
            deadSound.Play();
        }

        //if (deadAudio != null)
        //{
        //    deadAudio.clip = SoundManager.Instance.deadSounds[soundIndex].audioClip;
        //    deadAudio.Play();
        //}
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

    protected void Death(Action deadAction)
    {
        runtimeStats.hp = 0;
        attackLoop = 0;
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

    public IEnumerator OnMonsterDeathComplete()
    {
        // 오브젝트 풀링으로 죽은 몬스터를 빠르게 회수하면 죽는 소리가 짤려서 잠깐 기다림
        yield return new WaitForSeconds(0.65f);

        ObjectPoolManager.Instance.ReturnPooledObject(this);
        isDeathAnimComplete = false;
        runtimeStats.hp = runtimeStats.maxHp;
    }

    public void RegisterDeathCallback()
    {
        detectCollider.DetectedEnemy.HealthSystem.onDeath += ResetAttackState;
        Debug.Log("ResetAttackState 구독 완료");
    }

    private void ResetAttackState()
    {
        Debug.Log("ResetAttackState 호출");
        attackLoop = 0;
        detectCollider.EmptyDetectCollider2D();
        objectAnimator.SetBool("attack", false);

        //if (detectCollider.targetObject.CurrentHp() <= 0)
        //{
        //    attackLoop = 0;
        //    detectCollider.EmptyDetectCollider2D();
        //    objectAnimator.SetBool("attack", false);
        //}
    }

    public void RemoveDeathCallback()
    {
        detectCollider.DetectedEnemy.HealthSystem.onDeath -= ResetAttackState;
        Debug.Log("ResetAttackState 해지 완료");
    }

    public void ResetObjectStatus()
    {
        runtimeStats.hp = runtimeStats.maxHp;
        attackLoop = 0;
        detectCollider.EmptyDetectCollider2D();
    }

    protected void SetDefaultStats(float baseHp, float baseAttack, float baseAttackSpeed)
    {
        runtimeStats.hp = runtimeStats.maxHp = baseHp;
        runtimeStats.attack = baseAttack;
        runtimeStats.attackSpeed = baseAttackSpeed;
    }

    public bool ComparePooledObjectType(Object compareObject)
    {
        return objectType == compareObject.objectType;
    }


    public bool IsObjectAnimComplete(string animName)
    {
        bool isDeathComplete = objectAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName) &&
            objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !isDeathAnimComplete;

        return isDeathComplete;
    }

    public abstract void CheckState();
    public abstract float CurrentHp();
    public abstract void CurrentHpChange(float currentHp);
    public abstract float HpUp(float addHp);
    public abstract float CurrentAtk(float addAtk);
    public abstract void GetAttackDamage(float dmg);
}
