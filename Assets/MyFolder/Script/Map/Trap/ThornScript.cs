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

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        GManager.instance.playerHp -= damegePoint;
        GManager.instance.wrightLog("棘トラップを踏んだ");
    }
}
