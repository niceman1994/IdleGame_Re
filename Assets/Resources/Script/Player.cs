using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    Idle, Run, Attack, Death
}

public class Player : Object
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] PlayerState currentState;
    [SerializeField] HealthSystem healthSystem;

    private PlayerState previousState;

    private void OnEnable()
    {
        healthSystem.onHealthChanged += UpdateCurrentHp;
    }

    private void Start()
    {
        GameManager.Instance.userSpeed = moveSpeed;
        currentState = PlayerState.Run;
        objectAnimator = GetComponent<Animator>();
        objectAnimator.SetFloat("attackSpeed", attackSpeed);
        ShowCurrentHp();
    }

    // Update에 이동 함수를 넣으면 캐릭터가 떨리면서 이동하기 때문에 FixedUpdate에 넣었음
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
            GameManager.Instance.inventory.GetItem(collision.gameObject.GetComponent<Item>());
            // 아이템 정보를 받아왔으니 인벤토리 슬롯을 갱신
            GameManager.Instance.inventory.UpdateItemSlot();
        }
    }

    private void PlayerMove()
    {
        if (detectCollider.getEnemyCollider == null)
        {
            if (transform.position != endPoint.localPosition)
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, endPoint.localPosition, moveSpeed * Time.deltaTime);
            else
            {
                ChangeState(PlayerState.Idle);
                Invoke("StageUp", 2.0f);        // 죽인 몬스터를 1.5초 후에 큐로 회수하기 때문에 2초 뒤에 다음 스테이지로 넘어감
            }
        }
    }

    private void ChangeState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                if (previousState != currentState)
                {
                    objectAnimator.SetBool("attack", false);
                    objectAnimator.SetBool("idle", true);
                }
                break;
            case PlayerState.Run:
                if (previousState != currentState)
                {
                    objectAnimator.SetBool("attack", false);
                    objectAnimator.SetBool("idle", false);
                }
                atkLoop = 0;
                playerState = detectCollider.getEnemyCollider != null ? PlayerState.Attack : PlayerState.Run;
                break;
            case PlayerState.Attack:
                if (previousState != currentState)
                {
                    objectAnimator.SetBool("attack", true);
                    objectAnimator.SetBool("idle", false);
                }
                break;
            case PlayerState.Death:
                if (previousState != currentState)
                {
                    objectAnimator.SetBool("attack", false);
                    objectAnimator.SetBool("idle", false);
                    objectAnimator.SetBool("death", true);
                }
                Invoke("ResetGame", 2.0f);
                break;
            default:
                break;
        }
        previousState = currentState;
        currentState = playerState;
    }

    public override void CheckState()
    {
        if (hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // targetCollider의 현재 체력이 0 이하일 경우
                if (detectCollider.getTargetObject.CurrentHp() <= 0)
                {
                    atkLoop = 0;
                    detectCollider.DeleteCollider2D();
                    ChangeState(PlayerState.Run);
                    return;
                }

                // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
                /* normalizedTimeInProcess만 있으면 0.8f 이상부터는 계속 데미지를 계산해 한 번의 공격에 몬스터가 죽게 되고
                normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
                if (AttackStateProcess() >= 0.8f && AttackStateTime() > atkLoop)
                {
                    PlayAttackSound(attackSound, 0);
                }
            }
        }
        else
        {
            ChangeState(PlayerState.Death);
            Death(() => PlayDeadSound(deadSound, 0));
        }
    }

    private void StageUp()
    {
        if (currentState == PlayerState.Idle)
        {
            ResetPosition();
            ObjectPoolManager.Instance.StageUp();
        }
    }

    private void ResetGame()
    {
        if (currentState == PlayerState.Death)
        {
            ResetPosition();
            ObjectPoolManager.Instance.StageDown();
        }
    }

    private void ResetPosition()
    {
        atkLoop = 0;
        transform.localPosition = startPoint.localPosition;
        objectAnimator.SetBool("death", false);
        currentState = PlayerState.Run;
        moveSpeed = GameManager.Instance.userSpeed;
        ownCollider.enabled = true;
        detectCollider.DeleteCollider2D();
    }

    private void ShowCurrentHp()
    {
        GameManager.Instance.hpBar.fillAmount = hp / defaultHp;
        GameManager.Instance.currentHp.text = Math.Truncate(hp).ToString();
        GameManager.Instance.maxHp.text = Math.Truncate(defaultHp).ToString();
    }

    private void UpdateCurrentHp(float currentHp)
    {
        hp = currentHp;
        ShowCurrentHp();
    }

    public float GetMoveSpeed(float speed)
    {
        moveSpeed += speed;
        GameManager.Instance.userSpeed = moveSpeed;
        return moveSpeed;
    }

    // 스탯을 올리는 UI에서 강화할 경우 호출되는 함수
    public float GetAttackSpeed(float atkSpeed)
    {
        attackSpeed += atkSpeed;
        objectAnimator.SetFloat("attackSpeed", attackSpeed);
        return attackSpeed;
    }

    public override void GetAttackDamage(float dmg)
    {
        healthSystem.TakeDamage(hp, dmg);
    }

    public override float CurrentHp()
    {
        return hp;
    }

    public override void CurrentHp(float value)
    {
        if (hp + value > defaultHp)
            value -= hp + value - defaultHp;

        hp += value;
        
        ShowCurrentHp();
        TextPoolManager.Instance.ShowHealText(value, textPos, new Color(0.0f, 255.0f, 0.0f, 255.0f));
    }

    public override float HpUp(float addHp)
    {
        hp += addHp;
        defaultHp += addHp;
        ShowCurrentHp();

        return hp;
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
}
