using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrStrObj : SerializableDictionary<string, StrObj> { }
[System.Serializable]
public class StrObj : SerializableDictionary<string, GameObject> { }

// 이 스크립트를 오브젝트에 붙여서 아이템을 생성하는게 아니라 아이템 매니저에서 만들어놓은 아이템을 오브젝트가 죽었을 때 아이템이 옆에 위치하도록 수정함
public class ItemDrop : MonoBehaviour
{
    [SerializeField] StrStrObj items;
    private int currentItemPercent;

    public void CreateAllItem(Transform parent, List<Item> itemList)
    {
        for (int i = 0; i < items["Item"].Values.Count; i++)
        {
            GameObject item = Instantiate(items["Item"].Values.ElementAt(i), Vector3.zero, Quaternion.identity, parent);
            item.name = items["Item"].Values.ElementAt(i).name;
            item.SetActive(false);
            itemList.Add(item.GetComponent<Item>());
        }
    }

    public void SpawnItem(Vector3 spawnPosition)
    {
        currentItemPercent = Random.Range(1, 101);

        // ElementAt() : 지정된 시퀀스의 인덱스 요소를 반환
        if (currentItemPercent <= 100 && currentItemPercent > 0)
             MakeItem(spawnPosition, ItemManager.Instance.dropItemList[0]);                // ElementAt(0) : 잡템인 병
        //else if (currentItemPercent <= 65 && currentItemPercent > 40)
        //     MakeItem(spawnPosition, ItemManager.Instance.dropItemList[1]);                // ElementAt(1) : 회복 포션
        //else if (currentItemPercent <= 30 && currentItemPercent > 10)
        //     MakeItem(spawnPosition, ItemManager.Instance.dropItemList[2]);                // ElementAt(2) : 골드
        //else if (currentItemPercent <= 10 && currentItemPercent > 0)
        //     MakeItem(spawnPosition, ItemManager.Instance.dropItemList[3]);                // ElementAt(3) : 공격력을 증가시킬 검
    }

    public void SpawnItem(float _x, float _y, float _z)
    {
        currentItemPercent = Random.Range(1, 101);

        if (currentItemPercent <= 100 && currentItemPercent > 70)
            MakeItem(_x, _y, _z, ItemManager.Instance.dropItemList[0]);          // ElementAt(0) : 잡템인 병
        else if (currentItemPercent <= 65 && currentItemPercent > 40)
            MakeItem(_x, _y, _z, ItemManager.Instance.dropItemList[1]);          // ElementAt(1) : 회복 포션
        else if (currentItemPercent <= 30 && currentItemPercent > 10)
            MakeItem(_x, _y, _z, ItemManager.Instance.dropItemList[2]);          // ElementAt(2) : 골드
        else if (currentItemPercent <= 10 && currentItemPercent > 0)
            MakeItem(_x, _y, _z, ItemManager.Instance.dropItemList[3]);          // ElementAt(3) : 공격력을 증가시킬 검
    }

    private void MakeItem(Vector3 pos, Item item)
    {
        item.gameObject.SetActive(true);
        item.transform.position = new Vector3(pos.x + 1.4f, pos.y - 1.25f, pos.z);
    }

    private void MakeItem(float _x, float _y, float _z, Item item)
    {
        item.gameObject.SetActive(true);
        item.transform.position = new Vector3(_x + 1.4f, _y - 1.25f, _z);
    }
}
