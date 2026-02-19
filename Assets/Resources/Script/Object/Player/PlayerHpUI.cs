using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Text currentHp;
    [SerializeField] Text maxHp;
    [SerializeField] Image hpBar;

    private void Awake()
    {
        HpbarInit(player);
    }

    public void HpbarInit(Player player)
    {
        player.onHpbarChanged += UpdateHpbarUI;
    }

    private void UpdateHpbarUI(float objectCurrentHp, float objectMaxHp)
    {
        currentHp.text = $"{Math.Truncate(objectCurrentHp)}";
        maxHp.text = $"{Math.Truncate(objectMaxHp)}";
        hpBar.fillAmount = objectCurrentHp / objectMaxHp;
    }
}
