using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentPlayer : DamageActionComponentBase
{
    protected bool isDefeat = false;

    public override void reciveDamageAction(int hp)
    {
        if (isDefeat)
        {
            return;
        }
        if (hp <= 0)
        {
            isDefeat = true;
            //GManager.instance.wrightDeadLog(statusObj.charName.name);
            LogMessageManager.wrightLog(MessageManager.createMessage("6",statusObj.charName.name));
        }
    }
}
