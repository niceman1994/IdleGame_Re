using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHorizontalList : MonoBehaviour
{
    [SerializeField] InventorySlot slotPrefab;
    [SerializeField] int slotCount;
    
    public void AddSlot(List<InventorySlot> slotList, Action<InventorySlot> slotAction)
    {
        for (int i = 0; i < slotCount; ++i)
        {
            InventorySlot slotObj = Instantiate(slotPrefab, transform);
            slotObj.name = "Slot";
            slotList.Add(slotObj);
            slotAction?.Invoke(slotObj);
        }
    }
}
