using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    [Header("Skill")]
    public string skillName;
    public int skillLevel = 1;
    public int levelReq = 1;
    public int cooldown;
    public bool passive;

    [Header("Modifier")]
    public List<SkillModifier> modifier;
}
