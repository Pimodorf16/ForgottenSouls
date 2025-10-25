using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Party", menuName = "Enemy Party")]
public class EnemyParty : ScriptableObject
{
    [Header("Party")]
    public string partyName;
    public List<Waves> waves;

    public List<EnemyData> enemyPool;
}
