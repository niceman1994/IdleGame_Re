using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemExchanger : MonoBehaviour
{
    [SerializeField] bool isOpen = false;
    [SerializeField] RectTransform itemExchangeBox;
    [SerializeField] List<ItemExchangeButton> itemExchangeButtons = new List<ItemExchangeButton>();

    private void Update()
    {
        if (isOpen == true && PointerOverItemExchangeButton() == false)
        {
            if (Input.GetMouseButtonDown(0) ||
                Input.GetMouseButtonDown(1) ||
                Input.GetMouseButtonDown(2) ||
                Input.mouseScrollDelta.y != 0)
                DeactiveItemExchangeUI();
        }

    }

    public void OnClickExchangeButton(Inventory inventory)
    {
        itemExchangeButtons.ForEach(x => x.OnClickExchangeButton(inventory));
    }

    public void ActiveItemExchangeUI(Vector2 boxPosition, int slotItemCount)
    {
        isOpen = true;
        itemExchangeBox.anchoredPosition = boxPosition;
        itemExchangeButtons.ForEach(x =>
        {
            x.gameObject.SetActive(true);
            x.ShowExchangeableItem(slotItemCount);
        });
    }

    public bool PointerOverItemExchangeButton()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;    // 이 코드가 없으면 마우스 클릭과 이벤트가 연결되지 않아서 이벤트 시스템을 이용할 수 없음

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        
        for (int i = 0; i < results.Count; i++)
        {
            var button = results[i].gameObject.GetComponent<ItemExchangeButton>();

            if (results[i].gameObject.name.Contains("Button") && button.IsInteractable() == true)
                return true;
        }

        return false;
    }

    public void DeactiveItemExchangeUI()
    {
        isOpen = false;
        itemExchangeButtons.ForEach(x => x.gameObject.SetActive(false));
    }
}
