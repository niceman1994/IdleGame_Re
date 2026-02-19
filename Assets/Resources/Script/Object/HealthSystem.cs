using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float CurrentHP { get; private set; }
    public float CurrentMaxHP { get; private set; }

    // 오브젝트의 체력과 연관된 이벤트
    public event Action<float> onHealthDamaged;
    public event Action<float, float> onHealthChanged;
    public event Action onDeath;

    // TextPoolManager의 텍스트를 띄울 위치와 연관된 이벤트
    public event Action<float, Transform> onDamagedTaken;
    public event Action<float, Transform, Color> onHealTaken;

    public void TakeDamage(float hp, float damage)
    {
        CurrentHP = hp;
        
        if (CurrentHP > 0)
            CurrentHP -= damage;

        onHealthDamaged?.Invoke(CurrentHP);
    }

    public void ShowDamageText(float damage, Transform target)
    {
        onDamagedTaken?.Invoke(damage, target);
    }

    public void ChangeHealth(float hp, float maxHp, Action hpChangeAction)
    {
        CurrentHP = hp;
        CurrentMaxHP = maxHp;
        onHealthChanged?.Invoke(CurrentHP, CurrentMaxHP);
        hpChangeAction?.Invoke();
    }

    public void ShowHealText(float damage, Transform target, Color color)
    {
        onHealTaken?.Invoke(damage, target, color);
    }

    public void NotifyDeath()
    {
        onDeath?.Invoke();
    }
}
