using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EquipmentBase
{
    [Header("–hŒä—Í")] public int defenceParam;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem(StatusComponentBase statuObj)
    {
        string shieldName;
        base.useItem(statuObj);
        if (isEquip)
        {
            //GManager.instance.playerDefence += defenceParam;
            statuObj?.charDefence.settotalDefence(defenceParam);
            shieldName = name;
        }
        else
        {
            //GManager.instance.playerDefence -= defenceParam;
            statuObj?.charDefence.initializeTotalDefence();
            shieldName = "‚È‚µ";
        }
        ((StatusComponentPlayer)statuObj).shieldName = shieldName;
    }
}
