using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAllEnemy : BookBase
{
    [Header("ìGÇ…ó^Ç¶ÇÈÉ_ÉÅÅ[ÉWó ")] public int damageValue;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        damageValue = damageValue == 0 ? 10 : damageValue;
    }

    public override void useItem()
    {
        for (var i=0;i<GManager.instance.enemies.Count;i++)
        {
            SpriteRenderer sr = GManager.instance.enemies[i].GetComponent<SpriteRenderer>();
            if (sr.isVisible)
            {
                //GManager.instance.enemies[i].calculateDamage(damageValue);
                GManager.instance.enemies[i].GetComponent<OutAccessComponentBase>()?.callCalculateDamage(damageValue);
            }
        }
        base.useItem();
    }
}
