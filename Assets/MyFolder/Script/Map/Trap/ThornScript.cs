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
    protected override void stepOnTrap(Collider2D other)
    {
        //GManager.instance.playerHp -= damegePoint;
        OutAccessComponentBase outAccessObj = other.transform.gameObject?.GetComponent<OutAccessComponentBase>();
        //ダメージ処理
        outAccessObj?.callCalculateDamage(damegePoint,
            GManager.instance.messageManager.createMessage("7"));
    }
}
