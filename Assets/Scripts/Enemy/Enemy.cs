using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public Image indicator;
    public Slider hpSlider;

    public enum EnemyType { Rat, Skeleton, Zombie, Goblin, Ogre, None }

    [Header("Enemy")]
    public EnemyType type;
    public string enemyName;
    public enum EnemySize { Small, Medium, Big , None}
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

    private void Awake()
    {
        LoadDataValues(enemyData);

        currentHP = maxHP;
        currentMP = maxMP;
    }

    public void LoadDataValues(EnemyData data)
    {
        enemyName = data.enemyName;
        level = data.level;
        exp = data.exp;
        maxHP = data.maxHP;
        currentHP = data.currentHP;
        maxMP = data.maxMP;
        currentMP = data.currentMP;
        attackStat = data.attackStat;
        defenseStat = data.defenseStat;
        speedStat = data.speedStat;
        luckStat = data.luckStat;
        baseCritRate = data.baseCritRate;
        baseCritDamageMultiplier += data.baseCritDamageMultiplier;
        baseDodge = data.baseDodge;

        weapon = data.weapon;
        skill = data.skill;
        soul = data.soul;
        criticalChance = data.criticalChance;
        evasionChance = data.evasionChance;
    }

    private void Start()
    {
        hpSlider = GetComponentInChildren<Slider>();
    }

    public void SetValues()
    {
        hpSlider.maxValue = enemyData.maxHP;
        hpSlider.value = enemyData.currentHP;
    }

    public void TakeDamage(float damage)
    {
        if(currentHP > 0)
        {
            currentHP -= Mathf.CeilToInt(damage);
            Debug.Log("Took " + Mathf.CeilToInt(damage) + " Damage!");
            hpSlider.value = currentHP;
        }
        else
        {
            currentHP = 0;
            hpSlider.value = currentHP;
        }
        
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public float Attack(int roll)
    {;
        int rng;
        float damage = enemyData.attackStat + enemyData.weapon.damage;

        switch (roll)
        {
            case 1:
                rng = Random.Range(0, 17);
                damage *= (rng / 100f);
                break;
            case 2:
                rng = Random.Range(17, 34);
                damage *= (rng / 100f);
                break;
            case 3:
                rng = Random.Range(34, 51);
                damage *= (rng / 100f);
                break;
            case 4:
                rng = Random.Range(51, 68);
                damage *= (rng / 100f);
                break;
            case 5:
                rng = Random.Range(68, 85);
                damage *= (rng / 100f);
                break;
            case 6:
                rng = Random.Range(85, 101);
                damage *= (rng / 100f);
                break;
            default:
                break;
        }

        return damage;
    }

    public void Target()
    {

    }

    public int Roll()
    {
        int result = Random.Range(1, 7);
        return result;
    }

    public int CritCheck()
    {
        return 0;
    }

    public int GuardCheck()
    {
        return 0;
    }
}
