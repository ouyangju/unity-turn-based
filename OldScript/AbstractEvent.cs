using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbstractEvent : ScriptableObject
{

    public string eventName;
    public int eventID;
    public string eventType;//�¼������ͣ������ЧӦ������Ӧ��

    public virtual void ActiveEvent(CharacterStatus characterStatus) { }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
