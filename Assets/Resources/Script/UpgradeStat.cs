using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 스탯을 올리기 위해 사용하는 클래스
/// </summary>
public class UpgradeStat : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Text goldText;
    [SerializeField] int[] spendGold;   // 사용할 골드를 표시하기 위한 변수
    [SerializeField] int goldIndex;
    [SerializeField] int clickCount;
    [SerializeField] int maxCount;

    private Player playerStatus;

    public event Action goldIndexAction;

    private void Start()
    {
        goldText.text = spendGold.ToString();
        playerStatus = GameManager.Instance.player;
        GameManager.Instance.gameGold.GoldTheorem(spendGold, goldIndex);
        GameManager.Instance.gameGold.GetMoneyToString(spendGold, goldIndex, goldText);

        goldIndexAction += () =>
        {
            UpIndex();
            DisplayGoldText();
        };
    }

    private void UpIndex()
    {
        if (spendGold[goldIndex] > 1000)
            goldIndex += 1;
    }

    private void DisplayGoldText()
    {
        GameManager.Instance.gameGold.GoldTheorem(spendGold, goldIndex);
        GameManager.Instance.gameGold.GetMoneyToString(spendGold, goldIndex, goldText);

        if (clickCount == maxCount)
        {
            goldText.text = "max";
            button.enabled = false;
        }
    }

    private void SpendGold()
    {
        if (clickCount <= maxCount)
        {
            // 소유한 골드 인덱스와 소모할 골드 인덱스가 같을 때
            if (GameManager.Instance.gameGold.index == goldIndex)
            {
                // 소유한 골드가 소모할 골드 이상이면 골드를 사용함
                if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                {
                    GameManager.Instance.gameGold.curGold[goldIndex] -= spendGold[goldIndex];
                    IncreaseGold();
                }
                else    // 소유 골드가 소모 골드보다 적으면 계산이 안되도록 함
                    spendGold[goldIndex] += 0;
            }
            // 소유한 골드 인덱스가 소모할 골드 인덱스보다 클 때
            else if (GameManager.Instance.gameGold.index > goldIndex)
            {
                // 소유 골드 인덱스가 더 크더라도, 같은 골드 인덱스를 비교했을 때 소모 골드보다 소유 골드가 더 많으면 골드를 사용함
                if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                    GameManager.Instance.gameGold.curGold[goldIndex] -= spendGold[goldIndex];
                else
                {
                    // 같은 인덱스를 비교했을 때 소모 골드가 더 크면 소유 골드에 1000을 더하고 소모 골드를 뺀 다음에 소유 골드에서 1을 뺌
                    // 예) 1050 => 1.05A 인데 소모 골드가 150이라면 1050-150=900으로 계산함
                    GameManager.Instance.gameGold.curGold[goldIndex] = GameManager.Instance.gameGold.curGold[goldIndex] + 1000 - spendGold[goldIndex];
                    GameManager.Instance.gameGold.curGold[goldIndex + 1] -= 1;
                }
                IncreaseGold();
            }
            // 소유 골드 인덱스가 소모 골드 인덱스보다 작으면 계산이 안되도록 함
            else if (GameManager.Instance.gameGold.index < goldIndex)
                spendGold[goldIndex] += 0;
        }
    }

    private void IncreaseGold()
    {
        if (goldIndex >= 1)
        {
            for (int i = 1; i <= goldIndex; i++)
            {
                if (spendGold[i] / 10 == 0)
                    spendGold[i] += spendGold[i] / 3;
                else
                    spendGold[i] += spendGold[i] / 5;

                spendGold[i - 1] += spendGold[i - 1] < 100 ? spendGold[i - 1] * 3 : spendGold[i - 1];
            }
        }
        else
            spendGold[goldIndex] += (spendGold[goldIndex] / 4) + (spendGold[goldIndex] / 5);
    }

    public void AddAtk()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStatus.CurrentAtk(0.26f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStatus.CurrentAtk(0.26f));
    }

    public void AddHp()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStatus.HpUp(UnityEngine.Random.Range(20.0f, 24.0f)));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStatus.HpUp(UnityEngine.Random.Range(20.0f, 24.0f)));
    }

    /// <summary>
    /// <see cref="ListUp.SetStatusUpButtonEvent"/> 에서 호출되고 공격속도를 올리기 위해서 사용하는 함수
    /// </summary>
    public void AddAtkSpeed()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStatus.GetAttackSpeed(0.025f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStatus.GetAttackSpeed(0.025f));
    }

    public void AddMoveSpeed()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStatus.GetMoveSpeed(0.05f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStatus.GetMoveSpeed(0.05f));
    }

    public void AddSkillPower()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => GameManager.Instance.AddThunderPower(0.15f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => GameManager.Instance.AddThunderPower(0.15f));
    }

    public void AddEarnGold()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => GameManager.Instance.gameGold.getGold += 5 * clickCount);
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => GameManager.Instance.gameGold.getGold += 5 * clickCount);
    }

    public void Click()
    {
        SoundManager.Instance.upgradeSound.Play();
        clickCount += 1;
    }

    private void ClickUpgradeStatsButton(Action buttonClickAction)
    {
        Click();
        buttonClickAction?.Invoke();
    }

    public void SetUpgradeButtonEvent(UnityAction unityAction)
    {
        button.onClick.AddListener(() =>
        {
            unityAction();
            SpendGold();
            goldIndexAction.Invoke();
        });
    }
}
