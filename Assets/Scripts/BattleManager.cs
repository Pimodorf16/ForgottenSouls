using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum States { Start, Player, Target, Skill, Enemy, Won, Lost}

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public EnemyStage stage;
    int stageCount = 1;
    int currentWave = 0;
    int enemyCount = 0;

    public Transform playerStation;
    public List<Transform> enemyStation = new List<Transform>();

    Character character;
    GameObject playerGO;
    public BattleHUD playerHUD;

    public List<Enemy> enemies = new List<Enemy>();

    public States state;

    // Start is called before the first frame update
    void Start()
    {
        state = States.Start;
        StartCoroutine(SetupBattle());
    }

    void ChangeStateToPlayer()
    {
        state = States.Player;
        PlayerTurn();
    }

    void ChangeStateToEnemy()
    {
        state = States.Enemy;
        StartCoroutine(EnemyTurn());
    }

    void ChangeStateToTargeting()
    {
        state = States.Target;
        PlayerTarget();
    }

    void ChangeStateToSkill()
    {
        state = States.Skill;
    }

    void ChangeStateToWon()
    {
        state = States.Won;
        Debug.Log("Player Wins!");
    }

    void ChangeStateToLost()
    {
        state = States.Lost;
    }

    IEnumerator SetupBattle()
    {
        InstantiatePlayer();

        playerHUD.SetStageCount(stageCount);

        stage.RandomizeEnemy();

        playerHUD.SetWaveCount(currentWave + 1, stage.waves.Count);

        foreach (IndividualEnemy enemy in stage.waves[currentWave].enemies)
        {
            InstantiateEnemy(enemy);
        }

        yield return new WaitForSeconds(1f);

        state = States.Player;
        PlayerTurn();
    }

    IEnumerator SpawnNextWave()
    {
        currentWave++;
        playerHUD.SetWaveCount(currentWave + 1, stage.waves.Count);

        foreach (IndividualEnemy enemy in stage.waves[currentWave].enemies)
        {
            InstantiateEnemy(enemy);
        }

        yield return new WaitForSeconds(1f);

        state = States.Player;
        PlayerTurn();
    }

    void InstantiatePlayer()
    {
        playerGO = Instantiate(playerPrefab, playerStation);
        character = playerGO.GetComponent<Character>();
        playerHUD.SetHUD(character);
    }

    void InstantiateEnemy(IndividualEnemy enemy)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation[enemy.stationIndex]);
        enemies.Add(enemyGO.GetComponent<Enemy>());

        enemies[enemy.stationIndex].enemyData = enemy.enemyData;
        enemies[enemy.stationIndex].LoadDataValues(enemy.enemyData);
        enemies[enemy.stationIndex].SetValues();

        playerHUD.targetNames.Add(enemyGO.GetComponent<Enemy>().enemyName);

        IncreaseEnemyCount();
    }

    public void IncreaseEnemyCount()
    {
        enemyCount++;
        playerHUD.SetEnemyCount(enemyCount);
    }

    public void DecreaseEnemyCount()
    {
        enemyCount--;
        playerHUD.SetEnemyCount(enemyCount);
    }

    void PlayerTurn()
    {
        Debug.Log("Player Turn");

        ResetPlayerGuard();

        PlayerWonCheck();

        playerHUD.DisplayPlayerTurnHUD();
    }

    bool PlayerWonCheck()
    {
        if (CheckAllEnemiesDead() == true)
        {
            WaveCheck();
            return true;
        }
        else
        {
            return false;
        }
    }

    void WaveCheck()
    {
        if(stage.waves.Count > currentWave + 1)
        {
            StartCoroutine(SpawnNextWave());
        }
        else
        {
            ChangeStateToWon();
        }
    }

    void PlayerTarget()
    {
        Debug.Log("Targeting");

        playerHUD.DisplayTargetHUD(enemyCount);
    }

    void ResetPlayerGuard()
    {
        character.guarding = false;
        character.guardValue = 0;
    }
    
    IEnumerator PlayerAttack(int enemyIndex)
    {
        Debug.Log("Player Attack");

        int roll = RollCharacterDice();

        ShowDiceRoll(roll);
        AttackEnemy(enemyIndex, roll);
        CheckEnemyHP(enemyIndex);

        yield return new WaitForSeconds(1f);

        if (PlayerWonCheck() == false)
        {
            ChangeStateToEnemy();
        }
    }

    IEnumerator PlayerGuard()
    {
        Debug.Log("Player Guards!");

        character.guarding = true;
        int roll = RollCharacterDice();
        ShowDiceRoll(roll);

        character.guardValue = character.GuardCheck(roll);

        yield return new WaitForSeconds(1f);

        if (PlayerWonCheck() == false)
        {
            ChangeStateToEnemy();
        }
    }

    int RollCharacterDice()
    {
        return character.Roll();
    }

    void ShowDiceRoll(int roll)
    {
        StartCoroutine(playerHUD.SetDiceRoll(roll));
    }

    void AttackEnemy(int enemyIndex, int roll)
    {
        int damage = character.Attack(roll);

        if(CheckEnemyGuardStatus(enemyIndex) == true)
        {
            damage -= enemies[enemyIndex].guardValue;

            if(damage <= 0)
            {
                damage = 0;
            }
        }

        DamageEnemy(enemyIndex, damage);
    }

    public bool CheckEnemyGuardStatus(int enemyIndex)
    {
        return enemies[enemyIndex].guarding;
    }

    public void DamageEnemy(int enemyIndex, int damage)
    {
        enemies[enemyIndex].TakeDamage(damage);
    }

    void CheckEnemyHP(int enemyIndex)
    {
        if (enemies[enemyIndex].currentHP <= 0)
        {
            DestroyEnemyGO(enemyIndex);

            RemoveEnemyFromList(enemyIndex);

            ResetEnemyButtonHUD();

            DecreaseEnemyCount();
        }
    }

    void DestroyEnemyGO(int enemyIndex)
    {
        enemies[enemyIndex].gameObject.SetActive(false);
        Destroy(enemies[enemyIndex]);
    }

    void RemoveEnemyFromList(int enemyIndex)
    {
        enemies.RemoveAt(enemyIndex);
        playerHUD.targetNames.RemoveAt(enemyIndex);
    }

    void ResetEnemyButtonHUD()
    {
        playerHUD.DestroyTargetButtonHUD(enemyCount);
        playerHUD.createdTargets = false;
    }

    bool CheckAllEnemiesDead()
    {
        if(enemyCount <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");

        ResetEnemyGuard();
        
        foreach(Enemy enemy in enemies)
        {
            StartCoroutine(RandomEnemyAction(enemy));

            if(state == States.Lost)
            {
                break;
            }

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);

        if(state == States.Enemy)
        {
            ChangeStateToPlayer();
        }
    }

    IEnumerator RandomEnemyAction(Enemy enemy)
    {
        int rng = Random.Range(1, 3);

        switch (rng)
        {
            case 1:
                StartCoroutine(EnemyAttack(enemy));
                break;
            case 2:
                StartCoroutine(EnemyGuard(enemy));
                break;
            default:
                StartCoroutine(EnemyAttack(enemy));
                break;
        }

        yield return new WaitForSeconds(1f);
    }

    void ResetEnemyGuard()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.guarding = false;
            enemy.guardValue = 0;
        }
    }

    IEnumerator EnemyAttack(Enemy enemy)
    {
        Debug.Log("Enemy Attacks!");
        
        int roll = RollEnemyDice(enemy);

        ShowDiceRoll(roll);
        AttackPlayer(enemy, roll);
        CheckPlayerHP();

        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnemyGuard(Enemy enemy)
    {
        Debug.Log("Enemy Guards!");

        enemy.guarding = true;
        int roll = RollEnemyDice(enemy);
        ShowDiceRoll(roll);

        enemy.guardValue = enemy.GuardCheck(roll);
        
        yield return new WaitForSeconds(1f);
    }

    int RollEnemyDice(Enemy enemy)
    {
        return enemy.Roll();
    }

    void AttackPlayer(Enemy enemy, int roll)
    {
        int damage = enemy.Attack(roll);
        int guard = character.guardValue;
        damage -= guard;

        if (damage < 0)
        {
            damage = 0;
        }

        character.TakeDamage(damage);
        playerHUD.SetHP(character.currentHP);
    }

    void CheckPlayerHP()
    {
        if (character.currentHP <= 0)
        {
            DeactivatePlayerGO();

            ChangeStateToLost();
        }
    }

    void DeactivatePlayerGO()
    {
        playerGO.SetActive(false);
    }

    public void OnAttackButton()
    {
        if(state != States.Player)
        {
            return;
        }

        ChangeStateToTargeting();
    }

    public void OnSkillButton()
    {
        if(state != States.Player)
        {
            return;
        }

        ChangeStateToSkill();
    }

    public void OnGuardButton()
    {
        if (state != States.Player)
        {
            return;
        }

        StartCoroutine(PlayerGuard());
    }

    public void OnEnemySelectButton(int i)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.indicator.gameObject.SetActive(false);
        }
        
        ChangeStateToPlayer();

        StartCoroutine(PlayerAttack(i));
    }

    public void OnBackButton()
    {
        state = States.Player;
        PlayerTurn();
    }

    public void StageComplete()
    {
        //playerHUD.createdTargets = false;
        //playerHUD.originalTargets = false;
        stageCount++;
        playerHUD.SetStageCount(stageCount);
    }
}
