using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGold : MonoBehaviour
{
    [Header("GOLD")]
    [SerializeField] int getGold;
    [SerializeField] Text goldText;

    private BigInteger gold;
    public BigInteger Gold => gold;

   private void Start()
    {
        gold = 700;
        StartCoroutine(EarnGold());
    }

    private void Update()
    {
        FormatGoldToString();
    }

    private IEnumerator EarnGold()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return waitForSeconds;
            gold += getGold;
        }
    }

    public string FormatGoldToString()
    {
        double displayGold = (double)gold;
        int index = 0;

        while (displayGold >= 1000)
        {
            displayGold /= 1000;
            index++;
        }
        string text = "";

        if (index == 0)
            text = $"{gold}";
        else if (index >= 1 && index <= 26)
            text = $"{displayGold:F2}{(char)('A' + index - 1)}";
        else if (index >= 27)
            text = $"{displayGold:F2}{'Z'}";

        goldText.text = text;
        return text;
    }

    public string FormatGoldToString(BigInteger value)
    {
        double displayGold = (double)value;
        int index = 0;

        while (displayGold >= 1000)
        {
            displayGold /= 1000;
            index++;
        }
        string text = "";

        if (index == 0)
            text = $"{value}";
        else if (index >= 1 && index <= 26)
            text = $"{displayGold:F2}{(char)('A' + index - 1)}";
        else if (index >= 27)
            text = $"{displayGold:F2}{'Z'}";

        return text;
    }

    public void SpendGold(BigInteger amount)
    {
        if (gold < amount)
            return;

        gold -= amount;
    }

    public void AddGold(BigInteger getGold)
    {
        gold += getGold;
    }

    public void AddEarnGold(int clickCount)
    {
        getGold += 5 * clickCount;
    }
}
