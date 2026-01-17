using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeStatus : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Text goldText;
    [SerializeField] int[] spendGold;
    [SerializeField] int goldIndex;
    [SerializeField] int clickCount;
    [SerializeField] int maxCount;
    private Player playerStatus;

    private void Start()
    {
        goldText.text = spendGold.ToString();
        playerStatus = GameManager.Instance.player;
    }

    private void Update()
    {
        UpIndex();
        DisplayGoldText();
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
            if (GameManager.Instance.gameGold.index == goldIndex)
            {
                if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                {
                    GameManager.Instance.gameGold.curGold[goldIndex] -= spendGold[goldIndex];
                    IncreaseGold();
                }
                else
                    spendGold[goldIndex] += 0;
            }
            else if (GameManager.Instance.gameGold.index > goldIndex)
            {
                if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
                    GameManager.Instance.gameGold.curGold[goldIndex] -= spendGold[goldIndex];
                else
                {
                    GameManager.Instance.gameGold.curGold[goldIndex] = GameManager.Instance.gameGold.curGold[goldIndex] + 1000 - spendGold[goldIndex];
                    GameManager.Instance.gameGold.curGold[goldIndex + 1] -= 1;
                }
                IncreaseGold();
            }
            else if (GameManager.Instance.gameGold.index < goldIndex)
                spendGold[goldIndex] += 0;
        }
    }

    private void IncreaseGold()
    {
        if (goldIndex >= 1)
        {
            for (int j = 1; j <= goldIndex; ++j)
            {
                if (spendGold[j] / 10 == 0)
                    spendGold[j] += spendGold[j] / 3;
                else
                    spendGold[j] += spendGold[j] / 5;

                spendGold[j - 1] += spendGold[j - 1] < 100 ? spendGold[j - 1] * 3 : spendGold[j - 1];
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
            {
                Click();
                playerStatus.CurrentAtk(0.26f);
            }
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
        {
            Click();
            playerStatus.CurrentAtk(0.26f);
        }
    }

    public void AddHp()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.HpUp(UnityEngine.Random.Range(20.0f, 24.0f));
            }
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
        {
            Click();
            playerStatus.HpUp(UnityEngine.Random.Range(20.0f, 24.0f));
        }
    }

    /// <summary>
    /// <see cref="ListUp.SetStatusUpButtonEvent"/> 에서 호출되고 공격속도를 올리기 위해서 사용하는 함수
    /// </summary>
    public void AddAtkSpeed()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.GetAttackSpeed(0.025f);
            }
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
        {
            Click();
            playerStatus.GetAttackSpeed(0.025f);
        }
    }

    public void AddMoveSpeed()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.GetMoveSpeed(0.05f);
            }
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
        {
            Click();
            playerStatus.GetMoveSpeed(0.05f);
        }
    }

    public void AddSkillPower()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                GameManager.Instance.AddThunderPower(0.15f);
            }
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
        {
            Click();
            GameManager.Instance.AddThunderPower(0.15f);
        }
    }

    public void AddEarnGold()
    {
        if (GameManager.Instance.gameGold.index == goldIndex)
        {
            if (GameManager.Instance.gameGold.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                GameManager.Instance.gameGold.getGold += 5 * clickCount;
            }
        }
        else if (GameManager.Instance.gameGold.index > goldIndex)
        {
            Click();
            GameManager.Instance.gameGold.getGold += 5 * clickCount;
        }
    }

    public void Click()
    {
        SoundManager.Instance.upgradeSound.Play();
        clickCount += 1;
    }

    public void SetUpgradeButtonEvent(UnityAction unityAction)
    {
        button.onClick.AddListener(() =>
        {
            unityAction();
            SpendGold();
        });
    }
}
