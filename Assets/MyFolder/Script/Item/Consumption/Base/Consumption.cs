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

    public override void useItem(StatusComponentBase statusObj)
    {
        LogMessageManager.wrightLog(MessageManager.createMessage("12", statusObj.charName.name,name));
        deleteSelectedItem(id);
        Destroy(this.gameObject);
    }
}
