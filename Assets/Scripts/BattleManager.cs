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
    public BattleHUD playerHUD;

    public List<Enemy> enemies = new List<Enemy>();

    public States state;

    // Start is called before the first frame update
    void Start()
    {
        state = States.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerStation);
        character = playerGO.GetComponent<Character>();
        playerHUD.SetHUD(character);

        foreach (IndividualEnemy enemy in party.enemies)
        {
            GameObject enemyGO = Instantiate(enemyPrefab, enemyStation[enemy.stationIndex]);
            enemies.Add(enemyGO.GetComponent<Enemy>());
            playerHUD.targetNames.Add(enemyGO.GetComponent<Enemy>().enemyName);

            enemies[enemy.stationIndex].enemyData = enemy.enemyData;
            enemies[enemy.stationIndex].LoadDataValues(enemy.enemyData);
            enemies[enemy.stationIndex].SetValues();

            enemyCount ++;
        }

        playerHUD.SetEnemyCount(enemyCount);

        yield return new WaitForSeconds(1f);

        state = States.Player;
        PlayerTurn();
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
    
    IEnumerator PlayerAttack(int i)
    {
        Debug.Log("Player Attack");
        int roll = character.Roll();
        StartCoroutine(playerHUD.SetDiceRoll(roll));
        float damage = character.Attack(roll);
        enemies[i].TakeDamage(damage);
        Debug.Log("Damage = " + damage);

        if(enemies[i].currentHP <= 0)
        {
            enemies[i].gameObject.SetActive(false);
            Destroy(enemies[i]);
            enemies.RemoveAt(i);
            playerHUD.targetNames.RemoveAt(i);
            playerHUD.DestroyTargetButtonHUD(enemyCount);
            playerHUD.createdTargets = false;
            enemyCount --;
            playerHUD.SetEnemyCount(enemyCount);
        }

        yield return new WaitForSeconds(1f);

        state = States.Enemy;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        
        foreach(Enemy enemy in enemies)
        {
            if(enemy.currentHP > 0)
            {
                int roll = enemy.Roll();
                float damage = enemy.Attack(roll);
                character.TakeDamage(damage);
                playerHUD.SetHP(character.currentHP);
                Debug.Log("Damage = " + damage);
                yield return new WaitForSeconds(1f);
            }
        }


        yield return new WaitForSeconds(1f);

        state = States.Player;
        PlayerTurn();
    }

    public void OnAttackButton()
    {
        if(state != States.Player)
        {
            return;
        }

        state = States.Target;

        PlayerTarget();
    }

    public void OnSkillButton()
    {
        if(state != States.Player)
        {
            return;
        }

        state = States.Skill;
        
    }

    public void OnGuardButton()
    {
        if (state != States.Player)
        {
            return;
        }

        state = States.Enemy;

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
