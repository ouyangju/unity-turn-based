using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text battleDialogue;

    public Text skillText1;
    public Text skillText2;
    public Text skillText3;
    public void SetBattleHud(CharacterStatus characterStatus)
    {
        skillText1.text = characterStatus.characterSkill[0].skillsName;
        skillText2.text = characterStatus.characterSkill[1].skillsName;
        skillText3.text = characterStatus.characterSkill[2].skillsName;
    }
}
