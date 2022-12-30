using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : BookBase
{
    [Header("UŒ‚—Í‚Ìã¸’l")] public int attackRiseValue;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem(StatusComponentBase statuObj)
    {
        //GManager.instance.playerAttack += attackRiseValue==0?10: attackRiseValue;
        statuObj?.charAttack.addAttack(attackRiseValue);
        LogMessageManager.wrightLog(MessageManager.createMessage("12",name));
        base.useItem(statuObj);
    }
}
