using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{
    private List<DurationBuff> activeBuffs = new List<DurationBuff>();

    private void Update()
    {
        if (activeBuffs.Count > 0)
        {
            for (int i = activeBuffs.Count - 1; i >= 0; i--)
            {
                activeBuffs[i].Update();

                if (!activeBuffs[i].IsActive)
                    activeBuffs.RemoveAt(i);
            }
        }
    }

    public void AddBuff(DurationBuff durationBuff)
    {
        activeBuffs.Add(durationBuff);
    }
}
