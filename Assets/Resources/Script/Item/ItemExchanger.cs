using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class ExchangeButton
{
    public Image itemExchageButtonImage;
    public Button itemExchangeButton;
    public Text itemExchangeText;
}

public class ItemExchanger : MonoBehaviour
{
    [SerializeField] bool isOpen = false;
    [SerializeField] RectTransform itemExchangeBox;
    [SerializeField] List<ExchangeButton> itemExchangeButtons = new List<ExchangeButton>();

    public bool IsOpen => isOpen;
    public Action<InventorySlot> itemExchangeEvent;

    private void Start()
    {
        itemExchangeEvent += OnClickExchangeButton;
    }

    public void SetItemExchangeBoxPosition(RectTransform boxParent, Vector2 setBoxPosition)
    {
        isOpen = true;
        Vector2 boxPosition;
        // https://bonnate.tistory.com/219 링크를 참조해 슬롯 위에 있는 마우스 위치에 맞게 아이템교환 UI가 뜨도록 함
        RectTransformUtility.ScreenPointToLocalPointInRectangle(boxParent, setBoxPosition, null, out boxPosition);
        itemExchangeBox.gameObject.SetActive(true);
        itemExchangeBox.anchoredPosition = boxPosition;
    }

    public void DeactivateItemExchangeBox()
    {
        isOpen = false;
        itemExchangeBox.gameObject.SetActive(false);
    }

    public void ShowExchangeableItem(int itemCount)
    {
        for (int i = 0; i < itemExchangeButtons.Count; i++)
        {
            if (itemCount < 4)
                SetButtonInteractable(i, false, new Color(1, 1, 1, 0.5f));
            else if (itemCount >= 4 && itemCount < 8)
                SetButtonInteractable(i, true, Color.white);
            else if (itemCount >= 8 && itemCount < 12)
                SetButtonInteractable(i, true, Color.white);
            else if (itemCount >= 12)
                SetButtonInteractable(i, true, Color.white);
        }
    }

    private void ItemExchange(InventorySlot slot)
    {
        for (int i = 0; i < itemExchangeButtons.Count; i++)
        {
            if (slot.ItemCount >= 4 && slot.ItemCount < 8)
            {
                slot.ItemExchange((i + 1) * 4);
                slot.AddItem(ItemManager.Instance.CreateItem("Gold"));
                return;
            }
            else if (slot.ItemCount >= 8 && slot.ItemCount < 12)
            {
                slot.ItemExchange((i + 1) * 4);
                slot.AddItem(ItemManager.Instance.CreateItem("HpPotion"));
                return;
            }
            else if (slot.ItemCount >= 12)
            {
                slot.ItemExchange((i + 1) * 4);
                slot.AddItem(ItemManager.Instance.CreateItem("Sword"));
                return;
            }
        }
        itemExchangeBox.gameObject.SetActive(false);
    }

    private void OnClickExchangeButton(InventorySlot slot)
    {
        for (int i = 0; i < itemExchangeButtons.Count; i++)
            itemExchangeButtons[i].itemExchangeButton.onClick.AddListener(() => ItemExchange(slot));
    }

    private void SetButtonInteractable(int buttonIndex, bool interactable, Color color)
    {
        itemExchangeButtons[buttonIndex].itemExchangeButton.interactable = interactable;
        itemExchangeButtons[buttonIndex].itemExchageButtonImage.color = color;
    }

    public bool CheckDeactiveButton()
    {
        // 병 아이템의 수가 모자라 버튼에 등록된 이벤트를 호출시킬 수 없을 때
        if (itemExchangeButtons.All(x => x.itemExchangeButton.interactable == false))
            return false;

        else
            return true;
    }
}
