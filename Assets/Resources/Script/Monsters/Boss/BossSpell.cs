using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : MonoBehaviour
{
    [SerializeField] float damage;
    private float waitTime = 1.0f;

    // 데미지 처리 후 BossSpell 오브젝트를 큐에 넣기 위해서 사용하는 이벤트 변수
    public Action spellEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<IObject>().GetAttackDamage(damage);
            StartCoroutine(Damage());
        }
    }

    private IEnumerator Damage()
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
        spellEvent.Invoke();
    }
}
