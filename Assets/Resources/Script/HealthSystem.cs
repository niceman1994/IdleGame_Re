using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float CurrentHP { get; private set; }

    public event Action<float> onHealthChanged;
    public event Action onDeath;

    public void TakeDamage(float hp, float damage)
    {
        CurrentHP = hp;
        
        if (CurrentHP > 0)
            CurrentHP -= damage;

        onHealthChanged?.Invoke(CurrentHP);
    }

    public void NotifyDeath()
    {
        onDeath?.Invoke();
    }
}
