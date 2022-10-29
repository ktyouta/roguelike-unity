using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentEnemy : AttackComponentEnemyBase
{
    /**
    * 攻撃アクション
    */
    public override IEnumerator attackAction(Vector2 start, Vector2 end)
    {
        //攻撃の場合はプレイヤーの行動完了を待つ
        yield return new WaitUntil(() => GManager.instance.isEndPlayerAction);
        RaycastHit2D hit = Physics2D.Linecast(start, end, playerLayer);
        animator.Play("EnemyAttack");

        //対象オブジェクトのダメージ処理を行う
        OutAccessComponentBase outAccessObj = hit.transform?.gameObject?.GetComponent<OutAccessComponentBase>();
        //ダメージ処理
        outAccessObj?.callCalculateDamage(statusObj.charAttack.totalAttack, GManager.instance.messageManager.createMessage("1", statusObj.charName.name, outAccessObj.statusObj.charName.name, statusObj.charAttack.totalAttack.ToString()));
        yield return new WaitForSeconds(0.5f);
        // 行動終了のカウント
        GManager.instance.enemyActionEndCount++;
    }
}
