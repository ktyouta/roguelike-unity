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
        //�v���C���[��HP�����^���̏ꍇ�͉񕜂��Ȃ�
        if (GManager.instance.playerHp == GManager.instance.nowPlayerMaxHp)
        {
            GManager.instance.wrightLog("�v���C���[��HP�����^���ł��B");
            return;
        }
        GManager.instance.recoveryHp(hpPoint);
        base.useItem();
    }
}
