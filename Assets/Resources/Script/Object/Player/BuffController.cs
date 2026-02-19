using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private List<Buff> activeBuffs = new List<Buff>();

    private void Update()
    {
        if (activeBuffs.Count > 0)
            BuffUpdate();
    }

    private void BuffUpdate()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].Tick(Time.deltaTime);

            if (!activeBuffs[i].IsActive)
                activeBuffs.RemoveAt(i);
        }
    }

    public void AddBuff(Buff buff)
    {
        activeBuffs.Add(buff);
    }

    // 죽으면 모든 버프를 종료시키도록 정했기 때문에 이런 함수를 사용함
    public void ClearAllBuffs()
    {
        for (int i = 0; i < activeBuffs.Count; i++)
            activeBuffs[i].ExpireBuff();

        activeBuffs.Clear();
    }
}
