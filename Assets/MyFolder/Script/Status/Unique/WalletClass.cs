using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalletClass
{
    [Header("所持金")] public int money;
    [Header("所持金上限額")] public int maxMoney = 99999;

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
}
