using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : EquipmentBase
{

    [Header("攻撃力")] public int attackParam;
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
        string weaponName;
        base.useItem();
        if (isEquip)
        {
            GManager.instance.playerAttack += attackParam;
            weaponName = name;
        }
        else
        {
            GManager.instance.playerAttack -= attackParam;
            weaponName = "なし";
        }
        GManager.instance.weaponName = weaponName;
    }
}
