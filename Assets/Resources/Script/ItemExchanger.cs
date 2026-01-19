using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ExchangeButton
{
    public Image itemExchageButtonImage;
    public Button itemExchangeButton;
}

public class ItemExchanger : MonoBehaviour
{
    [SerializeField] bool isOpen = false;
    [SerializeField] RectTransform itemExchangeBox;
    [SerializeField] List<ExchangeButton> itemExchangeButtons = new List<ExchangeButton>();

    public bool IsOpen => isOpen;

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
        if (itemCount >= 5 && itemCount < 10)           // 골드
        {
            itemExchangeButtons[0].itemExchangeButton.interactable = true;
            itemExchangeButtons[0].itemExchageButtonImage.color = Color.white;
        }
        else if (itemCount >= 10 && itemCount < 15)     // 체력 포션
        {
            itemExchangeButtons[1].itemExchangeButton.interactable = true;
            itemExchangeButtons[1].itemExchageButtonImage.color = Color.white;
        }
        else if (itemCount >= 15)                       // 검
        {
            itemExchangeButtons[2].itemExchangeButton.interactable = true;
            itemExchangeButtons[2].itemExchageButtonImage.color = Color.white;
        }
    }

    public void ClickExchangeButton()
    {
        for (int i = 0; i < itemExchangeButtons.Count; i++)
        {
            if (itemExchangeButtons[i].itemExchangeButton.interactable == true)
            {
                
            }
        }
    }
}
