using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusBase
{
    [Header("HP")] public HpClass charHp;
    [Header("名前")] public NameClass charName;
    [Header("攻撃力")] public AttackClass charAttack;
    [Header("防御力")] public DefenceClass charDefence;

    /**
     * ステータスの更新
     */
    public void updateStatus()
    {
        charHp.addHp(10);
        charHp.addMaxHp(10);
        charAttack.addAttack(2);
        charDefence.adddefence(2);
    }
}