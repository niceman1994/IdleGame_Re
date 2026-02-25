using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    public override void OnUseItem(Player player)
    {
        player.CurrentAtk(itemData.itemAbility);
        TextPoolManager.Instance.ShowItemText("ATK", itemData.itemAbility,
                        player.transform.position, new Color(0, 0, 0, 255), 20);
    }
}
