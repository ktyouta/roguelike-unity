using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentBossEnemy : DamageActionComponentEnemy
{
    // �G�l�~�[���̓���A�N�V����
    protected override void specialAction() {
        BoardManager boardObj = GManager.instance.gameObject.GetComponent<BoardManager>();
        boardObj.LayoutStairsAtRandom(transform.position);
        //GManager.instance.wrightLog("�K�i���o�������B");
        GManager.instance.wrightLog(MessageManager.createMessage("5"));
    }
}
