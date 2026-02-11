using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "캐릭터 스탯 생성", menuName = "CreateCharacterStat")]
public class PlayerStatSO : ScriptableObject
{
    public ObjectStats playerStats;
    public float baseMoveSpeed;
}
