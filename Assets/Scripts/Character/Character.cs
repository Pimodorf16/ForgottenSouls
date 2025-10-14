using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterData characterData;

    public float Attack()
    {
        int roll = Roll();
        int rng;
        float damage = characterData.attackStat + characterData.weapon.damage;
        
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

    public void TakeDamage(float damage)
    {
        if(characterData.currentHP > 0)
        {
            characterData.currentHP -= Mathf.CeilToInt(damage);
            Debug.Log("Took " + Mathf.CeilToInt(damage) + " Damage!");
        }
        else
        {
            characterData.currentHP = 0;
        }
    }
}
