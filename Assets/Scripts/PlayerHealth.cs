using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth
{
    public int CurrentHP { get; private set; }
    public bool IsInvincible { get; private set; }
    public bool IsDead => CurrentHP <= 0;
    public bool IsKnockedBack { get; set; }

    public event Action OnTakeDamage;
    public event Action OnDie;

    public PlayerHealth(int maxHp)
    {
        CurrentHP = maxHp;
    }

    public void TakeDamage(int amount)
    {
        CurrentHP -= amount;
        IsKnockedBack = true;

        if (IsDead) OnDie?.Invoke();
        else OnTakeDamage?.Invoke();
    }

    public void SetInvincible(bool state) => IsInvincible = state;
}