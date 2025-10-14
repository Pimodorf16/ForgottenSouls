using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon")]
    public string weaponName;

    [Header("Status")]
    public int damage;
    public int target;

    [Header("Modifier")]
    public WeaponModifier modifier;
}
