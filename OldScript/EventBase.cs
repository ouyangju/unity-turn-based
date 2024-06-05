using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventBase : AbstractEvent
{
    public AbstractSkill skillPrefab;
    public override void ActiveEvent(CharacterStatus characterStatus)
    {
        characterStatus.AddSkill(skillPrefab);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
