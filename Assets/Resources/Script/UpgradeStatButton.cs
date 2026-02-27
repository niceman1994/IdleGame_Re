using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum UpgradeType
{
    ATTACK, HP, ATTACKSPEED, MOVESPEED, THUNDERPOWER, EARNGOLD
}

/// <summary>
/// 캐릭터 스탯을 올리기 위해 사용하는 클래스
/// </summary>
public class UpgradeStatButton : MonoBehaviour
{
    [SerializeField] UpgradeStatSO upgradeStatSO;
    [SerializeField] Button button;
    [SerializeField] Text goldText;
    [SerializeField] int clickCount;
    [Header("스탯을 올릴 주체")]
    [SerializeField] Player player;

    private BigInteger upgradeCost;
    private UpgradeSystem upgradeSystem;

    private void Start()
    {
        upgradeCost = upgradeStatSO.BaseCost;
        upgradeSystem = new UpgradeSystem(player, GameManager.Instance.gameGold);
        upgradeSystem.onUpgrade += () =>
        {
            SpendGold();
            DisplayGoldText();
            Click();
        };
        UpgradeButtonInit();
        goldText.text = GameManager.Instance.gameGold.FormatGoldToString(upgradeCost);
    }
    
    private void UpgradeButtonInit()
    {
        switch (upgradeStatSO.upgradeType)
        {
            case UpgradeType.ATTACK:
                button.onClick.AddListener(() => upgradeSystem.UpgradeAttack(upgradeCost));
                break;
            case UpgradeType.HP:
                button.onClick.AddListener(() => upgradeSystem.UpgradeHp(upgradeCost));
                break;
            case UpgradeType.ATTACKSPEED:
                button.onClick.AddListener(() => upgradeSystem.UpgradeAttackSpeed(upgradeCost));
                break;
            case UpgradeType.MOVESPEED:
                button.onClick.AddListener(() => upgradeSystem.UpgradeMoveSpeed(upgradeCost));
                break;
            case UpgradeType.THUNDERPOWER:
                button.onClick.AddListener(() => upgradeSystem.UpgradeThunderPower(upgradeCost));
                break;
            case UpgradeType.EARNGOLD:
                button.onClick.AddListener(() => upgradeSystem.UpgradeEarnGold(upgradeCost, clickCount));
                break;
        }
    }

    private void DisplayGoldText()
    {
        goldText.text = GameManager.Instance.gameGold.FormatGoldToString(upgradeCost);

        if (clickCount == upgradeStatSO.maxClickCount)
        {
            goldText.text = "max";
            button.enabled = false;
        }
    }

    private void SpendGold()
    {
        if (clickCount <= upgradeStatSO.maxClickCount)
        {
            GameManager.Instance.gameGold.SpendGold(upgradeCost);
            IncreaseGold();
        }
    }

    private void IncreaseGold()
    {
        upgradeCost += (upgradeCost / 4) + (upgradeCost / 5);
    }

    public void Click()
    {
        SoundManager.Instance.PlayUpgradeStatClickSound();
        clickCount += 1;
    }
}
