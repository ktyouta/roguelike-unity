using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornScript : TrapBase
{
    [Header("棘を踏んだ際のダメージ量")] public int damegePoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        damegePoint = damegePoint == 0 ?10:damegePoint;
    }

    /**
     * トラップを踏んだ際の処理
     */
    protected override void stepOnTrap()
    {
        GManager.instance.playerHp -= damegePoint;
        GManager.instance.wrightLog("棘トラップを踏んだ");
    }
}
