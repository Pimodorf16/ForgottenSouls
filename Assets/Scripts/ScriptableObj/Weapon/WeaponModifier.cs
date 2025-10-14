using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponModStat
{
    public enum WeaponMod { Att, Def, Spd, Lck, CrtRate, CrtDmg, Dmg }
    public WeaponMod mod;
    public float modifier;
}

[Serializable]
public class WeaponEffect
{
    public StatusEffect effect;
    public int chance;
    public int duration;
}

[Serializable]
public class WeaponModifier
{
    public List<WeaponModStat> mods;

    public enum WeaponModReqSize { Small, Medium, Big }
    public List<WeaponModReqSize> weaponModReqSizes;

    public List<WeaponEffect> weaponEffect;
    public enum WeaponPassive { LastToAttack, Rush } //rush: if kills, attack 1 random enemy
    public List <WeaponPassive> passive;
}
