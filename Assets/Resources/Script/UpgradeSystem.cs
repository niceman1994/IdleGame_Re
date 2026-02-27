using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class UpgradeSystem
{
    protected Player player;
    protected GameGold gameGold;

    public event Action onUpgrade;

    public UpgradeSystem(Player player, GameGold gameGold)
    {
        this.player = player;
        this.gameGold = gameGold;
    }

    public void UpgradeAttack(BigInteger spendGold)
    {
        if (gameGold.Gold >= spendGold)
        {
            player.CurrentAtk(0.2f);
            onUpgrade?.Invoke();
        }
    }

    public void UpgradeHp(BigInteger spendGold)
    {
        if (gameGold.Gold >= spendGold)
        {
            player.HpUp(UnityEngine.Random.Range(24.0f, 32.0f));
            onUpgrade?.Invoke();
        }   
    }

    public void UpgradeAttackSpeed(BigInteger spendGold)
    {
        if (gameGold.Gold >= spendGold)
        {
            player.GetAttackSpeed(0.015f);
            onUpgrade?.Invoke();
        }   
    }

    public void UpgradeMoveSpeed(BigInteger spendGold)
    {
        if (gameGold.Gold >= spendGold)
        {
            player.GetMoveSpeed(0.04f);
            onUpgrade?.Invoke();
        }
    }

    public void UpgradeThunderPower(BigInteger spendGold)
    {
        if (gameGold.Gold >= spendGold)
        {
            GameManager.Instance.AddThunderPower(0.2f);
            onUpgrade?.Invoke();
        }
    }

    public void UpgradeEarnGold(BigInteger spendGold, int clickCount)
    {
        if (gameGold.Gold >= spendGold)
        {
            GameManager.Instance.gameGold.AddEarnGold(clickCount);
            onUpgrade?.Invoke();
        }
    }}
