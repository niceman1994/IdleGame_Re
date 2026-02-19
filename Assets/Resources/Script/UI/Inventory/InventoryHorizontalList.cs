using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHorizontalList : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount;

    private void Start()
    {
        AddSlot();
    }

    // 슬롯이 추가될 경우 slotCount의 수만큼 반복문을 돌려 아이템 슬롯을 추가
    private void AddSlot()
    {
        for (int i = 0; i < slotCount; ++i)
        {
            GameObject Obj = Instantiate(slotPrefab, transform);
            Obj.name = "Slot";
        }
    }
}
