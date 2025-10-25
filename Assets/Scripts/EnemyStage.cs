using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStage : MonoBehaviour
{
    [Header("Stage")]
    public List<Waves> waves = new List<Waves>();

    public List<EnemyData> enemyPool;

    public void RandomizeEnemy()
    {
        foreach (Waves wave in waves)
        {
            int index = 0;

            foreach(IndividualEnemy enemy in wave.enemies)
            {
                int rng = Random.Range(0, enemyPool.Count);
                enemy.enemyData = enemyPool[rng];
                enemy.stationIndex = index;
                index++;
            }
        }
    }

    public void AddWave()
    {
        waves.Add(new Waves());
    }

    public void AddEnemy()
    {
        if (waves[waves.Count].enemies.Count >= 3)
        {
            AddWave();
        }
        
        waves[waves.Count].enemies.Add(new IndividualEnemy());

        RandomizeEnemy();
    }
}

[System.Serializable]
public class Waves
{
    public List<IndividualEnemy> enemies;
}

[System.Serializable]
public class IndividualEnemy
{
    public EnemyData enemyData;
    public int stationIndex;
}