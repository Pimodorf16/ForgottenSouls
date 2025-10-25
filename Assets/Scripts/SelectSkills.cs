using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSkills : MonoBehaviour
{
    [Header("Battle Manager")]
    public BattleManager battleManager;
    public Character character;

    [Header("Skill")]
    public int selectedSkill = 0;

    public bool CheckMPIsEnough()
    {
        if(character.currentMP < character.skill[selectedSkill].modifier[0].manaCost)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void UseSkill()
    {
        if (CheckMPIsEnough() == true)
        {
            TargetSkill();
        }
        else
        {
            Debug.Log("Not Enough MP!");
        }
    }

    public void TargetSkill()
    {
        switch (character.skill[selectedSkill].modifier[0].target.target)
        {
            case SkillTarget.Target.Self:
                StartCoroutine(battleManager.PlayerSkillSelf(selectedSkill));
                break;
            case SkillTarget.Target.Enemy:
                battleManager.playerHUD.DisplaySkillTargetHUD(battleManager.enemyCount);
                break;
            case SkillTarget.Target.Aoe:
                StartCoroutine(battleManager.PlayerSkillAllOfEnemies(selectedSkill));
                break;
            case SkillTarget.Target.Anyone:
                battleManager.playerHUD.DisplaySkillTargetHUD(battleManager.enemyCount);
                break;
            case SkillTarget.Target.Player:
                break;
            default:
                break;
        }
    }
    
    public void Fireball()
    {
        selectedSkill = 0;
        UseSkill();
    }

    public void Freeze()
    {
        selectedSkill = 1;
        UseSkill();
    }

    public void Spark()
    {
        selectedSkill = 2;
        UseSkill();
    }

    public void Heal()
    {
        selectedSkill = 3;
        UseSkill();
    }

    public void Berserk()
    {
        selectedSkill = 4;
        UseSkill();
    }

    public void MagicShield()
    {
        selectedSkill = 5;
        UseSkill();
    }

    public void ChainLightning()
    {
        selectedSkill = 6;
        UseSkill();
    }

    public void GlacialEnhancement()
    {
        selectedSkill = 7;
        UseSkill();
    }

    public void Meteor()
    {
        selectedSkill = 8;
        UseSkill();
    }

    public void Bite()
    {
        
    }

    public void Poison()
    {
        
    }

    public void Stab()
    {
        
    }

    public void Tiny()
    {

    }

    public void Undead()
    {

    }

    public void Huge()
    {

    }
}
