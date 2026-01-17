using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemExchanger : MonoBehaviour
{
    [SerializeField] RectTransform itemExchangeBox;

    public void SetItemExchangeBoxPosition(Vector3 boxPosition)
    {
        itemExchangeBox.gameObject.SetActive(true);
        itemExchangeBox.anchoredPosition = boxPosition;
    }

    public void DeactivateItemExchangeBox()
    {
        itemExchangeBox.gameObject.SetActive(false);
    }
}
