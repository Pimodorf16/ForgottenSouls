using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTarget
{
    public enum Target { Self, Enemy, Aoe, Anyone, Player}
    public Target target;
    public int amount;
}

[Serializable]
public class SkillDamageMod
{
    public StatusEffect statusReq;
    public float modifier;
}

[Serializable]
public class SkillMod
{
    public enum Mod {Damage, NegateDamage, EvasionChance}
    public Mod modifier;
    public int duration;
    public float value;
}

[Serializable]
public class SkillEffect
{
    public StatusEffect effect;
    public int chance;
    public int duration;
}

[Serializable]
public class SkillImmunity
{
    public StatusEffect immuneEffect;
    public int chance;
    public int duration;
}

[Serializable]
public class SkillModifier
{
    public bool damaging;
    public bool healing;
    public float multiplier;
    public int manaCost;
    public SkillTarget target;
    public List<SkillDamageMod> damageMods;
    public List<SkillMod> skillMods;
    public List<SkillEffect> effects;
    public List<SkillImmunity> immunities;
}
