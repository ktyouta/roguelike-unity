using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EquipmentBase
{
    [Header("防御力")] public int defenceParam;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    //衝突したトリガーのタグがFoodであるか確認してください。
    //    if (other.tag == "Player")
    //    {
    //        //アイテム取得後、非表示
    //        GManager.instance.addItem(itemObj.GetComponent<Sword>());
    //        int itemId = GManager.instance.itemList.Count;
    //        itemObj.GetComponent<Sword>().id = itemId;
    //        //Debug.Log("reid" + itemObj.GetComponent<RecoveryItem>().id);
    //        itemObj.SetActive(false);
    //    }
    //}


    public override void useItem()
    {
        string shieldName;
        base.useItem();
        if (isEquip)
        {
            GManager.instance.playerDefence += defenceParam;
            shieldName = name;
        }
        else
        {
            GManager.instance.playerDefence -= defenceParam;
            shieldName = "なし";
        }
        GManager.instance.shieldName = shieldName;
    }
}
