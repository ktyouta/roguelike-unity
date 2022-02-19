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

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void useItem()
    {
        ///GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("Equipment");
        //Debug.Log(itemBtns.Length);
        for (int i=0;i<GManager.instance.itemList.Count;i++)
        {
            //Debug.Log(GManager.instance.itemList[i].type);
            if (GManager.instance.itemList[i].type == type)
            {
                if (GManager.instance.itemList[i].id == id)
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
        base.useItem();
    }

    public void changeListPos(int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        for (int i = 0; i < itemBtns.Length; i++)
        {
            if (GManager.instance.itemList[i].type == "Equipment" && GManager.instance.itemList[i].GetType().Name == itemObj.GetComponent<Item>().GetType().Name)
            {
                if (((EquipmentBase)GManager.instance.itemList[i]).isEquip)
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
