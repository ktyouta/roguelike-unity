using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalletClass
{
    [Header("������")] public int money;
    [Header("����������z")] public int maxMoney;

    public void addMoney(int point)
    {
        int afterMoney = money + point;
        money = afterMoney > maxMoney ? maxMoney : afterMoney;
    }

    public void subMoney(int point)
    {
        int afterMoney = money - point;
        money = afterMoney < 0 ? 0 : afterMoney;
    }

    public int showMoney()
    {
        return money;
    }
}
