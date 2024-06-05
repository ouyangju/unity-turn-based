using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EventManager : MonoBehaviour
{
    public AbstractEvent[] events;
    public PlayerDataManager playerDataManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnEventButton()
    {
        int eventindex = Random.Range(0, events.Length); ;
        events[eventindex].ActiveEvent(playerDataManager.playerCurrentUnit[0].GetComponent<CharacterStatus>());
    }

    public void LoadNewScene(string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
    }

    public void LoadBattleScene()
    {
        LoadNewScene("BattleScene");
    }
    public void SetUpevent()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
