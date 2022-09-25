using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionBossEnemy : DamageAtionEnemy
{
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
            BoardManager boardObj = GManager.instance.gameObject.GetComponent<BoardManager>();
            boardObj.LayoutStairsAtRandom(transform.position);
            Destroy(gameObject, 0.5f);
        }
    }
}
