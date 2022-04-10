using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumption : Item
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        Destroy(this.gameObject);
        //changeListPos(id);
        deleteSelectedItem(id);
        base.useItem();
        isUsedFlag = true;
        Destroy(this.gameObject);
        GManager.instance.playersTurn = false;
    }
}
