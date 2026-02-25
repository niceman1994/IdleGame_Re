using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Item item;
    [SerializeField] int itemCount;
    [SerializeField] Image slotIcon;
    [SerializeField] Button slotItemClick;
    [SerializeField] Text itemCountText;
    /// <summary>
    /// <see cref="https://higatsuryu9975.tistory.com/10"/> 링크 참조<para/>
    /// 스크롤뷰와 버튼때문에 생기는 버그를 해결하기 위해 사용한 변수
    /// </summary>
    [SerializeField] ScrollRect scrollParent;

    private bool haveItem;

    public bool HaveItem => haveItem;

    public event Action<InventorySlot, Vector2, int> onActiveItemExchanger;
    public event Action<InventorySlot, Item> onUseItem;

    private void Start()
    {
        haveItem = false;
        slotItemClick.onClick.AddListener(UseItem);
        StartCoroutine(SearchScrollRect());
    }

    public IEnumerator SearchScrollRect()
    {
        yield return null;
        scrollParent = transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void AddItem(Item newItem, int count = 1)
    {
        haveItem = true;
        item = newItem;
        itemCount = count;
        slotIcon.sprite = item.ItemData.itemImage;
        slotIcon.raycastTarget = item.ItemData.itemAbilityType == ItemStatSO.AbilityType.None ? false : true;
        itemCountText.text = $"{itemCount}";
        SetColor(1);
    }

    private void ClearSlot()
    {
        haveItem = false;
        item = null;
        itemCount = 0;
        slotIcon.sprite = null;
        slotIcon.raycastTarget = true;
        itemCountText.text = string.Empty;
        SetColor(0);
    }

    private void SetColor(float slotIconAlpha)
    {
        Color color = slotIcon.color;
        color.a = slotIconAlpha;
        slotIcon.color = color;
    }

    public void AddSameItem(int count)
    {
        itemCount += count;
        itemCountText.text = $"{itemCount}";

        if (itemCount <= 0)
            ClearSlot();
    }

    public bool IsSameItem(Item addItem)
    {
        return addItem.ItemData.itemImage == slotIcon.sprite && itemCount < item.ItemData.itemMaxCount;
    }

    public void SubtractItemCount(int requiredItemCount)
    {
        itemCount -= requiredItemCount;
        itemCountText.text = $"{itemCount}";

        if (itemCount <= 0)
            ClearSlot();
    }

    /// <summary>
    /// 아이템 슬롯이 추가되면서 <see cref="Inventory.AddEventToSlot"/> 함수에 이벤트로 추가할 때 이미 생성된 슬롯의 중복 등록을 방지하기 위한 함수
    /// </summary>
    public void ClearItemEvent()
    {
        onUseItem = null;
        onActiveItemExchanger = null;
    }

    private void UseItem()
    {
        onUseItem?.Invoke(this, item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onActiveItemExchanger.Invoke(this, eventData.pressPosition, itemCount);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)          // 좌클릭이 아니면 드래그가 아닌 것으로 취급함
            return;
        else
        {
            if (item != null)
            {
                DragSlot.instance.dragSlot = this;                          // 이 스크립트가 들어간 객체를 dragSlot에 넣음
                DragSlot.instance.isDrag = true;
                DragSlot.instance.DragSetImage(slotIcon);                   // 드래그한 객체의 이미지가 보이게 수정
                DragSlot.instance.transform.position = eventData.position;
            }
            else if (!DragSlot.instance.isDrag)                             // 아이템이 없어서 드래그 중인게 아닐때
                scrollParent.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)          // 좌클릭이 아니면 드래그가 아닌 것으로 취급함
            return;
        else
        {
            if (item != null)
                DragSlot.instance.transform.position = eventData.position;
            else if (!DragSlot.instance.isDrag)
                scrollParent.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)          // 좌클릭이 아니면 드래그가 아닌 것으로 취급함
            return;
        else
        {
            DragSlot.instance.SetColor(0);                                  // 드래그가 끝난 dragslot을 보이지 않게 수정
            DragSlot.instance.dragSlot = null;                              // dragSlot을 비움

            if (!DragSlot.instance.isDrag)
                scrollParent.OnEndDrag(eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)          // 좌클릭이 아니면 드래그가 아닌 것으로 취급함
            return;
        else
        {
            if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot != this)
                SwapSlot();
        }
    }

    private void SwapSlot()
    {
        Item tempItem = item;           // 슬롯에 있는 아이템을 다른 슬롯으로 옮길 때 클릭한 아이템을 임시로 저장
        int tempItemCount = itemCount;  // 옮기기위해 클릭한 아이템의 갯수를 또한 임시로 저장

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)   // 옮길 슬롯에 아이템이 있을 경우
        {
            if (tempItem.name != DragSlot.instance.dragSlot.item.name)              // 해당 슬롯의 아이템 이름이 드래그한 아이템 이름과 다르다면
                DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
            else                                                                    // 해당 슬롯의 아이템 이름이 드래그한 아이템 이름과 같다면
            {
                int totalItemCount = tempItemCount + DragSlot.instance.dragSlot.itemCount;

                if (totalItemCount <= item.ItemData.itemMaxCount)    // 합산된 아이템 갯수가 최대 갯수 이하일 경우
                {
                    AddItem(DragSlot.instance.dragSlot.item, tempItemCount + DragSlot.instance.dragSlot.itemCount);
                    DragSlot.instance.dragSlot.ClearSlot();
                }
                else                                                 // 합산된 아이템 갯수가 최대 갯수를 넘을 경우
                {
                    // 현재 슬롯의 아이템 수를 최대치로 채우고 나머지는 다른 슬롯에 추가
                    AddItem(DragSlot.instance.dragSlot.item, item.ItemData.itemMaxCount);
                    DragSlot.instance.dragSlot.AddItem(DragSlot.instance.dragSlot.item, totalItemCount - item.ItemData.itemMaxCount);
                }
            }
        }
        else                    // 옮길 슬롯에 아이템이 없을 경우
            DragSlot.instance.dragSlot.ClearSlot();

        DragSlot.instance.isDrag = false;   // 드래그가 끝났으니 bool 값을 false로 되돌림
    }
}