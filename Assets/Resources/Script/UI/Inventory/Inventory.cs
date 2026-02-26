using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] RectTransform itemExchangeRectParent;
    [SerializeField] RectTransform content;
    [SerializeField] InventoryHorizontalList horizontalListPrefab;
    [SerializeField] RectTransform addListButtonParent;
    [SerializeField] Button addListButton;
    [SerializeField] ItemExchanger itemExchanger;

    // 획득한 아이템에 대한 변수
    private Item item;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventorySlot currentSlot;
    private int addListCount = 0;

    private void Start()
    {
        for (int i = 0; i < 2; i++)
            MakeInvetorySlots();

        itemExchanger.OnClickExchangeButton(this);
        addListButton.onClick.AddListener(AddHorizontalList);
    }

    private void MakeInvetorySlots()
    {
        InventoryHorizontalList horzontalList = Instantiate(horizontalListPrefab, content);
        horzontalList.name = "HorizontalList";
        horzontalList.AddSlot(slots, AddSlotEvent);

        addListButtonParent.SetAsLastSibling();
    }

    public void AddHorizontalList()
    {
        addListCount++;
        MakeInvetorySlots();

        if (addListCount >= 3)
            addListButtonParent.gameObject.SetActive(false);
    }

    private void AddSlotEvent(InventorySlot slot)
    {
        // 모든 슬롯에 이벤트를 등록함
        slot.onActiveItemExchanger += SetItemExchangeUIPosition;
        slot.onUseItem += OnUseItem;
    }

    public void SetItemExchangeUIPosition(InventorySlot slot, Vector2 setBoxPosition, int slotItemCount)
    {
        currentSlot = slot;

        Vector2 boxPosition;
        // https://bonnate.tistory.com/219 링크를 참고해서 슬롯 위에 있는 마우스 위치에 맞게 아이템교환 UI가 뜨도록 함
        RectTransformUtility.ScreenPointToLocalPointInRectangle(itemExchangeRectParent, setBoxPosition, null, out boxPosition);
        itemExchanger.ActiveItemExchangeUI(boxPosition, slotItemCount);
    }

    public void GetItem(Item dropItem)
    {
        item = dropItem;
        UpdateItemSlot();
    }

    private void UpdateItemSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            // 1. 인벤토리 슬롯에 아이템이 존재하지 않을 때
            // 2. 획득한 아이템의 이미지와 슬롯의 아이템 이미지가 같으면서, 해당 아이템의 갯수가 최대 갯수 이하일 때
            if (!slots[i].HaveItem || slots[i].IsSameItem(item))
            {
                // 없는 아이템 추가 또는 소유한 아이템 수 증가
                if (!slots[i].HaveItem)
                    slots[i].AddItem(item);
                else
                    slots[i].AddSameItem(1);

                return;
            }
        }
    }

    // 아이템 교환 버튼에 등록하기 위해 사용하는 함수
    public void TryExchangeItem(string exchangeItemName, int requiredItemCount)
    {
        if (currentSlot == null) return;

        currentSlot.SubtractItemCount(requiredItemCount);
        GetItem(ItemManager.Instance.CreateItem(exchangeItemName));
        itemExchanger.DeactiveItemExchangeUI();
    }

    public void OnUseItem(InventorySlot slot, Item useItem)
    {
        if (useItem != null)
        {
            if (useItem.IsItemNoAbility())                   // 아이템 능력이 없다면 사용할 수 없게함
                slot.AddSameItem(0);
            else
            {
                useItem.OnUseItem(player);
                slot.AddSameItem(-1);
            }
        }
    }
}
