using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverManager : MonoBehaviour
{
    public static RecoverManager instance;

    public void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DoRecover(RecoverInfo recoverInfo)
    {
        BuffHandler userBuffHandler = recoverInfo.user?.GetComponent<BuffHandler>();
        BuffHandler targetBuffHandler = recoverInfo.target?.GetComponent<BuffHandler>();

        if (userBuffHandler)
        {
            foreach (var buffInfo in userBuffHandler.buffList)
            {
                buffInfo.buffData.OnHeal?.Apply(buffInfo, null, recoverInfo);
            }
        }
        if (targetBuffHandler)
        {
            foreach (var buffInfo in targetBuffHandler.buffList)
            {
                buffInfo.buffData.OnBeHeal?.Apply(buffInfo, null, recoverInfo);
            }
        }
    }
}
