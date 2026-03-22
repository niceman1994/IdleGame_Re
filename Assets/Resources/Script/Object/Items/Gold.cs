using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
{
    public override void OnUseItem(Player player)
    {
        GameManager.Instance.gameGold.AddGold(itemData.itemAbility);
        TextPoolManager.Instance.ShowItemAbilityText("Gold", itemData.itemAbility,
                        player.transform.position, new Color(255, 200, 0, 255), 16);
    }
}
