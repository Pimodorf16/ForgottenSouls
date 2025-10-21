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

    public EnemyParty party;
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
    }

    void ChangeStateToLost()
    {
        state = States.Lost;
    }

    IEnumerator SetupBattle()
    {
        InstantiatePlayer();

        foreach (IndividualEnemy enemy in party.enemies)
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
        playerHUD.targetNames.Add(enemyGO.GetComponent<Enemy>().enemyName);

        enemies[enemy.stationIndex].enemyData = enemy.enemyData;
        enemies[enemy.stationIndex].LoadDataValues(enemy.enemyData);
        enemies[enemy.stationIndex].SetValues();

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

        playerHUD.DisplayPlayerTurnHUD();
    }

    void PlayerTarget()
    {
        Debug.Log("Targeting");

        playerHUD.DisplayTargetHUD(enemyCount);
    }
    
    IEnumerator PlayerAttack(int enemyIndex)
    {
        Debug.Log("Player Attack");

        int roll = RollCharacterDice();

        ShowDiceRoll(roll);
        AttackEnemy(enemyIndex, roll);
        CheckEnemyHP(enemyIndex);

        yield return new WaitForSeconds(1f);

        if(CheckAllEnemiesDead() == true)
        {
            ChangeStateToWon();
        }
        else
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
        float damage = character.Attack(roll);

        DamageEnemy(enemyIndex, damage);
    }

    public void DamageEnemy(int enemyIndex, float damage)
    {
        enemies[enemyIndex].TakeDamage(damage);
        Debug.Log("Damage = " + damage);
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
        
        foreach(Enemy enemy in enemies)
        {
            StartCoroutine(EnemyAttack(enemy));
        }

        yield return new WaitForSeconds(1f);

        ChangeStateToPlayer();
    }

    IEnumerator EnemyAttack(Enemy enemy)
    {
        int roll = RollEnemyDice(enemy);

        ShowDiceRoll(roll);
        AttackPlayer(enemy, roll);
        CheckPlayerHP();

        yield return new WaitForSeconds(1f);
    }

    int RollEnemyDice(Enemy enemy)
    {
        return enemy.Roll();
    }

    void AttackPlayer(Enemy enemy, int roll)
    {
        float damage = enemy.Attack(roll);
        character.TakeDamage(damage);
        playerHUD.SetHP(character.currentHP);

        Debug.Log("Damage = " + damage);
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

        ChangeStateToEnemy();
    }

    public void OnEnemySelectButton(int i)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.indicator.gameObject.SetActive(false);
        }
        
        state = States.Player;
        PlayerTurn();

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
    }
}
