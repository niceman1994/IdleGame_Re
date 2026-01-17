using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    [SerializeField] Image imageItem;

    public bool isDrag;
    public InventorySlot dragSlot;

    private void Start()
    {
        instance = this;
        isDrag = false;
    }

    public void DragSetImage(Image dragItemImage)
    {
        imageItem.sprite = dragItemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float itemImageAlpha)
    {
        Color color = imageItem.color;
        color.a = itemImageAlpha;
        imageItem.color = color;
    }
}
