using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : Consumption
{
    [Header("HP回復量")] public int hpPoint;
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
    //        GManager.instance.addItem(itemObj.GetComponent<RecoveryItem>());
    //        int itemId = GManager.instance.itemList.Count;
    //        itemObj.GetComponent<RecoveryItem>().id = itemId;
    //        //Debug.Log("reid" + itemObj.GetComponent<RecoveryItem>().id);
    //        itemObj.SetActive(false);
    //    }
    //}

    public override void useItem()
    {
        GManager.instance.playerHp += hpPoint;
        //Debug.Log("回復アイテム"+id);
        //changeListPos(id);
        base.useItem();
    }

    //public void changeListPos(int index)
    //{
    //    Debug.Log("index" + index);
    //    GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
    //    float beforePos = itemBtns[0].transform.position.y;
    //    //float beforePos;
    //    for (int i = index; i < itemBtns.Length; i++)
    //    {
    //        if (index != 1 && i == index)
    //        {
    //            beforePos = itemBtns[i - 1].transform.position.y;
    //        }
    //        Vector3 pos = itemBtns[i].transform.position;
    //        pos.y = beforePos;
    //        beforePos = itemBtns[i].transform.position.y;
    //        itemBtns[i].transform.position = pos;
    //    }
    //}
}
