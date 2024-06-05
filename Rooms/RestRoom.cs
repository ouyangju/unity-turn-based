using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestRoom : MonoBehaviour
{
    private void Start()
    {
        LevelDataManager.Instance.InitializeUI();
    }

    public void OnRecoverButton()
    {
        if (LevelDataManager.Instance)
        {
            GameObject[] playerCharacters = LevelDataManager.Instance.playerCharacter;
            foreach (GameObject characterGO in playerCharacters)
            {
                if (characterGO != null)
                {
                    Character character = characterGO.GetComponent<Character>();
                    if (character)
                    {
                        character.currentHp += (int)(character.currentHp * 0.3);
                        character.currentHp = Mathf.Min(character.currentHp, character.maxHp);
                    }
                }
            }
        }
    }
}
