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
    protected override void stepOnTrap(Collider2D other)
    {
        //GManager.instance.playerHp -= damegePoint;
        OutAccessComponentBase outAccessObj = other.transform.gameObject?.GetComponent<OutAccessComponentBase>();
        //�_���[�W����
        outAccessObj?.callCalculateDamage(damegePoint,
            GManager.instance.messageManager.createMessage("7"));
    }
}
