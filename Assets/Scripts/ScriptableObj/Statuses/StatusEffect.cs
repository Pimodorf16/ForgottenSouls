using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    public enum Status { None, Burn, Wet, Freeze, Stun, Poison, Bleed, Regen, Berserk, Shield, Enhancement }

    [Header("Status Effect")]
    public Status status;
    public int duration = 1;
    public bool negativeEffect;

    [Header("Initiative Effects")]
    public bool allowAction;
    public bool allowGuard;

    [Header("HP Effects")]
    public int damageOverTime;
    public int damagePercentageOverTime;
    public int healOverTime;
    public int healPercentageOverTime;
    public int maxHealthLockPercentageOverTime;

    [Header("Damage Effects")]
    public float damageMultiplier;
    public float damageReceivedMultiplier;

    [Header("Attack Apply Effects")]
    public Status applyStatus;
    public int chance;
    public int applyDuration;

    [Header("Follow Up Status")]
    public Status nextStatus;
}
