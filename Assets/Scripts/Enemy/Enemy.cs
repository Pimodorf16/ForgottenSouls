using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    public Slider hpSlider;

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
        if(enemyData.currentHP > 0)
        {
            enemyData.currentHP -= Mathf.CeilToInt(damage);
            Debug.Log("Took " + Mathf.CeilToInt(damage) + " Damage!");
            hpSlider.value = enemyData.currentHP;
        }
        else
        {
            enemyData.currentHP = 0;
            hpSlider.value = enemyData.currentHP;
        }
        
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public float Attack()
    {
        int roll = Roll();
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
