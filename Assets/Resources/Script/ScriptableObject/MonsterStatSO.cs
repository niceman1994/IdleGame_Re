using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "몬스터 스탯 생성", menuName = "CreateMonsterStat")]
public class MonsterStatSO : ScriptableObject
{
    public int giveGold;
    public ObjectStats monsterStats;
}
