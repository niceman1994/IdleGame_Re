using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Item item;
    [SerializeField] int itemCount;
    [SerializeField] Image slotIcon;
    [SerializeField] Button slotItemClick;
    [SerializeField] Text itemCountText;
    [SerializeField] ScrollRect scrollParent;

    public bool haveItem;

    public int ItemCount { get { return itemCount; } }
    public Image Icon { get { return slotIcon; } }

    private void Start()
    {
        haveItem = false;
        slotItemClick.onClick.AddListener(UseItem);
        StartCoroutine(SearchScrollRect());
    }

    private IEnumerator SearchScrollRect()
    {
        yield return null;
        scrollParent = transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void AddItem(Item newItem, int count = 1)
    {
        haveItem = true;
        item = newItem;
        itemCount = count;
        slotIcon.sprite = item.ItemImage;
        itemCountText.text = item.GetItemType != Item.ItemType.Weapon ? itemCount.ToString() : "";
        SetColor(1);
    }

    private void ClearSlot()
    {
        haveItem = false;
        item = null;
        itemCount = 0;
        slotIcon.sprite = null;
        itemCountText.text = "";
        SetColor(0);
    }

    private void SetColor(float slotIconAlpha)
    {
        Color color = slotIcon.color;
        color.a = slotIconAlpha;
        slotIcon.color = color;
    }

    public void SetItemCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    public void UseItem()
    {
        if (item != null)
        {
            if (item.ItemAbilityType == Item.AbilityType.None)  // 아이템 능력이 아무것도 없다면 사용되지 않게한다
                itemCount -= 0;
            else
            {
                if (item.ItemAbilityType == Item.AbilityType.GoldUp)    // 아이템 능력이 골드 증가일 때
                {
                    GameManager.Instance.gameGold.curGold[0] += item.ItemAbility;
                    ObjectPoolManager.Instance.ShowItemText("Gold", item.ItemAbility,
                        GameManager.Instance.player.transform.position, new Color(255, 200, 0, 255), 13);
                }
                else if (item.ItemAbilityType == Item.AbilityType.Heal) // 아이템 능력이 체력 회복일 때
                    GameManager.Instance.player.CurrentHp(item.ItemAbility);
                else if (item.ItemAbilityType == Item.AbilityType.PowerUp)  // 아이템 능력이 공격력 증가일 때
                {
                    GameManager.Instance.player.CurrentAtk(item.ItemAbility);
                    ObjectPoolManager.Instance.ShowItemText("ATK", item.ItemAbility,
                        GameManager.Instance.player.transform.position, new Color(0, 0, 0, 255), 15);
                }
                SetItemCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;                          // 이 스크립트가 들어간 객체를 dragSlot에 넣는다
            DragSlot.instance.isDrag = true;
            DragSlot.instance.DragSetImage(slotIcon);                       // 드래그한 객체의 이미지가 보이게 수정
            DragSlot.instance.transform.position = eventData.position;
        }
        else if (!DragSlot.instance.isDrag)     // 아이템이 없어서 드래그 중인게 아닐때
            scrollParent.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
        else if (!DragSlot.instance.isDrag)
            scrollParent.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);      // 드래그가 끝난 dragslot을 보이지 않게 수정
        DragSlot.instance.dragSlot = null;  // dragSlot을 비운다

        if (!DragSlot.instance.isDrag)
            scrollParent.OnEndDrag(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot != this)
            SwapSlot();
    }

    private void SwapSlot()
    {
        Item tempItem = item;           // 아이템이 이미 있는 슬롯으로 옮길때 기존에 있는 아이템을 임시로 저장
        int tempItemCount = itemCount;  // 마찬가지로 기존에 있는 아이템의 갯수를 임시로 저장

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)   // 바꾸려는 슬롯에 아이템이 존재할 경우
        {
            if (tempItem.name != DragSlot.instance.dragSlot.item.name)      // 기존 슬롯의 아이템 이름이 드래그한 슬롯의 아이템 이름과 같지 않다면
                DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
            else    // 기존 슬롯의 아이템 이름이 드래그한 슬롯의 아이템 이름과 같다면
            {
                int totalItemCount = tempItemCount + DragSlot.instance.dragSlot.itemCount;

                if (totalItemCount <= item.MaxCount)    // 합산된 아이템 갯수가 최대 갯수 이하일 경우
                {
                    AddItem(DragSlot.instance.dragSlot.item, tempItemCount + DragSlot.instance.dragSlot.itemCount);
                    DragSlot.instance.dragSlot.ClearSlot();
                }
                else    // 합산된 아이템 갯수가 최대 갯수를 넘을 경우
                {
                    // 현재 슬롯을 최대 갯수로 채우고 나머지는 이전 슬롯에 추가
                    AddItem(DragSlot.instance.dragSlot.item, item.MaxCount);
                    DragSlot.instance.dragSlot.AddItem(DragSlot.instance.dragSlot.item, totalItemCount - item.MaxCount);
                }
            }
        }
        else    // 바꾸려는 슬롯에 아이템이 없을 경우
            DragSlot.instance.dragSlot.ClearSlot();

        DragSlot.instance.isDrag = false;   // 드래그가 끝났으니 bool 값을 false로 되돌린다.
    }

    public void SetItemExchangePosition()
    {
        if (slotIcon.name.Equals("Bottle") && Input.GetMouseButton(1))
        {

        }
    }
}