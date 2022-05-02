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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void useItem()
    {
        for (var i=0;i<GManager.instance.enemies.Count;i++)
        {
            SpriteRenderer sr = GManager.instance.enemies[i].GetComponent<SpriteRenderer>();
            if (sr.isVisible)
            {
                GManager.instance.enemies[i].enemyHp -= damageValue == 0 ?10:20;
            }
        }
        base.useItem();
    }
}
