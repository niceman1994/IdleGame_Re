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

    private InventorySlot[] slots;
    private Item item;
    private int addListCount = 0;

    private void Start()
    {
        MakeInvetorySlots();
        addListButton.onClick.AddListener(AddHorizontalList);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (itemExchanger.IsOpen == false)
                PointerOverThisSlot();
            else itemExchanger.DeactivateItemExchangeBox();
        }
    }

    private void PointerOverThisSlot()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.name.Equals("Slot"))
            {
                InventorySlot slot = results[i].gameObject.GetComponent<InventorySlot>();
                ItemExchange(slot, data.position);
                return;
            }
        }
    }

    public void SlotItemsUI()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            // 1. 인벤토리 슬롯에 아이템 이미지가 없을 때
            // 2. 획득한 아이템의 이미지와 슬롯 아이템 이미지가 같으면서, 아이템의 갯수가 최대 갯수 이하일 때
            if (!slots[i].haveItem || CheckSameItem(slots[i]))
            {
                // 아이템 추가 또는 갯수 증가
                if (!slots[i].haveItem)
                    slots[i].AddItem(item);
                else
                    slots[i].SetItemCount(1);

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

        // Grid Layout Group으로 정리된 content의 자식객체로 horizontalListPrefab을 생성한다.
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

    private bool CheckSameItem(InventorySlot slot)
    {
        return item.ItemImage == slot.Icon.sprite && slot.ItemCount < item.MaxCount;
    }

    public void GetItem(Item dropItem)
    {
        item = dropItem;
    }

    private void ItemExchange(InventorySlot slot, Vector2 dataPosition)
    {
        Debug.Log($"클릭한 스프라이트 이름 : {slot.Icon.sprite.name}");
        if (slot.Icon.sprite.name.Equals("Bottle"))
        {
            itemExchanger.SetItemExchangeBoxPosition(itemExchangeRectParent, dataPosition);
            itemExchanger.ShowExchangeableItem(slot.ItemCount);
        }
    }
}
