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

    public override void useItem(StatusComponentBase statusObj)
    {
        //�v���C���[��HP�����^���̏ꍇ�͉񕜂��Ȃ�
        if (statusObj.charHp.hp >= statusObj.charHp.maxHp)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("20",statusObj.charName.name));
            return;
        }
        statusObj.charHp.addHp(hpPoint);
        base.useItem(statusObj);
    }
}
