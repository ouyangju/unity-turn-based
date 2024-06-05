using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour
{
    public Text hpText;
    public Slider hpSlider;

    public void setUI(CharacterStatus characterStatus)
    {
        hpText.text = "HP:" + characterStatus.currentHealth.ToString() + "/" + characterStatus.maxHealth.ToString();
        hpSlider.maxValue = characterStatus.maxHealth;
        hpSlider.value = characterStatus.currentHealth;
    }
}
