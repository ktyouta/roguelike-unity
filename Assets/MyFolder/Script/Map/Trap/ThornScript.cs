using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornScript : TrapBase
{
    [Header("���𓥂񂾍ۂ̃_���[�W��")] public int damegePoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        damegePoint = damegePoint == 0 ?10:damegePoint;
    }

    /**
     * �g���b�v�𓥂񂾍ۂ̏���
     */
    protected override void stepOnTrap()
    {
        GManager.instance.playerHp -= damegePoint;
        GManager.instance.wrightLog("���g���b�v�𓥂�");
    }
}
