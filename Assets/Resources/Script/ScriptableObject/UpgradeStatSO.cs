using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeStat", menuName = "CreateSO/UpgradeStat")]
public class UpgradeStatSO : ScriptableObject
{
    public UpgradeType upgradeType;
    public string baseCostString;
    public int maxClickCount;

    // 문자열을 골드로 표기하기 위해 BigIntger 타입으로 변환
    public BigInteger BaseCost => BigInteger.Parse(baseCostString);
}
