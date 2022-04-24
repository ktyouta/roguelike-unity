using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : Consumption
{
    [Header("Õ“Ë‚Ì‰ñ•œ—Ê")] public int recoveryPoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    /**
     * ƒAƒCƒeƒ€‚ªÕ“Ë‚µ‚½ê‡‚Ìˆ—
     */
    public override void collisionItem(Enemy enemy)
    {
        int point = recoveryPoint != 0 ? recoveryPoint : 10;
        enemy.enemyHp += point;
    }
}
