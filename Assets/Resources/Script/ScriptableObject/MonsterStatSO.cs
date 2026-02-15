using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStat", menuName = "CreateSO/CreateMonsterStat")]
public class MonsterStatSO : ScriptableObject
{
    public int giveGold;
    public ObjectStats monsterStats;
}
