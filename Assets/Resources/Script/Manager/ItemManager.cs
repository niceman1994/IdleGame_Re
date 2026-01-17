using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] ItemDrop itemDrop;
    [SerializeField] Transform dropitemParent;

    // 서로 다른 아이템을 드랍하는거라서 큐보단 리스트가 적합
    public List<GameObject> dropItemList = new List<GameObject>();

    private void Start()
    {
        CreateItem();
    }

    private void CreateItem()
    {
        itemDrop.CreateAllItem(dropitemParent, dropItemList);
    }

    public void SpawnItem(Vector3 position)
    {
        itemDrop.SpawnItem(position);
    }
}
