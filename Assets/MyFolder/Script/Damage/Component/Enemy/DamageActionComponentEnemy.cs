using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentEnemy : DamageActionComponentBase
{
    /**
     * �K�v�ȃC���X�^���X���Z�b�g
     */
    public override void setParam()
    {
        this.damageCalculateObj = new DamageCalculateBase();
    }
}