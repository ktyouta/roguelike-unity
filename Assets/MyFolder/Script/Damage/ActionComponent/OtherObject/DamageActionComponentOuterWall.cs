using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentOuterWall : DamageActionComponentBase
{
    //”j‰ó•s‰Âƒtƒ‰ƒO
    [HideInInspector] public bool isIndestructible;

    public override void reciveDamageAction(int hp)
    {
        if (isIndestructible)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("13"));
            return;
        }
        if (hp <= 0)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("14"));
            Destroy(this.gameObject);
        }
        else
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("15",$"{hp}"));
        }
    }
}
