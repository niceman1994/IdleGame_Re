using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Item
{
    public override void OnUseItem(Player player)
    {
        player.CurrentHpChange(itemData.itemAbility);
    }
}
