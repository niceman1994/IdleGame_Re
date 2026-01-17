using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float currentHP { get; private set; }

    public Action<float> onHealthChanged;

    public void TakeDamage(float hp, float damage)
    {
        currentHP = hp;
        
        if (currentHP > 0)
            currentHP -= damage;

        onHealthChanged?.Invoke(currentHP);
    }
}
