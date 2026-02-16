using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle, Run, Attack, Death
}

public class Player : Object
{
    [Header("Object를 상속받은 Player 스크립트")]
    [SerializeField] PlayerStatSO playerData;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] PlayerState currentState;
    [SerializeField] BuffController buffController;
     
    private float runtimeMoveSpeed;
    private StateMachine playerStateMachine;
    private PlayerState previousState;

    protected override void Awake()
    {
        base.Awake();
        SetPlayerStatus();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void Update()
    {
        CheckState();
        ChangeState(currentState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            collision.gameObject.SetActive(false);
            // 플레이어와 충돌한 객체 정보를 받아옴
            GameManager.Instance.Inventory.GetItem(collision.gameObject.GetComponent<Item>());
            // 아이템 정보를 받아왔으니 인벤토리 슬롯을 갱신
            GameManager.Instance.Inventory.UpdateItemSlot();
        }
    }

    private void SetPlayerStatus()
    {
        SetDefaultStats(playerData.playerStats.baseHp, playerData.playerStats.baseAttack, playerData.playerStats.baseAttackSpeed);
        runtimeMoveSpeed = moveSpeed = playerData.baseMoveSpeed;
        healthSystem.onHealthDamaged += GetCurrentHp;

        playerStateMachine = new StateMachine(this);
        playerStateMachine.ChangeState(playerStateMachine.RunState);
        currentState = PlayerState.Run;
        ShowCurrentHp();
    }

    private void PlayerMove()
    {
        if (detectCollider.DetectedEnemy == null)
        {
            if (transform.localPosition != endPoint.localPosition)
            {
                if (currentState != PlayerState.Death)
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, endPoint.localPosition, moveSpeed * Time.deltaTime);
                else
                    transform.localPosition = Vector2.MoveTowards(transform.localPosition, endPoint.localPosition, 0.0f);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    private void ChangeState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                if (previousState != currentState)
                    playerStateMachine.ChangeState(playerStateMachine.IdleState);
                break;
            case PlayerState.Run:
                if (previousState != currentState)
                    playerStateMachine.ChangeState(playerStateMachine.RunState);

                playerState = detectCollider.DetectedEnemy != null ? PlayerState.Attack : PlayerState.Run;
                break;
            case PlayerState.Attack:
                if (previousState != currentState)
                    playerStateMachine.ChangeState(playerStateMachine.AttackState);
                break;
            case PlayerState.Death:
                if (previousState != currentState)
                    playerStateMachine.ChangeState(playerStateMachine.DeathState);
                break;
        }
        previousState = currentState;
        currentState = playerState;
    }

    public override void CheckState()
    {
        if (runtimeStats.hp > 0 && objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
            /* normalizedTimeInProcess만 있으면 0.8f 이상부터는 계속 데미지를 계산해 한 번의 공격에 몬스터가 죽게 되고
            normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
            if (AttackStateProcess() >= 0.8f && AttackStateTime() > attackLoop)
                PlayAttackSound(playerData.playerStats.attackClip);
        }
    }

    public override void AddBuff(Buff buff)
    {
        buffController.AddBuff(buff);
    }

    private void ShowCurrentHp()
    {
        GameManager.Instance.hpBar.fillAmount = runtimeStats.hp / runtimeStats.maxHp;
        GameManager.Instance.currentHp.text = Math.Truncate(runtimeStats.hp).ToString();
        GameManager.Instance.maxHp.text = Math.Truncate(runtimeStats.maxHp).ToString();
    }

    private void GetCurrentHp(float currentHp)
    {
        runtimeStats.hp = currentHp;
        ShowCurrentHp();
    }

    public override void GetAttackDamage(float dmg)
    {
        healthSystem.TakeDamage(runtimeStats.hp, dmg);

        if (runtimeStats.hp <= 0)
        {
            buffController.ClearAllBuffs();
            Death();
        }
    }

    protected override void ResetAttackState()
    {
        base.ResetAttackState();
        ChangeState(PlayerState.Run);
    }

    protected override void Death()
    {
        base.Death();
        ChangeState(PlayerState.Death);
        PlayDeadSound(playerData.playerStats.deadClip);
        detectCollider.enabled = false;
        healthSystem.NotifyDeath();
    }

    public void OnPlayerDeathComplete()
    {
        isDeathAnimComplete = true;
        StartCoroutine(DeathAfterSequence());
    }

    private IEnumerator DeathAfterSequence()
    {
        yield return new WaitForSeconds(1.2f);
        ObjectPoolManager.Instance.ReturnPooledMonsters();
        yield return new WaitUntil(() => ObjectPoolManager.Instance.IsReturnComplete());

        runtimeStats.hp = runtimeStats.maxHp;
        isDeathAnimComplete = false;
        ResetPosition();
        ObjectPoolManager.Instance.StageDown();
    }

    private void ResetPosition()
    {
        attackLoop = 0;
        transform.localPosition = startPoint.localPosition;
        currentState = PlayerState.Run;
        playerStateMachine.ChangeState(playerStateMachine.RunState);

        ShowCurrentHp();
        moveSpeed = runtimeMoveSpeed;
        ownCollider.enabled = true;
        detectCollider.enabled = true;
    }

    public IEnumerator StageUp()
    {
        yield return new WaitForSeconds(2.0f);      // 죽인 몬스터를 잠깐 대기 후 큐로 회수하기 때문에 2초 뒤에 다음 스테이지로 넘어감
        ResetPosition();
        ObjectPoolManager.Instance.StageUp();
    }

    public float GetMoveSpeed(float speed)
    {
        moveSpeed += speed;
        runtimeMoveSpeed = moveSpeed;
        return moveSpeed;
    }

    // 스탯을 올리는 UI에서 강화할 경우 호출되는 함수
    public float GetAttackSpeed(float atkSpeed)
    {
        runtimeStats.attackSpeed += atkSpeed;
        objectAnimator.SetFloat("attackSpeed", runtimeStats.attackSpeed);
        return runtimeStats.attackSpeed;
    }

    public override float CurrentHp()
    {
        return runtimeStats.hp;
    }

    public override void CurrentHpChange(float value)
    {
        if (runtimeStats.hp <= 0) return;

        // 포션으로 회복할 수치와 현재 체력의 합이 최대 체력보다 많으면 최대 체력만큼만 회복되도록 함
        if (runtimeStats.hp + value > runtimeStats.maxHp)
            value -= runtimeStats.hp + value - runtimeStats.maxHp;

        runtimeStats.hp += value;
        
        ShowCurrentHp();
        TextPoolManager.Instance.ShowHealText(value, textPos, new Color(0.0f, 255.0f, 0.0f, 255.0f));
    }

    public override void HpUp(float addHp)
    {
        runtimeStats.hp += addHp;
        runtimeStats.maxHp += addHp;
        healthSystem.ChangeHealth(runtimeStats.hp, runtimeStats.maxHp, ShowCurrentHp);
    }

    public override void CurrentAtk(float addAtk)
    {
        runtimeStats.attack += addAtk;
    }
}
