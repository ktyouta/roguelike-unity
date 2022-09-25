using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAtionEnemy : DamageActionBase
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
            GManager.instance.wrightDeadLog(statusObj.charName.showName());
            GManager.instance.removeEnemyToList(GetComponent<Enemy>());
            Destroy(gameObject, 0.5f);
        }
    }
}
