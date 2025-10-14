using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attachment", menuName = "Attachment")]
public class Attachment : ScriptableObject
{
    public enum AttachmentName
    {
        ResultTotem,
        SoulsBondContract,
        KineticConverter,
        PendantOfStatus,
        LoadedDice,
        ExpRing,
        VampiricRing,
        BloodPactSeal,
        OverchargedCrystal,
        GreedRing
    }
    
    [Header("Attachment")]
    public AttachmentName attachmentName;

    [Header("Effects")]
    public int stat1;
    public int stat2;
    public int stat3;

    public void OnEquip()
    {
        switch (attachmentName)
        {
            case AttachmentName.ResultTotem:
                //Dice Result +1, max value 6
                break;
            case AttachmentName.SoulsBondContract:
                //Set Death Count, each death 1++
                //If Roll higher than Death Count, resurrect with 50% hp mp
                break;
            case AttachmentName.KineticConverter:
                //Set Damage Bonus, each time damaged add 20% of damage to damage bonus
                //Next attack = attack result + damage bonus
                break;
            case AttachmentName.PendantOfStatus:
                //If have negative status Damage + 30%
                //cannot heal negative status
                break;
            case AttachmentName.LoadedDice:
                //if dice roll = even, Final result + 20%
                //else, - 20%
                break;
            case AttachmentName.ExpRing:
                //expRate + 25%
                break;
            case AttachmentName.VampiricRing:
                //Hp + 15% damage dealt
                //evasion chance - 10
                break;
            case AttachmentName.BloodPactSeal:
                //crit damage + 50
                //if crit, - 5% max hp
                break;
            case AttachmentName.OverchargedCrystal:
                //skill damage + 25%
                //after use skill, cannot use for one turn.
                break;
            case AttachmentName.GreedRing:
                //each level gold gain from enemy + 25%
                break;
            default:
                break;
        }
    }
}
