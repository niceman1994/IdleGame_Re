using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemStatSO itemData;
    public ItemStatSO ItemData => itemData;

    public abstract void OnUseItem(Player player);

    public bool IsItemNoAbility()
    {
        if (itemData.itemAbilityType == ItemStatSO.AbilityType.None)
            return true;
        else
            return false;
    }
}
