using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public enum States { Start, Player, Target, Skill, Enemy, Won, Lost}

public class BattleManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    [Header("States")]
    public States state;

    [Header("Enemy Components")]
    public EnemyStage stage;
    int stageCount = 1;
    int currentWave = 0;
    public int enemyCount = 0;
    int ratCount = 0;
    int skeletonCount = 0;
    int zombieCount = 0;
    int goblinCount = 0;
    int ogreCount = 0;
    public List<Enemy> enemies = new List<Enemy>();

    [Header("Player Components")]
    Character character;
    GameObject playerGO;
    public BattleHUD playerHUD;
    public SelectSkills selectSkills;

    [Header("Stations")]
    public Transform playerStation;
    public List<Transform> enemyStation = new List<Transform>();

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
        PlayerSelectSkill();
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

        SetEnemyName();
        SetEnemyButtonNames();

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

        SetEnemyName();
        SetEnemyButtonNames();

        yield return new WaitForSeconds(1f);

        state = States.Player;
        PlayerTurn();
    }

    void InstantiatePlayer()
    {
        playerGO = Instantiate(playerPrefab, playerStation);
        character = playerGO.GetComponent<Character>();
        selectSkills.character = character;
        playerHUD.SetHUD(character);
    }

    void InstantiateEnemy(IndividualEnemy enemy)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation[enemy.stationIndex]);
        enemies.Add(enemyGO.GetComponent<Enemy>());
        AddEnemyTypeAmount(enemy.enemyData);

        enemies[enemy.stationIndex].enemyData = enemy.enemyData;
        enemies[enemy.stationIndex].stationIndex = enemy.stationIndex;
        enemies[enemy.stationIndex].LoadDataValues(enemy.enemyData);
        enemies[enemy.stationIndex].SetValues();

        IncreaseEnemyCount();
    }

    void AddEnemyTypeAmount(EnemyData enemy)
    {
        switch (enemy.type)
        {
            case EnemyData.EnemyType.Rat:
                ratCount++;
                break;
            case EnemyData.EnemyType.Skeleton:
                skeletonCount++;
                break;
            case EnemyData.EnemyType.Zombie:
                zombieCount++;
                break;
            case EnemyData.EnemyType.Goblin:
                goblinCount++;
                break;
            case EnemyData.EnemyType.Ogre:
                ogreCount++;
                break;
            case EnemyData.EnemyType.None:
                break;
            default:
                break;
        }
    }

    void SetEnemyName()
    {
        int rat = 0;
        int skeleton = 0;
        int zombie = 0;
        int goblin = 0;
        int ogre = 0;
        
        foreach (Enemy enemy in enemies)
        {
            switch (enemy.type)
            {
                case EnemyData.EnemyType.Rat:
                    if(ratCount > 1)
                    {
                        rat++;
                        enemy.enemyName = (enemy.enemyName + " " + rat);
                    }
                    else
                    {
                        break;
                    }
                        break;
                case EnemyData.EnemyType.Skeleton:
                    if (skeletonCount > 1)
                    {
                        skeleton++;
                        enemy.enemyName = (enemy.enemyName + " " + skeleton);
                    }
                    else
                    {
                        break;
                    }
                    break;
                case EnemyData.EnemyType.Zombie:
                    if (zombieCount > 1)
                    {
                        zombie++;
                        enemy.enemyName = (enemy.enemyName + " " + zombie);
                    }
                    else
                    {
                        break;
                    }
                    break;
                case EnemyData.EnemyType.Goblin:
                    if (goblinCount > 1)
                    {
                        goblin++;
                        enemy.enemyName = (enemy.enemyName + " " + goblin);
                    }
                    else
                    {
                        break;
                    }
                    break;
                case EnemyData.EnemyType.Ogre:
                    if (ogreCount > 1)
                    {
                        ogre++;
                        enemy.enemyName = (enemy.enemyName + " " + ogre);
                    }
                    else
                    {
                        break;
                    }
                    break;
                case EnemyData.EnemyType.None:
                    break;
                default:
                    break;
            }
        }
    }

    void SetEnemyButtonNames()
    {
        foreach(Enemy enemy in enemies)
        {
            playerHUD.targetNames.Add(enemy.enemyName);
        }
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
        character.CheckStatusEffectDuration();
        PlayerDOT();
        PlayerWonCheck();
        
        if (character.allowAction == true)
        {
            playerHUD.DisplayPlayerTurnHUD();
        }
        else
        {
            Debug.Log("Cannot Move! Turn Skipped!");
            ChangeStateToEnemy();
        }
    }

    void PlayerDOT()
    {
        character.TakeDamage(character.maxHP * (character.damagePercentageOverTime / 100));
        character.TakeDamage(character.maxHP * (character.maxHealthLockPercentageOverTime) / 100);
        character.TakeDamage(character.damageOverTime);
        character.maxHP -= Mathf.CeilToInt(character.maxHealthLockPercentageOverTime / 100);
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

    void PlayerSelectSkill()
    {
        playerHUD.DisplaySkillSelectionHUD();
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
        AttackEnemy(enemyIndex, roll, 1f);
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

    public IEnumerator PlayerSkill(int enemyIndex, int skill)
    {
        Skill skillData = character.skill[skill];
        Debug.Log("Player Uses Skill: " + skillData.skillName + "!");

        character.UseMP(skillData.modifier[0].manaCost);
        playerHUD.SetMP(character.currentMP);

        CheckSkillDamaging(skillData, 1, enemyIndex);

        CheckSkillEffects(skillData, 1, enemyIndex);

        CheckSkillImmunities(skillData, 1, enemyIndex);

        yield return new WaitForSeconds(1f);

        if (PlayerWonCheck() == false)
        {
            ChangeStateToEnemy();
        }
    }

    public IEnumerator PlayerSkillAllOfEnemies(int skill)
    {
        Skill skillData = character.skill[skill];
        Debug.Log("Player Uses Skill: " + skillData.skillName + "!");

        character.UseMP(skillData.modifier[0].manaCost);
        playerHUD.SetMP(character.currentMP);

        CheckSkillDamaging(skillData, 2, 0);

        CheckSkillEffects(skillData, 2, 0);

        CheckSkillImmunities(skillData, 2, 0);

        playerHUD.DisplayPlayerTurnHUD();

        yield return new WaitForSeconds(1f);

        if (PlayerWonCheck() == false)
        {
            ChangeStateToEnemy();
        }
    }

    public IEnumerator PlayerSkillSelf(int skill)
    {
        Skill skillData = character.skill[skill];
        Debug.Log("Player Uses Skill: " + skillData.skillName + "!");

        character.UseMP(skillData.modifier[0].manaCost);
        playerHUD.SetMP(character.currentMP);

        CheckSkillDamaging(skillData, 0, 0);

        CheckSkillHealing(skillData);

        CheckSkillEffects(skillData, 0, 0);

        CheckSkillImmunities(skillData, 0, 0);

        playerHUD.DisplayPlayerTurnHUD();

        yield return new WaitForSeconds(1f);

        if (PlayerWonCheck() == false)
        {
            ChangeStateToEnemy();
        }
    }

    void CheckSkillDamaging(Skill skill, int target, int enemyIndex)
    {
        if (skill.modifier[0].damaging == true)
        {
            int roll = RollCharacterDice();
            ShowDiceRoll(roll);

            switch (target)
            {
                case 0:
                    AttackSelf(roll, skill.modifier[0].multiplier);
                    CheckPlayerHP();
                    break;
                case 1:
                    AttackEnemy(enemyIndex, roll, skill.modifier[0].multiplier);
                    CheckEnemyHP(enemyIndex);
                    break;
                case 2:
                    AttackAllEnemy(roll, skill.modifier[0].multiplier);
                    CheckEnemyHP(enemyIndex);
                    break;
                default:
                    break;
            }
        }
    }

    void CheckSkillHealing(Skill skill)
    {
        if (skill.modifier[0].healing == true)
        {
            int heal = character.Heal(skill.modifier[0].multiplier);
            playerHUD.SetHP(heal);
        }
    }

    void CheckSkillEffects(Skill skill, int target, int enemyIndex)
    {
        if (skill.modifier[0].effects != null)
        {
            switch (target)
            {
                case 0:
                    ApplyStatusToCharacter(skill.modifier[0]);
                    break;
                case 1:
                    ApplyStatusToEnemy(skill.modifier[0], enemyIndex);
                    break;
                case 2:
                    ApplyStatusToAllEnemy(skill.modifier[0]);
                    break;
                default:
                    break;
            }
        }
    }

    void CheckSkillImmunities(Skill skill, int target, int enemyIndex)
    {
        if (skill.modifier[0].immunities != null)
        {
            switch (target)
            {
                case 0:
                    ApplyImmunityToCharacter(skill.modifier[0]);
                    break;
                case 1:
                    ApplyImmunityToEnemy(skill.modifier[0], enemyIndex);
                    break;
                case 2:
                    ApplyImmunityToAllEnemy(skill.modifier[0]);
                    break;
                default:
                    break;
            }
        }
    }

    void ApplyStatusToCharacter(SkillModifier mod)
    {
        foreach(SkillEffect effect in mod.effects)
        {
            if(Random.Range(1, 101) < effect.chance)
            {
                Debug.Log("Applied " + effect.effect.status + " Status to Player!");
                character.SetStatusEffect(effect.effect, effect.duration);
            }
        }
    }

    void ApplyImmunityToCharacter(SkillModifier mod)
    {
        foreach(SkillImmunity immunity in mod.immunities)
        {
            if (Random.Range(1, 101) < immunity.chance)
            {
                Debug.Log("Applied " + immunity.immuneEffect.status + " Status to Player!");
                character.SetImmunity(immunity.immuneEffect, immunity.chance);
            }
        }
    }

    void ApplyStatusToEnemy(SkillModifier mod, int enemyIndex)
    {
        foreach (SkillEffect effect in mod.effects)
        {
            if (Random.Range(1, 101) < effect.chance)
            {
                Debug.Log("Applied " + effect.effect.status + " Status to " + playerHUD.targetNames[enemyIndex] + "!");
                enemies[enemyIndex].SetStatusEffect(effect.effect, effect.duration);
                Debug.Log("DOT = " + enemies[enemyIndex].damagePercentageOverTime);
            }
        }
    }

    void ApplyStatusToAllEnemy(SkillModifier mod)
    {
        int index = 0;
        
        foreach(Enemy enemy in enemies)
        {
            ApplyStatusToEnemy(mod, index);
        }
    }

    void ApplyImmunityToEnemy(SkillModifier mod, int enemyIndex)
    {
        foreach (SkillImmunity immunity in mod.immunities)
        {
            if (Random.Range(1, 101) < immunity.chance)
            {
                Debug.Log("Applied " + immunity.immuneEffect.status + " Status to " + playerHUD.targetNames[enemyIndex] + "!");
                enemies[enemyIndex].SetImmunity(immunity.immuneEffect, immunity.chance);
            }
        }
    }

    void ApplyImmunityToAllEnemy(SkillModifier mod)
    {
        int index = 0;

        foreach (Enemy enemy in enemies)
        {
            ApplyImmunityToEnemy(mod, index);
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

    void AttackAllEnemy(int roll, float multiplier)
    {
        int enemyIndex = 0;
        int damage = character.Attack(roll);
        damage = Mathf.CeilToInt(damage * multiplier);

        foreach(Enemy enemy in enemies)
        {
            if (CheckEnemyGuardStatus(enemyIndex) == true)
            {
                damage -= enemy.guardValue;

                if (damage <= 0)
                {
                    damage = 0;
                }
            }

            DamageEnemy(enemyIndex, damage);
            enemyIndex++;
        }
    }

    void AttackEnemy(int enemyIndex, int roll, float multiplier)
    {
        int damage = character.Attack(roll);
        damage = Mathf.CeilToInt(damage * multiplier);
        if (CheckEnemyGuardStatus(enemyIndex) == true)
        {
            damage -= enemies[enemyIndex].guardValue;

            if(damage <= 0)
            {
                damage = 0;
            }
        }

        DamageEnemy(enemyIndex, damage);
    }

    void AttackSelf(int roll, float multiplier)
    {
        int damage = character.Attack(roll);
        damage = Mathf.CeilToInt(damage * multiplier);
        character.TakeDamage(damage);
        playerHUD.SetHP(character.currentHP);
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
            ResetSkillTargetHUD();

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
    }

    void ResetSkillTargetHUD()
    {
        playerHUD.DestroySkillTargetButtonHUD(enemyCount);
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
            enemy.CheckStatusEffectDuration();
            EnemyDOT(enemy);
            
            
            if (CheckEnemyAllowAction(enemy) == true)
            {
                StartCoroutine(RandomEnemyAction(enemy));

                if (state == States.Lost)
                {
                    break;
                }
            }
            

            yield return new WaitForSeconds(1f);
        }


        yield return new WaitForSeconds(1f);

        if(state == States.Enemy)
        {
            ChangeStateToPlayer();
        }
    }

    void EnemyDOT(Enemy enemy)
    {
        enemy.TakeDamage(Mathf.CeilToInt(enemy.maxHP * ((float)enemy.damagePercentageOverTime / 100f)));
        enemy.TakeDamage(Mathf.CeilToInt(enemy.maxHP * ((float)enemy.maxHealthLockPercentageOverTime) / 100f));
        enemy.TakeDamage(enemy.damageOverTime);
        enemy.maxHP -= Mathf.CeilToInt(enemy.maxHealthLockPercentageOverTime / 100);
    }

    bool CheckEnemyAllowAction(Enemy enemy)
    {
        if(enemy.allowAction == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckEnemyAllowGuard(Enemy enemy)
    {
        if (enemy.allowGuard == true)
        {
            return true;
        }
        else
        {
            return false;
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
                if (CheckEnemyAllowGuard(enemy) == true)
                {
                    StartCoroutine(EnemyGuard(enemy));
                }
                else
                {
                    StartCoroutine(EnemyAttack(enemy));
                }
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

        if(character.allowGuard == true)
        {
            StartCoroutine(PlayerGuard());
        }
        else
        {
            Debug.Log("Cannot Guard!");
        }
    }

    public void OnEnemySelectButton(int i)
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.indicator.gameObject.SetActive(false);
        }

        ResetEnemyButtonHUD();
        playerHUD.DisplayPlayerTurnHUD();

        StartCoroutine(PlayerAttack(i));
    }

    public void OnEnemySkillSelectButton(int i)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.indicator.gameObject.SetActive(false);
        }

        playerHUD.DisplayPlayerTurnHUD();

        StartCoroutine(PlayerSkill(i, selectSkills.selectedSkill));
    }

    public void OnSelfSkillSelectButton()
    {
        playerHUD.DisplayPlayerTurnHUD();

        StartCoroutine(PlayerSkillSelf(selectSkills.selectedSkill));
    }

    public void OnBackButton()
    {
        state = States.Player;
        ResetEnemyButtonHUD();
        ResetSkillTargetHUD();
        playerHUD.DisplayPlayerTurnHUD();
    }

    public void StageComplete()
    {
        stageCount++;
        //playerHUD.SetStageCount(stageCount);
    }
}
