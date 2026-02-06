using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : MonoBehaviour
{
    [SerializeField] bool isReturned;
    [SerializeField] float damage;

    private Animator spellAnimator;

    public Action spellEnqueueAction;

    private void OnEnable()
    {
        spellAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (spellAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !isReturned)
        {
            isReturned = true;
            DeactivateSpell();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<IObject>().GetAttackDamage(damage);
        }
    }

    public void DeactivateSpell()
    {
        if (spellEnqueueAction != null)
        {
            spellEnqueueAction.Invoke();
            gameObject.SetActive(false);
            isReturned = false;
            Debug.Log("BossSpell 회수 완료");
        }
    }
}
