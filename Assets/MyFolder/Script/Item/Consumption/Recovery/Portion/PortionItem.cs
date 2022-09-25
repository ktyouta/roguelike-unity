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
        StatusComponentPlayer playerStatusObj = (StatusComponentPlayer)GameObject.FindGameObjectWithTag("Player").GetComponent<player>().statusObj;
        //プレイヤーのHPが満タンの場合は回復しない
        //if (GManager.instance.playerHp == GManager.instance.nowPlayerMaxHp)
        if (playerStatusObj.charHp.showHp() >= playerStatusObj.charHp.showMaxHp())
        {
            GManager.instance.wrightLog("プレイヤーのHPが満タンです。");
            return;
        }
        playerStatusObj.charHp.addHp(hpPoint);
        base.useItem();
    }
}
