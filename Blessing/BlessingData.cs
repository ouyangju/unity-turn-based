using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingData : ScriptableObject
{
    public int id;
    public string blessingName;
    public string blessingDescription;
    public string blessingType;

    //»Øµ÷µã
    public BaseBlessingModel OnCreate;
    public BaseBlessingModel OnBattleBegin;
    public BaseBlessingModel OnSwitchLevel;

}
