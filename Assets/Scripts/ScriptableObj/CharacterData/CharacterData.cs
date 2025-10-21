using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character")]
    public string characterName;

    [Header("Level")]
    public int level = 1;
    public int maxExp = 30;
    public int exp = 0;

    [Header("Status")]
    public int maxHP = 20;
    public int currentHP;
    public int maxMP = 10;
    public int currentMP;
    public int attackStat = 5;
    public int defenseStat = 5;
    public int speedStat = 5;
    public int luckStat = 5;
    public int manaRecoveryOvertime = 2;
    public float baseCritRate = 0.10f;
    public float baseCritDamageMultiplier = 0.15f;
    public float baseDodge = 0.10f;

    [Header("Weapon")]
    public Weapon weapon;

    [Header("Skills")]
    public List<Skill> skill;

    [Header("Soul")]
    public Soul soul;

    [Header("Probability")]
    public float criticalChance;
    public float evasionChance;

    public void calculateValues()
    {
        criticalChance = (float)Math.Round((luckStat * (5.0 / 1000)) + baseCritRate, 2);
        evasionChance = (float)Math.Round((speedStat * (5.0 / 1000)) + baseDodge, 2);
    }
}
