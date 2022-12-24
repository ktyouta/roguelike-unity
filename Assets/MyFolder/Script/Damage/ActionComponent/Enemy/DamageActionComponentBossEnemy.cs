using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentBossEnemy : DamageActionComponentEnemy
{
    // エネミー毎の特殊アクション
    protected override void specialAction() {
        BoardManager boardObj = GManager.instance.gameObject.GetComponent<BoardManager>();
        boardObj.LayoutStairsAtRandom(transform.position);
        //GManager.instance.wrightLog("階段が出現した。");
        GManager.instance.wrightLog(MessageManager.createMessage("5"));
    }
}
