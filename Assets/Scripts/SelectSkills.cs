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

    public void UseSkill()
    {
        switch (character.skill[selectedSkill].modifier[0].target.target)
        {
            case SkillTarget.Target.Self:

                break;
            case SkillTarget.Target.Enemy:
                break;
            case SkillTarget.Target.Aoe:
                break;
            case SkillTarget.Target.Anyone:
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
    }

    public void Freeze()
    {
        selectedSkill = 1;
    }

    public void Spark()
    {
        selectedSkill = 2;
    }

    public void Heal()
    {
        selectedSkill = 3;
    }

    public void Berserk()
    {
        selectedSkill = 4;
    }

    public void MagicShield()
    {
        selectedSkill = 5;
    }

    public void ChainLightning()
    {
        selectedSkill = 6;
    }

    public void GlacialEnhancement()
    {
        selectedSkill = 7;
    }

    public void Meteor()
    {
        selectedSkill = 8;
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
