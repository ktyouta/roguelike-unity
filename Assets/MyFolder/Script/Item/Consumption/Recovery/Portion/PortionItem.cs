using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionItem : RecoveryItem
{
    [Header("HP�񕜗�")] public int hpPoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        StatusComponentPlayer playerStatusObj = (StatusComponentPlayer)GameObject.FindGameObjectWithTag("Player").GetComponent<player>().statusObj;
        //�v���C���[��HP�����^���̏ꍇ�͉񕜂��Ȃ�
        //if (GManager.instance.playerHp == GManager.instance.nowPlayerMaxHp)
        if (playerStatusObj.charHp.showHp() >= playerStatusObj.charHp.showMaxHp())
        {
            GManager.instance.wrightLog("�v���C���[��HP�����^���ł��B");
            return;
        }
        playerStatusObj.charHp.addHp(hpPoint);
        base.useItem();
    }
}
