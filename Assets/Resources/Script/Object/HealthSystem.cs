using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<float> onHealthChanged;
    public event Action<float, float> onHealthUp;
    public event Action onDeath;

    public void TakeDamage(float hp, float damage)
    {
        if (hp > 0)
            hp -= damage;

        onHealthChanged?.Invoke(hp);
    }

    public void HpUp(float hp, float maxHp, Action hpChangeAction)
    {
        onHealthUp?.Invoke(hp, maxHp);
        hpChangeAction?.Invoke();
    }

    public void NotifyDeath()
    {
        onDeath?.Invoke();
    }
}
