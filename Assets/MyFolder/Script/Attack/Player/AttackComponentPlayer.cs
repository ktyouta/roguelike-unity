using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentPlayer : AttackComponentPlayerBase
{
    /**
    * 攻撃アクション
    */
    public override IEnumerator attackAction(Vector2 start, Vector2 end)
    {
        animator.Play("PlayerAttack");
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer | blockingLayer);
        //攻撃の場合は現在地を追加
        GManager.instance.enemyNextPosition.Add(transform.position);
        //時間差で実行されるフラグをオンにする処理
        yield return new WaitForSeconds(0.3f);
        GManager.instance.isEndPlayerAction = true;
        GManager.instance.playersTurn = false;

        //対象オブジェクトのダメージ処理を行う
        OutAccessComponentBase outAccessObj = hit.transform?.gameObject?.GetComponent<OutAccessComponentBase>();
        //ダメージ処理
        outAccessObj?.callCalculateDamage(statusObj.charAttack.totalAttack,
            GManager.instance.messageManager.createMessage("1", statusObj.charName.name, outAccessObj.statusObj.charName.name, statusObj.charAttack.totalAttack.ToString()));
    }
}
