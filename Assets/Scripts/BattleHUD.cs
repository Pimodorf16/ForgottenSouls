using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;

public class BattleHUD : MonoBehaviour
{
    [Header("BattleManager")]
    public BattleManager battleManager;
    
    [Header("Player UI")]
    public GameObject playerTurn;

    [Header("Display UI")]
    public GameObject display;
    public TextMeshProUGUI diceRoll;
    public Slider hpSlider;
    public Slider mpSlider;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI waveCountText;
    public TextMeshProUGUI stageCountText;

    [Header("Targeting UI")]
    public GameObject targeting;
    public GameObject gridLayoutEnemy;
    public GameObject enemyButtonPrefab;
    public List<string> targetNames = new List<string>();
    public bool createdTargets = false;
    public bool originalTargets = false;
    public List<GameObject> enemyButton = new List<GameObject>();

    [Header("Skill Selection UI")]
    public SelectSkills selectSkills;
    public GameObject skillSelection;
    public GameObject gridLayoutSkill;
    public GameObject skillButtonPrefab;

    [Header("Skill Targeting UI")]
    public GameObject skillTargeting;
    public GameObject gridLayoutSkillTarget;
    public GameObject playerButtonPrefab;
    public List<string> targetSkillNames = new List<string>();
    public bool createdSkillTargets = false;
    public bool originalSkillTargets = false;
    public List<GameObject> enemySkillButton = new List<GameObject>();

    public void SetHUD(Character c)
    {
        hpSlider.maxValue = c.characterData.maxHP;
        hpSlider.value = c.characterData.currentHP;
        mpSlider.maxValue = c.characterData.maxMP;
        mpSlider.value = c.characterData.currentMP;
    }

    public IEnumerator SetDiceRoll(int roll)
    {
        diceRoll.gameObject.SetActive(true);

        diceRoll.text = roll.ToString();

        yield return new WaitForSeconds(2f);

        diceRoll.gameObject.SetActive(false);
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetMP(int mp)
    {
        mpSlider.value = mp;
    }

    public void SetEnemyCount(int count)
    {
        enemyCountText.text = "Enemy: " + count;
    }

    public void SetWaveCount(int currentWave,int waveCount)
    {
        waveCountText.text = "Wave: " + currentWave + " / " + waveCount;
    }

    public void SetStageCount(int stageCount)
    {
        stageCountText.text = "Stage: " + stageCount;
    }

    public void DisplaySkillSelectionHUD()
    {
        playerTurn.SetActive(false);
        skillSelection.SetActive(true);
    }

    public void DisplayTargetHUD(int enemyCount)
    {
        playerTurn.SetActive(false);
        targeting.SetActive(true);

        if (createdTargets == false)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                CreateEnemyButton(i);
            }
        }else if(createdTargets == true)
        {
            return;
        }

        originalTargets = true;
    }

    public void DisplaySkillTargetHUD(int enemyCount)
    {
        skillSelection.SetActive(false);
        skillTargeting.SetActive(true);

        if(createdSkillTargets == false)
        {
            for(int i = 0; i < enemyCount; i++)
            {
                CreateEnemySkillButton(i);
            }
        }else if(createdSkillTargets== true)
        {
            return;
        }

        originalSkillTargets = true;
    }

    public void DestroyTargetButtonHUD(int enemyCount)
    {
        for(int i = enemyCount; i > 0; i--)
        {
            Destroy(enemyButton[i - 1]);
            enemyButton.RemoveAt(i - 1);
        }
    }

    public void DisplayPlayerTurnHUD()
    {
        playerTurn.SetActive(true);
        targeting.SetActive(false);
        skillSelection.SetActive(false);
        skillTargeting.SetActive(false);
    }
    public void CreateEnemySkillButton(int i)
    {
        createdSkillTargets = true;

        GameObject newEnemySkillButton = Instantiate(enemyButtonPrefab);
        enemyButton.Add(newEnemySkillButton);
        EventTrigger trigger = newEnemySkillButton.GetComponent<EventTrigger>();
        newEnemySkillButton.transform.SetParent(gridLayoutSkillTarget.transform);
        newEnemySkillButton.transform.localScale = new Vector3(1, 1, 1);

        newEnemySkillButton.GetComponent<Button>().onClick.AddListener(() => battleManager.OnEnemySkillSelectButton(i));

        AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, (eventData) => OnPointerEnterFunction(eventData, i));
        AddEventTriggerListener(trigger, EventTriggerType.PointerExit, (eventData) => OnPointerExitFunction(eventData, i));


        TextMeshProUGUI textComponent = newEnemySkillButton.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            if (originalSkillTargets == false)
            {
                targetNames[i] = targetNames[i] + " " + (i + 1);
            }

            textComponent.text = targetNames[i];
        }
    }

    public void CreateEnemyButton(int i)
    {
        createdTargets = true;
        
        GameObject newEnemyButton = Instantiate(enemyButtonPrefab);
        enemyButton.Add(newEnemyButton);
        EventTrigger trigger = newEnemyButton.GetComponent<EventTrigger>();
        newEnemyButton.transform.SetParent(gridLayoutEnemy.transform);
        newEnemyButton.transform.localScale = new Vector3(1, 1, 1);

        newEnemyButton.GetComponent<Button>().onClick.AddListener(() => battleManager.OnEnemySelectButton(i));

        AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, (eventData) => OnPointerEnterFunction(eventData, i));
        AddEventTriggerListener(trigger, EventTriggerType.PointerExit, (eventData) => OnPointerExitFunction(eventData, i));


        TextMeshProUGUI textComponent = newEnemyButton.GetComponentInChildren<TextMeshProUGUI>();
        if(textComponent != null )
        {
            if(originalTargets == false)
            {
                targetNames[i] = targetNames[i] + " " + (i + 1);
            }

            textComponent.text = targetNames[i];
        }
    }

    private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> listener)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(listener);
        trigger.triggers.Add(entry);
    }

    private void OnPointerEnterFunction(BaseEventData eventData, int i)
    {
        battleManager.enemies[i].indicator.gameObject.SetActive(true);
    }

    private void OnPointerExitFunction(BaseEventData eventData, int i)
    {
        battleManager.enemies[i].indicator.gameObject.SetActive(false);
    }
}
