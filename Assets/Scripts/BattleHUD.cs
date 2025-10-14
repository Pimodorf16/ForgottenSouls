using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Slider hpSlider;

    public void SetHUD(Character c)
    {
        hpSlider.maxValue = c.characterData.maxHP;
        hpSlider.value = c.characterData.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
