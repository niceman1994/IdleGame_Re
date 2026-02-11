using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateItemStat")]
public class ItemStatSO : ScriptableObject
{
    public enum ItemType
    {
        Gold, Potion, Weapon, Misc
    }

    public enum AbilityType
    {
        GoldUp, Heal, PowerUp, None
    }

    public string itemName;
    public int itemMaxCount;
    [Multiline]
    public string description;
    public int itemAbility;
    public ItemType itemType;
    public AbilityType itemAbilityType;
    public Sprite itemImage;
}
