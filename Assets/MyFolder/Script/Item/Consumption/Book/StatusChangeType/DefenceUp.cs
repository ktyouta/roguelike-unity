using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUp : BookBase
{
    [Header("�h��͂̏㏸�l")] public int defenceRiseValue;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        GManager.instance.playerAttack += defenceRiseValue==0?10: defenceRiseValue;
        GManager.instance.wrightLog(name + "���g�p����");
        base.useItem();
    }
}
