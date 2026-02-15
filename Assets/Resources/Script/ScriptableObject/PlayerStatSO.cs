using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaterStat", menuName = "CreateSO/CreateCharacterStat")]
public class PlayerStatSO : ScriptableObject
{
    public ObjectStats playerStats;
    public float baseMoveSpeed;
}
