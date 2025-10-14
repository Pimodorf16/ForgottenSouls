using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Soul", menuName = "Soul")]
public class Soul : ScriptableObject
{
    [Header("Soul")]
    public string soulName;

    [Header("Attachments")]
    public Attachment attachment1;
    public Attachment attachment2;
}
