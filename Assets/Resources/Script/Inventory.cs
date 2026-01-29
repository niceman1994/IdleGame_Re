using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] RectTransform itemExchangeRectParent;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject horizontalListPrefab;
    [SerializeField] RectTransform addListButtonParent;
    [SerializeField] Button addListButton;
    [SerializeField] ItemExchanger itemExchanger;

    /// <summary>
    /// 획득한 아이템에 대한 변수
    /// </summary>
    private Item item;
    private InventorySlot[] slots;
    private int addListCount = 0;

    private void Start()
    {
        MakeInvetorySlots();
        addListButton.onClick.AddListener(AddHorizontalList);
        // 여기서 OnClickExchangeButton 에 함수를 등록해 아이템을 교환하면서 슬롯을 갱신함
        itemExchanger.OnClickExchangeButton(() => GetItem(itemExchanger.ExchangeItem), UpdateItemSlot);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (itemExchanger.IsOpen == false)
                PointerOverInventorySlot();
            else itemExchanger.DeactivateItemExchangeBox();
        }
    }

    private void PointerOverInventorySlot()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;    // 이 코드가 없으면 마우스 클릭과 이벤트가 연결되지 않아서 이벤트 시스템을 이용할 수 없음

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.name.Equals("Slot"))
            {
                InventorySlot slot = results[i].gameObject.GetComponent<InventorySlot>();
                ShowItemExchangeUI(slot, data.position);
                return;
            }
        }
    }

    public void UpdateItemSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 1. 인벤토리 슬롯에 아이템을 존재하지 않을 때
            // 2. 획득한 아이템의 이미지와 슬롯 아이템 이미지가 같으면서, 아이템의 갯수가 최대 갯수 이하일 때
            if (!slots[i].HaveItem || slots[i].IsSameItem(item))
            {
                // 없는 아이템 추가 또는 소유한 아이템 수 증가
                if (!slots[i].HaveItem)
                    slots[i].AddNewItem(item);
                else
                    slots[i].AddSameItem(1);

                return;
            }
        }
    }

    private void MakeInvetorySlots()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject horzontalList = Instantiate(horizontalListPrefab, content);
            horzontalList.name = "HorizontalList";
            addListButtonParent.SetAsLastSibling();
        }
        StartCoroutine(FindInvetorySlots());
    }

    public void AddHorizontalList()
    {
        addListCount++;

        // Grid Layout Group으로 정리된 content의 자식객체로 horizontalListPrefab을 생성
        GameObject Obj = Instantiate(horizontalListPrefab, content);
        Obj.name = "HorizontalList";

        // 인벤토리 추가버튼이 맨 아래로 가도록 SetAsLastSibling을 사용
        addListButtonParent.SetAsLastSibling();
        StartCoroutine(FindInvetorySlots());

        if (addListCount >= 3)
            addListButtonParent.gameObject.SetActive(false);
    }

    private IEnumerator FindInvetorySlots()
    {
        yield return null;
        slots = content.GetComponentsInChildren<InventorySlot>();
    }

    public void GetItem(Item dropItem)
    {
        item = dropItem;
    }

    private void ShowItemExchangeUI(InventorySlot slot, Vector2 dataPosition)
    {
        if (slot.Icon.sprite.name.Equals("Bottle"))
        {
            itemExchanger.SetItemExchangeBoxPosition(itemExchangeRectParent, dataPosition);
            itemExchanger.ShowExchangeableItem(slot);
        }
    }
}
