using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBase : Item
{
    public bool isEquip = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem()
    {
        for (int i=0;i<GManager.instance.itemList.Count;i++)
        {
            if (GManager.instance.itemList[i].GetComponent<Item>().type == type)
            {
                if (GManager.instance.itemList[i].GetComponent<Item>().id == id)
                {
                    if (isEquip)
                    {
                        isEquip = false;
                    }
                    else
                    {
                        isEquip = true;
                    }
                }
                else
                {
                    GManager.instance.itemList[i].GetComponent<EquipmentBase>().isEquip = false;
                }
            }
        }
        changeListPos(id);
    }

    public void changeListPos(int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        for (int i = 0; i < itemBtns.Length; i++)
        {
            if (GManager.instance.itemList[i].GetComponent<Item>().type == "Equipment" && GManager.instance.itemList[i].GetType().Name == this.gameObject.GetComponent<Item>().GetType().Name)
            {
                if (((EquipmentBase)GManager.instance.itemList[i].GetComponent<Item>()).isEquip)
                {
                    itemBtns[i].transform.Find("Text").GetComponent<Text>().text = "E " + itemBtns[i].transform.Find("Text").GetComponent<Text>().text;
                }
                else
                {
                    itemBtns[i].transform.Find("Text").GetComponent<Text>().text = itemBtns[i].transform.Find("Text").GetComponent<Text>().text.Replace("E ", "");
                }
            }
        }
    }
}
