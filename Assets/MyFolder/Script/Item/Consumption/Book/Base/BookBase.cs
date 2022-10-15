using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBase : Consumption
{
    [Header("衝突時のダメージ量")] public int damagePoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    /**
     * アイテムが衝突した場合の処理
     */
    public override void collisionItem(Enemy enemy)
    {
        int point = damagePoint != 0 ? damagePoint : 10;
        //enemy.enemyHp += point;
        //対象オブジェクトの回復処理を行う
        OutAccessComponentBase outAccessObj = enemy?.GetComponent<OutAccessComponentBase>();
        if (outAccessObj == null)
        {
            return;
        }
        //回復処理
        outAccessObj.callCalculateRecoveryHp(point, GManager.instance.messageManager.createMessage("2", outAccessObj.statusObj.charName.name, point.ToString()));
    }
}
