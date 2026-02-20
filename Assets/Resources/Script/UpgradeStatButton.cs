using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 스탯을 올리기 위해 사용하는 클래스
/// </summary>
public class UpgradeStatButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Text goldText;
    [SerializeField] int[] spendGold;   // 사용할 골드를 표시하기 위한 변수
    [SerializeField] int goldIndex;
    [SerializeField] int clickCount;
    [SerializeField] int maxCount;
    [Header("스탯을 올릴 주체")]
    [SerializeField] Player player;

    private Player playerStats;

    public event Action onDisplayGold;

    private void Start()
    {
        playerStats = player;
        goldText.text = $"{spendGold}";
        GameManager.Instance.gameGold.GoldTheorem(spendGold, goldIndex);
        goldText.text = GameManager.Instance.gameGold.GetMoneyToString(spendGold, goldIndex);

        onDisplayGold += () =>
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
        goldText.text =  GameManager.Instance.gameGold.GetMoneyToString(spendGold, goldIndex);

        if (clickCount == maxCount)
        {
            goldText.text = "max";
            button.enabled = false;
        }
    }

    private void SpendGold()
    {
        if (clickCount <= maxCount)
            GameManager.Instance.gameGold.SpendGold(spendGold, goldIndex, IncreaseGold);
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
                ClickUpgradeStatsButton(() => playerStats.CurrentAtk(0.25f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStats.CurrentAtk(0.25f));
    }

    public void AddHp()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStats.HpUp(UnityEngine.Random.Range(24.0f, 32.0f)));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStats.HpUp(UnityEngine.Random.Range(24.0f, 32.0f)));
    }

    public void AddAtkSpeed()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStats.GetAttackSpeed(0.02f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStats.GetAttackSpeed(0.02f));
    }

    public void AddMoveSpeed()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => playerStats.GetMoveSpeed(0.05f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => playerStats.GetMoveSpeed(0.05f));
    }

    public void AddSkillPower()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => GameManager.Instance.AddThunderPower(0.2f));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => GameManager.Instance.AddThunderPower(0.2f));
    }

    public void AddEarnGold()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                ClickUpgradeStatsButton(() => GameManager.Instance.AddEarnGold(clickCount));
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
            ClickUpgradeStatsButton(() => GameManager.Instance.AddEarnGold(clickCount));
    }

    public void Click()
    {
        SoundManager.Instance.PlayUpgradeStatClickSound();
        clickCount += 1;
    }

    private void ClickUpgradeStatsButton(Action buttonClickAction)
    {
        Click();
        buttonClickAction?.Invoke();
    }

    public void InvokeUpgradeEvent(UnityAction unityAction)
    {
        button.onClick.AddListener(() =>
        {
            unityAction();
            SpendGold();
            onDisplayGold.Invoke();
        });
    }
}
