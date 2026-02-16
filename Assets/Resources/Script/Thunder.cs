using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] float damage;
    [SerializeField] BoxCollider2D boxCollider;

    private Animator thunderAnimator;

    private void Awake()
    {
        thunderAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 번개 애니메이션이 끝나면 오브젝트를 비활성화시킴
        if (thunderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    // 번개에 2d 박스 콜라이더를 달아 크기를 정하고, 충돌이 끝날 때 충돌한 객체의 체력에 영향을 줌
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Boss"))
            collision.gameObject.GetComponent<IObject>().GetAttackDamage(damage);
    }

    public float AddPower(float addDamage)
    {
        damage += addDamage;
        return damage;
    }
}
