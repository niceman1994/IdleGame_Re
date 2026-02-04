using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class ExchangeButton
{
    public Image itemExchageButtonImage;
    public Button itemExchangeButton;
}

public class ItemExchanger : MonoBehaviour
{
    [SerializeField] bool isOpen = false;
    [SerializeField] int minItemCount;      // 다른 아이템 교환에 필요한 최소 갯수
    [SerializeField] RectTransform itemExchangeBox;
    [SerializeField] List<ExchangeButton> itemExchangeButtons = new List<ExchangeButton>();

    /// <summary>
    /// 매개변수 없이 itemExchangeButton 에 이벤트를 등록하기 위해서 캐싱하는 인벤토리 슬롯 변수<para/>
    /// <see cref="Inventory.Start"/> 에서 OnClickExchangeButton 함수를 호출함
    /// </summary>
    private InventorySlot cachedSlot;

    /// <summary>
    /// 교환할 아이템 정보를 <see cref="Inventory"/> 로 넘기기 위해 캐싱하는 아이템 변수
    /// </summary>
    private Item item;

    public Item ExchangeItem => item;
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

    public void ShowExchangeableItem(InventorySlot slot)
    {
        cachedSlot = slot;
        
        for (int i = 0; i < itemExchangeButtons.Count; i++)
            SetButtonInteractable(i, cachedSlot.ItemCount >= (i + 1) * minItemCount);
    }

    public void OnClickExchangeButton(UnityAction getItemEvent, UnityAction itemExchangeEvent)
    {
        for (int i = 0; i < itemExchangeButtons.Count; i++)
        {
            // "https://mentum.tistory.com/343" 링크를 참조할 것
            // for문에서 람다식으로 버튼 이벤트를 등록하면 반복문의 마지막 변수 값만 참조되어 Closure Problem 이 발생해 값을 복사해서 사용해야함
            int temp = i;
            itemExchangeButtons[i].itemExchangeButton.onClick.AddListener(() => ItemExchange(temp));
            itemExchangeButtons[i].itemExchangeButton.onClick.AddListener(getItemEvent);
            itemExchangeButtons[i].itemExchangeButton.onClick.AddListener(itemExchangeEvent);
        }
    }

    private void ItemExchange(int buttonIndex)
    {
        if (buttonIndex == 0)
            item = ItemManager.Instance.CreateItem("Gold");
        else if (buttonIndex == 1)
            item = ItemManager.Instance.CreateItem("HpPotion");
        else if (buttonIndex == 2)
            item = ItemManager.Instance.CreateItem("Sword");
        
        cachedSlot.SubtractItemCount((buttonIndex + 1) * minItemCount);
        itemExchangeBox.gameObject.SetActive(false);
    }

    private void SetButtonInteractable(int buttonIndex, bool interactable)
    {
        itemExchangeButtons[buttonIndex].itemExchangeButton.interactable = interactable;

        if (interactable == true)
            itemExchangeButtons[buttonIndex].itemExchageButtonImage.color = Color.white;
        else
        {
            itemExchangeButtons[buttonIndex].itemExchageButtonImage.color =
                  new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 128.0f / 255.0f);
        }
    }

    public bool PointerOverItemExchangeButton()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;    // 이 코드가 없으면 마우스 클릭과 이벤트가 연결되지 않아서 이벤트 시스템을 이용할 수 없음

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        for (int i = 0; i < results.Count; i++)
        {
            var button = results[i].gameObject.GetComponent<Button>();

            if (results[i].gameObject.name.Contains("Button") && button.IsInteractable() == true)
                return true;
        }

        return false;
    }
}
