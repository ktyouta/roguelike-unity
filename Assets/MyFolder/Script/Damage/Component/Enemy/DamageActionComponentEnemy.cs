using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentEnemy : DamageActionComponentBase
{
    /**
     * 必要なインスタンスをセット
     */
    public override void setParam()
    {
        this.damageCalculateObj = new DamageCalculateBase();
    }
}