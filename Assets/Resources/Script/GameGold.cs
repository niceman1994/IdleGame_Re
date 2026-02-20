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

        // 1000골드 이상이면 아스키코드를 이용해 대문자 알파벳(A~Z)을 단위로 지정함
        char unit = (char)(64 + index);
        string p;
        // Math.Truncate : 지정한 소수점이하를 버림, Math.Truncate(a * 100) / 100 : 소수점 둘째자리 이하를 버림
        // 삼항연산자를 이용해 unit이 65이상일 경우 대문자를 표기해 골드를 보여주고 아니라면 현재 가진 골드를 보여줌
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : $"{curGold[0]}";
        goldText.text = p;

        return p;
    }

    public string GetMoneyToString(int[] gold, int index)
    {
        // 현재 골드를 수소점을 포함해 볼 수 있게함
        float a = gold[index];

        // index가 0보다 크면 현재 1000골드이상이니까 100의 자리수까지의 숫자를 1000으로 나눠서 선언한 a에 더해줌
        if (index > 0)
        {
            float b = gold[index - 1];
            a += b / 1000;
        }

        // 1000골드이 상이면 아스키코드를 이용해 대문자 알파벳(A~Z)을 단위로 지정함
        char unit = (char)(64 + index);
        string p;
        // Math.Truncate : 지정한 소수점이하를 버림, Math.Truncate(a * 100) / 100 : 소수점 둘째자리 이하를 버림
        // 삼항연산자를 이용해 unit이 65이상일 경우 대문자를 표기해 골드를 보여주고 아니라면 현재 가진 골드를 보여줌
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : $"{gold[0]}";

        return p;
    }

    public void SpendGold(int[] spendGold, int spendGoldIndex, Action increaseGoldAction)
    {
        if (index == spendGoldIndex)                                 // 소유한 골드 인덱스와 소모할 골드 인덱스가 같을 때
        {
            if (curGold[index] >= spendGold[spendGoldIndex])        // 소유한 골드가 소모할 골드 이상이면 골드를 사용함
            {
                curGold[index] -= spendGold[spendGoldIndex];
                increaseGoldAction?.Invoke();
            }
            else                                                    // 소유 골드가 소모 골드보다 적으면 계산이 안되게 함
                spendGold[spendGoldIndex] += 0;
        }
        else if (index > spendGoldIndex)                            // 소유한 골드 인덱스가 소모할 골드 인덱스보다 클 때
        {
            if (curGold[spendGoldIndex] >= spendGold[spendGoldIndex])        // 소유 골드 인덱스가 크더라도, 소유 골드가 소모 골드보다 많은 소유 골드를 사용함
                curGold[spendGoldIndex] -= spendGold[spendGoldIndex];
            else
            {
                // 같은 인덱스를 비교했을 때 소모 골드가 더 크면 소유 골드에 1000을 더하고 소모 골드를 뺀 다음에 소유 골드에서 1을 뺌
                // 예) 1050 => 1.05A 인데 소모 골드가 150이라면 1050-150=900으로 계산함
                curGold[spendGoldIndex] = curGold[spendGoldIndex] + 1000 - spendGold[spendGoldIndex];
                curGold[spendGoldIndex + 1] -= 1;
            }
            increaseGoldAction?.Invoke();
        }
        else if (index < spendGoldIndex)                            // 소유 골드 인덱스가 소모 골드 인덱스보다 작으면 계산이 안되게 함
            spendGold[spendGoldIndex] += 0;
    }

    public void AddGold(int getGold)
    {
        curGold[0] += getGold;
    }
}
