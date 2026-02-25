using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemExchangeButton : MonoBehaviour
{
    [SerializeField] ItemStatSO itemData;
    [SerializeField] Image itemButtonImage;
    [SerializeField] Button itemButton;
    [SerializeField] int requireItemCount;      // 다른 아이템 교환에 필요한 최소 갯수

    public void ShowExchangeableItem(int slotItemCount)
    {
        SetButtonInteractable(slotItemCount >= requireItemCount);
    }

    private void SetButtonInteractable(bool interactable)
    {
        itemButton.interactable = interactable;
    }

    public void OnClickExchangeButton(Inventory inventory)
    {
        // 아이템 버튼에 교환 이벤트 등록
        itemButton.onClick.AddListener(() => inventory.TryExchangeItem(itemData.itemName, requireItemCount));
    }

    public bool IsInteractable()
    {
        return itemButton.interactable;
    }
}
