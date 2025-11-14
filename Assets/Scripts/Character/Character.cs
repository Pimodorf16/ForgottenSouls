using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterData characterData;
    public Animator animator;

    [Header("Character")]
    public string characterName;

    [Header("Level")]
    public int level = 1;
    public int maxExp = 30;
    public int exp = 0;
    public int gold = 0;
    public int point = 0;

    [Header("Status")]
    public int maxHP = 20;
    public int currentHP;
    public int maxMP = 10;
    public int currentMP;
    public int attackStat = 5;
    public int defenseStat = 5;
    public int speedStat = 5;
    public int luckStat = 5;
    public float baseCritRate = 0.01f;
    public float baseCritDamageMultiplier = 0.15f;
    public float baseDodge = 0.01f;

    [Header("Weapon")]
    public Weapon weapon;

    [Header("Skills")]
    public List<Skill> skill;

    [Header("Soul")]
    public Soul soul;

    [Header("Probability")]
    public float criticalChance;
    public float evasionChance;

    [Header("In Battle")]
    public bool guarding = false;
    public int guardValue = 0;

    [Header("Status Effect")]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    public List<StatusEffect> immunities = new List<StatusEffect>();
    public List<StatusEffect> statusEffectsToRemove = new List<StatusEffect>();
    public List<StatusEffect> immunitiesToRemove = new List<StatusEffect>();
    public bool allowAction = true;
    public bool allowGuard = true;
    public int damageOverTime = 0;
    public int damagePercentageOverTime = 0;
    public int healOverTime = 0;
    public int healPercentageOverTime = 0;
    public int maxHealthLockPercentageOverTime = 0;
    public float damageMultiplier = 1;
    public float damageReceivedMultiplier = 1;
    public StatusEffect.Status applyStatus = StatusEffect.Status.None;
    public int chance = 0;
    public int applyDuration = 0;

    private void Awake()
    {
        LoadDataValues(characterData);
        
        animator = GetComponent<Animator>();

        currentHP = maxHP;
        currentMP = maxMP;
    }

    public void PlayTheSFX(int index)
    {
        GetComponent<PlaySound>().PlayByIndex(index);
    }

    public void LoadDataValues(CharacterData data)
    {
        characterName = data.characterName;
        level = data.level;
        maxExp = data.maxExp;
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
        criticalChance = (0.005f * luckStat) + baseCritRate;
        evasionChance = (0.005f * speedStat) + baseDodge;
    }

    public void SetStatusEffect(StatusEffect status, int duration)
    {
        if(CheckImmunity(status) == false)
        {
            statusEffects.Add(status);
            status.duration = duration;

            if (status.allowAction == false)
            {
                allowAction = status.allowAction;
            }
            if (status.allowGuard == false)
            {
                allowGuard = status.allowGuard;
            }

            if(allowAction == false)
            {
                animator.speed = 0f;
            }else if(allowAction == true)
            {
                animator.speed = 1f;
            }

                damageOverTime += status.damageOverTime;
            damagePercentageOverTime += status.damagePercentageOverTime;
            healOverTime += status.healOverTime;
            healPercentageOverTime += status.healPercentageOverTime;
            maxHealthLockPercentageOverTime += status.maxHealthLockPercentageOverTime;
            damageMultiplier += status.damageMultiplier;
            damageReceivedMultiplier += status.damageReceivedMultiplier;
            applyStatus = status.applyStatus;
            chance += status.chance;
            applyDuration += status.applyDuration;
        }
    }

    public void RemoveStatusEffect(StatusEffect status)
    {
        Debug.Log("Removing Status Effect " + status.status + " from " + characterName);

        if (status.allowAction == false)
        {
            allowAction = true;
        }
        if (status.allowGuard == false)
        {
            allowGuard = true;
        }

        if (allowAction == true)
        {
            animator.speed = 1f;
        }

        damageOverTime -= status.damageOverTime;
        damagePercentageOverTime -= status.damagePercentageOverTime;
        healOverTime -= status.healOverTime;
        healPercentageOverTime -= status.healPercentageOverTime;
        maxHealthLockPercentageOverTime -= status.maxHealthLockPercentageOverTime;
        damageMultiplier -= status.damageMultiplier;
        damageReceivedMultiplier -= status.damageReceivedMultiplier;
        applyStatus = status.applyStatus;
        chance -= status.chance;
        applyDuration -= status.applyDuration;
    }

    public void CheckStatusEffectDuration()
    {
        foreach (StatusEffect effect in statusEffects)
        {
            if (effect.duration <= 0)
            {
                RemoveStatusEffect(effect);
                statusEffectsToRemove.Add(effect);
            }
            else
            {
                effect.duration--;
            }
        }

        foreach (StatusEffect effect in statusEffectsToRemove)
        {
            statusEffects.Remove(effect);
        }

        statusEffectsToRemove.Clear();
    }

    public void ClearAllStatusEffect()
    {
        foreach (StatusEffect effect in statusEffects)
        {
            RemoveStatusEffect(effect);
            statusEffectsToRemove.Add(effect);
        }
        
        foreach (StatusEffect effect in statusEffectsToRemove)
        {
            statusEffects.Remove(effect);
        }

        statusEffectsToRemove.Clear();
    }

    public bool CheckImmunity(StatusEffect newStatus)
    {
        foreach(StatusEffect immune in immunities)
        {
            if(newStatus == immune)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public void SetImmunity(StatusEffect immune, int duration)
    {
        immunities.Add(immune);
        immune.duration = duration;
    }

    public void RemoveImmunity(StatusEffect immune)
    {
        immunities.Remove(immune);
    }

    public void CheckImmunityDuration()
    {
        foreach (StatusEffect immune in immunities)
        {
            if (immune.duration <= 0)
            {
                immunitiesToRemove.Add(immune);
            }
            else
            {
                immune.duration--;
            }
        }

        foreach (StatusEffect immune in immunitiesToRemove)
        {
            immunities.Remove(immune);
        }

        immunitiesToRemove.Clear();
    }

    float RollValueConversion(int roll)
    {
        float rng = 0f;

        switch (roll)
        {
            case 1:
                rng = Random.Range(0f, 17f);
                break;
            case 2:
                rng = Random.Range(17f, 34f);
                break;
            case 3:
                rng = Random.Range(34f, 51f);
                break;
            case 4:
                rng = Random.Range(51f, 68f);
                break;
            case 5:
                rng = Random.Range(68f, 85f);
                break;
            case 6:
                rng = Random.Range(85f, 101f);
                break;
            default:
                break;
        }

        rng = rng / 100f;

        return rng;
    }

    public int Attack(int roll)
    {
        ;
        float rng = RollValueConversion(roll);
        float damage = characterData.attackStat + characterData.weapon.damage;
        damage *= rng;
        damage *= damageMultiplier;

        Debug.Log("Damage = " + damage);
        PlayTheSFX(0);
        damage = Mathf.CeilToInt(damage);

        return (int)damage;
    }

    public void Skill(int roll)
    {

    }

    public int Roll()
    {
        int result = Random.Range(1, 7);
        Debug.Log("Character roll = " + result);
        return result;
    }

    public int CritCheck()
    {
        return 0;
    }

    public int GuardCheck(int roll)
    {
        float rng = RollValueConversion(roll);
        Debug.Log("Player Guard RNG = " + rng);

        float guard = characterData.defenseStat * 2 * rng;

        guard = Mathf.CeilToInt(guard);

        Debug.Log("Player Guard = " + guard);

        return (int)guard;
    }

    public void TakeDamage(int damage)
    {
        if(currentHP > 0)
        {
            float pastHP = currentHP;
            currentHP -= Mathf.CeilToInt((float)damage * damageReceivedMultiplier);
            Debug.Log("Player Took " + damage + " Damage!");
            if (currentHP < pastHP)
            {
                PlayTheSFX(0);
            }
        }
        else
        {
            currentHP = 0;
        }
    }

    public void UseMP(int cost)
    {
        currentMP -= cost;
        Debug.Log("Used " + cost + " Mana!");
    }

    public int Heal(float multiplier)
    {
        currentHP += (int)(multiplier * maxHP);
        Debug.Log("Healed " + (int)(multiplier * maxHP) + " HP!");
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        return currentHP;
    }

    public void RegenMP(int regen)
    {
        currentMP += regen;
        Debug.Log("Restore" + regen + " Mana!");
        if(currentMP > maxMP)
        {
            currentMP = maxMP;
        }
    }


}
