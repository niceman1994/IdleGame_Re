using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DamageText : MonoBehaviour
{
    // 텍스트가 위로 뜨면서 사라지게할 float 변수 3개
    [SerializeField] float moveSpeed;
    [SerializeField] float textFadeOutTime;
    [SerializeField] Vector3 textMoveVector;
    [SerializeField] TextMeshPro damageText;

    // 피격당한 객체가 TakeDamage 메서드를 호출
    public void TakeDamage(float num, Transform target, Color color)
    {
        transform.position = target.position + textMoveVector;
        damageText = gameObject.GetComponent<TextMeshPro>();

        // 정수만 나오도록 Math.Truncate()를 사용함
        damageText.text = Math.Truncate(MathF.Abs(num)).ToString();
        damageText.color = color;
        damageText.name = "Damage Text";
        StartCoroutine(TextPos(() => ObjectPoolManager.Instance.ReturnDamageTextObject(this)));

    }

    // 회복과 관련된 것을 사용해 체력을 회복할 때 TakeHeal 메서드를 호출
    public void TakeHeal(float num, Transform target, Color color)
    {
        transform.position = target.position + textMoveVector;
        damageText = gameObject.GetComponent<TextMeshPro>();

        // 정수만 나오도록 Math.Truncate()를 사용함
        damageText.text = $"+{Math.Truncate(num)}";
        damageText.color = color;
        damageText.name = "Heal";
        StartCoroutine(TextPos(() => ObjectPoolManager.Instance.ReturnDamageTextObject(this)));
    }

    // 회복이 아닌 아이템을 사용할 때는 다른 색으로 표시하는 ItemTypeName 메서드를 호출
    public void ItemTypeName(string itemName, float num, Vector3 target, Color color, int fontsize = 20)
    {
        transform.position = target + Vector3.up;
        damageText = gameObject.GetComponent<TextMeshPro>();

        // 정수만 나오도록 Math.Truncate()를 사용함
        damageText.text = $"+{Math.Truncate(num)} {itemName}";
        damageText.color = color;
        damageText.fontSize = fontsize;
        StartCoroutine(TextPos(() => ObjectPoolManager.Instance.ReturnItemTextObject(this)));
    }

    // 오브젝트가 피해를 받거나 아이템을 사용하거나 체력을 회복했을 때 텍스트가 위로 올라가게 하는 메서드
    private IEnumerator TextPos(Action poolingAction)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= textFadeOutTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.Translate(new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f));
        }
        poolingAction.Invoke();
    }
}
