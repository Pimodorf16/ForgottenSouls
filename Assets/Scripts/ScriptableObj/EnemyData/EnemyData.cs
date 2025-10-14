using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType { Rat, Skeleton, Zombie, Goblin, Ogre, None}
    
    [Header("Enemy")]
    public EnemyType type;
    public string enemyName;
    public enum EnemySize { Small, Medium, Big, None }
    public EnemySize enemySize;

    [Header("Level")]
    public int level = 1;
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
    public float baseCritRate = 0.10f;
    public float baseCritDamageMultiplier = 0.15f;
    public float baseDodge = 0.10f;

    public int baseGold;

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
        criticalChance = (luckStat * (5 / 1000)) + baseCritRate;
        evasionChance = (speedStat * (5 / 1000)) + baseDodge;
    }
}
