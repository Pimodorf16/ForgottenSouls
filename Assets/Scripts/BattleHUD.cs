using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;

    public TextMeshProUGUI diceRoll;

    public void SetHUD(Character c)
    {
        hpSlider.maxValue = c.characterData.maxHP;
        hpSlider.value = c.characterData.currentHP;
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
}
