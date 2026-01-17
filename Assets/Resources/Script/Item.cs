using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] int maxCount;
    [Multiline]
    [SerializeField] string description;
    [SerializeField] ItemType itemType;
    [SerializeField] AbilityType itemAbilityType;
    [SerializeField] int itemAbility;
    [SerializeField] Sprite itemImage;

    public enum ItemType
    {
        Gold, Potion, Weapon, Misc
    }

    public enum AbilityType
    {
        GoldUp, Heal, PowerUp, None
    }

    public int MaxCount { get { return maxCount; } }
    public int ItemAbility { get { return itemAbility; } }
    public ItemType GetItemType { get { return itemType; } }
    public AbilityType ItemAbilityType { get { return itemAbilityType; } }
    public Sprite ItemImage { get { return itemImage; } }
}
