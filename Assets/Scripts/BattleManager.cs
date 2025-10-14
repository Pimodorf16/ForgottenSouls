using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum States { Start, Player, Enemy, Won, Lost}

public class BattleManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public EnemyParty party;

    public Transform playerStation;
    public List<Transform> enemyStation = new List<Transform>();

    Character character;
    public BattleHUD playerHUD;

    List<Enemy> enemies = new List<Enemy>();

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

            enemies[enemy.stationIndex].enemyData = enemy.enemyData;
            enemies[enemy.stationIndex].LoadDataValues(enemy.enemyData);
            enemies[enemy.stationIndex].SetValues();
        }

        yield return new WaitForSeconds(2f);

        state = States.Player;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        Debug.Log("Player Turn");
    }
    
    IEnumerator PlayerAttack()
    {
        Debug.Log("Player Attack");
        int roll = character.Roll();
        StartCoroutine(playerHUD.SetDiceRoll(roll));
        float damage = character.Attack(roll);
        enemies[Random.Range(0, enemies.Count)].TakeDamage(damage);
        Debug.Log("Damage = " + damage);

        yield return new WaitForSeconds(2f);

        state = States.Enemy;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        
        foreach(Enemy enemy in enemies)
        {
            int roll = enemy.Roll();
            float damage = enemy.Attack(roll);
            character.TakeDamage(damage);
            playerHUD.SetHP(character.currentHP);
            Debug.Log("Damage = " + damage);
            yield return new WaitForSeconds(1f);
        }


        yield return new WaitForSeconds(2f);

        state = States.Player;
        PlayerTurn();
    }

    public void OnAttackButton()
    {
        if(state != States.Player)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }
}
