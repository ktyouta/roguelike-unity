using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : BookBase
{
    [Header("�U���͂̏㏸�l")] public int attackRiseValue;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        GManager.instance.playerAttack += attackRiseValue==0?10: attackRiseValue;
        GManager.instance.wrightLog(name+"���g�p����");
        base.useItem();
    }
}
