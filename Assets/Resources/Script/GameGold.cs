using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGold : MonoBehaviour
{
    [Header("GOLD")]
    public int[] curGold;
    public int getGold;
    public Text goldText;
    public int index;

   private void Start()
    {
        StartCoroutine(EarnGold());
    }

    private void Update()
    {
        GoldTheorem();
        GetMoneyToString();
    }

    private IEnumerator EarnGold()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return waitForSeconds;

            if (curGold[26] >= 999)
                curGold[26] = 999;
            else
                curGold[0] += getGold;
        }
    }

    private void GoldTheorem()
    {
        // 총 27개이며, 1000골드를 넘으면 A~Z까지 표기하고 아니라면 알파벳을 표기하지 않기때문에 알파벳 26개와 알파벳이 없는 골드까지 포함했음
        for (int i = 0; i < 27; ++i)
        {
            if (curGold[i] > 0)
                index = i;
        }

        for (int i = 0; i <= index; ++i)
        {
            if (index < 26)
            {
                if (curGold[i] > 1000)
                {
                    curGold[i] -= 1000;
                    curGold[i + 1] += 1;
                }

                // i번째 배열값이 음수일 경우다음 배열값에서 1을 빼고 i번째 배열에 1000을 더함
                if (curGold[i] < 0)
                {
                    if (index > i)
                    {
                        index = i;
                        curGold[i + 1] -= 1;
                        curGold[i] += 1000;
                    }
                }
            }
        }
    }

    public void GoldTheorem(int[] gold, int index)
    {
        for (int i = 0; i < 27; ++i)
        {
            if (gold[i] > 0)
                index = i;
        }

        for (int i = 0; i <= index; ++i)
        {
            if (index < 26)
            {
                if (gold[i] > 1000)
                {
                    gold[i] -= 1000;
                    gold[i + 1] += 1;
                }
            }
        }
    }

    private string GetMoneyToString()
    {
        // 현재 골드를 수소점을 포함해서 볼 수 있게함
        float a = curGold[index];

        // index가 0보다 크면 현재 1000골드를 넘었으니 100의 자리수까지의 숫자를 1000으로 나누고 float a에 더해줌
        if (index > 0)
        {
            float b = curGold[index - 1];
            a += b / 1000;
        }

        // index가 0이라면 1000골드를 넘지 않았기때문에 현재 골드에 보여줄 소수점이 없으므로 0을 더함
        if (index == 0)
            a += 0;

        // char형 변수를 선언하고 1000골드이상일 때 index가 1이상이므로 64 + index를 더하면 대문자 알파벳(A~Z)가 가진 숫자에 해당함
        char unit = (char)(64 + index);
        string p;
        // Math.Truncate : 지정한 소수점이하를 버림, Math.Truncate(a * 100) / 100 : 소수점 둘째자리 이하를 버림
        // 삼항연산자를 이용해 unit이 65이상일 경우 대문자를 표기해 골드를 보여주고 아니라면 현재 가진 골드를 보여줌
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : curGold[0].ToString();
        goldText.text = p;

        return p;
    }

    public string GetMoneyToString(int[] gold, int index, Text text)
    {
        // 현재 골드를 수소점을 포함해 볼 수 있게함
        float a = gold[index];

        // index가 0보다 크면 현재 1000골드이상이니까 100의 자리수까지의 숫자를 1000으로 나눠서 선언한 a에 더해줌
        if (index > 0)
        {
            float b = gold[index - 1];
            a += b / 1000;
        }

        // index가 0이라면 1000골드를 넘지 않았기때문에 현재 골드에 보여줄 소수점이 없으므로 0을 더함
        if (index == 0)
            a += 0;

        // char형 변수를 선언하고 1000골드이상일 때 index가 1이상이므로 64 + index를 더하면 대문자 알파벳(A~Z)가 가진 숫자에 해당함
        char unit = (char)(64 + index);
        string p;
        // Math.Truncate : 지정한 소수점이하를 버림, Math.Truncate(a * 100) / 100 : 소수점 둘째자리 이하를 버림
        // 삼항연산자를 이용해 unit이 65이상일 경우 대문자를 표기해 골드를 보여주고 아니라면 현재 가진 골드를 보여줌
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : gold[0].ToString();
        text.text = p;

        return p;
    }
}
