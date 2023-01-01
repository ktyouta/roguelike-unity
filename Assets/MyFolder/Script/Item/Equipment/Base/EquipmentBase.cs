using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBase : Item
{
    [Header("衝突時のダメージ量")] public int damagePoint;
    public bool isEquip = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void useItem(StatusComponentMoving statusObj)
    {
        for (int i=0;i< ItemManager.itemList.Count;i++)
        {
            if (ItemManager.itemList[i].GetComponent<Item>().type == type)
            {
                if (ItemManager.itemList[i].GetComponent<Item>().id == id)
                {
                    if (isEquip)
                    {
                        //装備を外す
                        isEquip = false;
                        LogMessageManager.wrightLog(MessageManager.createMessage("19", statusObj.charName.name, name));
                    }
                    else
                    {
                        //アイテムを装備する
                        isEquip = true;
                        LogMessageManager.wrightLog(MessageManager.createMessage("18", statusObj.charName.name, name));
                    }
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
            if (ItemManager.itemList[i].GetComponent<Item>().type.ToString() == "Equipment" && ItemManager.itemList[i].GetType().Name == this.gameObject.GetComponent<Item>().GetType().Name)
            {
                if (((EquipmentBase)ItemManager.itemList[i].GetComponent<Item>()).isEquip)
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

    /**
     * アイテムが衝突した場合の処理
     */
    public override void collisionItem(GameObject targetObj)
    {
        int point = damagePoint != 0 ? damagePoint :10;
        //enemy.enemyHp -= point;
        //対象オブジェクトのダメージ処理を行う
        OutAccessComponentBase outAccessObj = targetObj?.GetComponent<OutAccessComponentBase>();
        //ダメージ処理
        outAccessObj?.callCalculateDamage(point);
    }
}
