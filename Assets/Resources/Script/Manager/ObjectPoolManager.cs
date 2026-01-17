using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField] int defaultCapity = 5;
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] Transform damageParent;
    [SerializeField] Transform itemParent;

    public Queue<DamageText> poolingTextQueue = new Queue<DamageText>();
    public Queue<DamageText> poolingItemQueue = new Queue<DamageText>();

    private void Start()
    {
        for (int i = 0; i < defaultCapity; i++)
            poolingTextQueue.Enqueue(CreatePoolItem("Damage Text", damageParent));

        for (int i = 0; i < 5; i++)
            poolingItemQueue.Enqueue(CreatePoolItem("Item Text", itemParent));
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
        obj.transform.SetParent(damageParent);
        poolingTextQueue.Enqueue(obj);
    }

    public void ReturnItemTextObject(DamageText obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(itemParent);
        poolingItemQueue.Enqueue(obj);
    }

    public void ShowDamageText(float num, Transform target)
    {
        if (poolingTextQueue.Count > 0)
        {
            var damageObj = poolingTextQueue.Dequeue();
            damageObj.transform.SetParent(null);
            damageObj.gameObject.SetActive(true);
            damageObj.TakeDamage(num, target, new Color(255.0f, 0.0f, 0.0f, 255.0f));
        }
        else
        {
            var newDamageObj = CreatePoolItem("Damage Text", damageParent);
            newDamageObj.transform.SetParent(null);
            newDamageObj.gameObject.SetActive(true);
            newDamageObj.TakeDamage(num, target, new Color(255.0f, 0.0f, 0.0f, 255.0f));
            poolingTextQueue.Enqueue(newDamageObj);
        }
    }

    public void ShowHealText(float num, Transform target, Color color)
    {
        if (poolingTextQueue.Count > 0)
        {
            var damageObj = poolingTextQueue.Dequeue();
            damageObj.transform.SetParent(null);
            damageObj.gameObject.SetActive(true);
            damageObj.TakeHeal(num, target, color);
        }
        else
        {
            var newDamageObj = CreatePoolItem("Heal Text", damageParent);
            newDamageObj.transform.SetParent(null);
            newDamageObj.gameObject.SetActive(true);
            newDamageObj.TakeHeal(num, target, color);
            poolingTextQueue.Enqueue(newDamageObj);
        }
    }

    public void ShowItemText(string itemName, float num, Vector3 target, Color color, int fontsize = 20)
    {
        if (poolingItemQueue.Count > 0)
        {
            var itemObj = poolingItemQueue.Dequeue();
            itemObj.transform.SetParent(null);
            itemObj.gameObject.SetActive(true);
            itemObj.ItemTypeName(itemName, num, target, color);
        }
        else
        {
            var newItemObj = CreatePoolItem("Item Text", itemParent);
            newItemObj.transform.SetParent(null);
            newItemObj.ItemTypeName(itemName, num, target, color);
            poolingItemQueue.Enqueue(newItemObj);
        }
    }
}
