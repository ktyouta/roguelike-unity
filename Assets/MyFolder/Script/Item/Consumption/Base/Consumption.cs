using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumption : Item
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        LogMessageManager.wrightLog(MessageManager.createMessage("12",name));
        deleteSelectedItem(id);
        Destroy(this.gameObject);
    }
}
