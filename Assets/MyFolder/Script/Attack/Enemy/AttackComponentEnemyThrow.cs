using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentEnemyThrow : AttackComponentEnemyBase
{
    ThrowComponentBase throwComponentObj;
    private GameObject throwItem;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gameObject.AddComponent<ThrowComponentEnemy>();
        throwComponentObj = GetComponent<ThrowComponentBase>();
        throwItem = Resources.Load("Prefab/Item/Food") as GameObject;
    }

    /**
    * 攻撃アクション
    */
    public override IEnumerator attackAction(Vector2 start, Vector2 end)
    {
        //攻撃の場合はプレイヤーの行動完了を待つ
        yield return new WaitUntil(() => GManager.instance.isEndPlayerAction);
        int verticalDirection = (int)(end.y - start.y);
        int horizontalDirection = (int)(end.x - start.x);
        //アイテムを投擲する
        yield return StartCoroutine(throwComponentObj.throwObject(verticalDirection, horizontalDirection, throwItem));
        //yield return new WaitForSeconds(0.5f);
        // 行動終了のカウント
        GManager.instance.enemyActionEndCount++;
    }
}
