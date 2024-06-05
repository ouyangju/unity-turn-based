using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbstractEvent : ScriptableObject
{

    public string eventName;
    public int eventID;
    public string eventType;//事件的类型，像混沌效应那样吧应该

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
