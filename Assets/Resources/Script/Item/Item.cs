using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemStatSO itemData;
    public ItemStatSO ItemData => itemData;
}
