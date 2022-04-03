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
        Destroy(itemObj);
        changeListPos(id);
        base.useItem();
        isUsedFlag = true;
        GManager.instance.playersTurn = false;
    }

    public void changeListPos(int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        float beforePos = itemBtns[0].transform.position.y;
        
        for (int i = 0; i < itemBtns.Length; i++)
        {
            if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId + 1 > index)
            {
                Vector3 pos = itemBtns[i].transform.position;
                pos.y = beforePos;
                beforePos = itemBtns[i].transform.position.y;
                itemBtns[i].transform.position = pos;
            }
            else if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId + 1 == index)
            {
                beforePos = itemBtns[i].transform.position.y;
                //GManager.instance.consumeItem(i);
                Destroy(itemBtns[i]);
            }
            else
            {
                beforePos = itemBtns[i].transform.position.y;
            }
        }
    }
}
