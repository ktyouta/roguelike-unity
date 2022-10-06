using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlowBase
{
    DamageCalculateBase damageCal;
    HpClass hp;
    DamageActionComponentBase damageAction;

    public DamageFlowBase(DamageCalculateBase damageCal, HpClass hp, DamageActionComponentBase damageAction)
    {
        this.damageCal = damageCal;
        this.hp = hp;
        this.damageAction = damageAction;
    }

    /**
     * �_���[�W����
     */
    public virtual void damageFlowpPocess(int damage)
    {
        int calDamage = damageCal.calculateDamage(damage);
        int calHp = hp.subHp(calDamage);
        damageAction.reciveDamageAction(calHp);
    }
}
