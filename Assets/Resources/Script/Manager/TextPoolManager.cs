using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPoolManager : Singleton<TextPoolManager>
{
    [SerializeField] int defaultCapity = 5;
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] Transform damageTextParent;
    [SerializeField] Transform itemTextParent;

    public Queue<DamageText> damageTextQueue = new Queue<DamageText>();
    public Queue<DamageText> itemTextQueue = new Queue<DamageText>();

    private void Start()
    {
        for (int i = 0; i < defaultCapity; i++)
            damageTextQueue.Enqueue(CreatePoolItem("Damage Text", damageTextParent));

        for (int i = 0; i < 5; i++)
            itemTextQueue.Enqueue(CreatePoolItem("Item Text", itemTextParent));
    }

    private DamageText CreatePoolItem(string itemName, Transform parent)
    {
        DamageText damageText = Instantiate(damageTextPrefab, parent).gameObject.GetComponent<DamageText>();
        damageText.name = itemName;
        damageText.gameObject.SetActive(false);
        return damageText;
    }

    public void ReturnDamageTextObject(DamageText obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(damageTextParent);
        damageTextQueue.Enqueue(obj);
    }

    public void ReturnItemTextObject(DamageText obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(itemTextParent);
        itemTextQueue.Enqueue(obj);
    }

    public void ShowDamageText(float num, Transform target)
    {
        if (damageTextQueue.Count > 0)
        {
            var damageObj = damageTextQueue.Dequeue();
            damageObj.transform.SetParent(null);
            damageObj.gameObject.SetActive(true);
            damageObj.TakeDamage(num, target, new Color(255.0f, 0.0f, 0.0f, 255.0f));
        }
        else
        {
            var newDamageObj = CreatePoolItem("Damage Text", damageTextParent);
            newDamageObj.transform.SetParent(null);
            newDamageObj.gameObject.SetActive(true);
            newDamageObj.TakeDamage(num, target, new Color(255.0f, 0.0f, 0.0f, 255.0f));
            damageTextQueue.Enqueue(newDamageObj);
        }
    }

    public void ShowHealText(float num, Transform target, Color color)
    {
        if (damageTextQueue.Count > 0)
        {
            var damageObj = damageTextQueue.Dequeue();
            damageObj.transform.SetParent(null);
            damageObj.gameObject.SetActive(true);
            damageObj.TakeHeal(num, target, color);
        }
        else
        {
            var newDamageObj = CreatePoolItem("Heal Text", damageTextParent);
            newDamageObj.transform.SetParent(null);
            newDamageObj.gameObject.SetActive(true);
            newDamageObj.TakeHeal(num, target, color);
            damageTextQueue.Enqueue(newDamageObj);
        }
    }

    public void ShowItemText(string itemName, float num, Vector3 target, Color color, int fontsize = 20)
    {
        if (itemTextQueue.Count > 0)
        {
            var itemObj = itemTextQueue.Dequeue();
            itemObj.transform.SetParent(null);
            itemObj.gameObject.SetActive(true);
            itemObj.ItemTypeName(itemName, num, target, color, fontsize);
        }
        else
        {
            var newItemObj = CreatePoolItem("Item Text", itemTextParent);
            newItemObj.transform.SetParent(null);
            newItemObj.ItemTypeName(itemName, num, target, color, fontsize);
            itemTextQueue.Enqueue(newItemObj);
        }
    }
}
