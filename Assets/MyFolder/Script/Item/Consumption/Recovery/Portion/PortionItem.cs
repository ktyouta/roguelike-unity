using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : RecoveryItem
{
    [Header("HP回復量")] public int hpPoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        //プレイヤーのHPが満タンの場合は回復しない
        if (GManager.instance.playerHp == GManager.instance.nowPlayerMaxHp)
        {
            GManager.instance.wrightLog("プレイヤーのHPが満タンです。");
            return;
        }
        GManager.instance.recoveryHp(hpPoint);
        base.useItem();
    }
}
