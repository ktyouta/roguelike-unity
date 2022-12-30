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
        StatusComponentPlayer playerStatusObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
        //�v���C���[��HP�����^���̏ꍇ�͉񕜂��Ȃ�
        //if (GManager.instance.playerHp == GManager.instance.nowPlayerMaxHp)
        if (playerStatusObj.charHp.hp >= playerStatusObj.charHp.maxHp)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("11"));
            return;
        }
        playerStatusObj.charHp.addHp(hpPoint);
        base.useItem();
    }
}
