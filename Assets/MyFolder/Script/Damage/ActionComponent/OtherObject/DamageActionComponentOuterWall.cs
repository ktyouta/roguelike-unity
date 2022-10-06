using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentOuterWall : DamageActionComponentBase
{
    //破壊不可フラグ
    [HideInInspector] public bool isIndestructible;

    public override void reciveDamageAction(int hp)
    {
        if (isIndestructible)
        {
            GManager.instance.wrightLog("破壊不能オブジェクトです。");
            return;
        }
        if (hp <= 0)
        {
            GManager.instance.wrightLog("壁が壊れた。");
            Destroy(this.gameObject);
        }
        else
        {
            GManager.instance.wrightLog("外壁の残り耐久値：" + hp);
        }
    }
}
