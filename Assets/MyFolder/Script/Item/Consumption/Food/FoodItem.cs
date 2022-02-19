using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : Consumption
{
    [Header("満腹度回復量")] public int foodPoint;
    public Text foodText;
    // Start is called before the first frame update
    protected override void Start()
    {
        foodText = GameObject.Find("Food").GetComponent<Text>();
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
    //        GManager.instance.addItem(itemObj.GetComponent<FoodItem>());
    //        int itemId = GManager.instance.itemList.Count;
    //        itemObj.GetComponent<FoodItem>().id = itemId;
    //        itemObj.SetActive(false);
    //    }
    //}

    public override void useItem()
    {
        GManager.instance.playerFoodPoints += foodPoint;
        if(foodText == null)
        {
            foodText = GameObject.Find("Food").GetComponent<Text>();
        }
        foodText.text = "Food:" + GManager.instance.playerFoodPoints;
        //Debug.Log(GManager.instance.playerFoodPoints);
        //Debug.Log("回復アイテム" + id);
        //GManager.instance.consumeItem(id-1);
        //changeListPos(id);
        GManager.instance.wrightUseFoodLog(name,GManager.instance.playerName,foodPoint);
        base.useItem();
    }

    //public void changeListPos(int index)
    //{
    //    Debug.Log("index" + index) ;
    //    GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
    //    float beforePos = itemBtns[0].transform.position.y;
    //    //float beforePos;
    //    for (int i=index;i<itemBtns.Length;i++)
    //    {
    //        if (index != 1 && i == index)
    //        {
    //            beforePos = itemBtns[i-1].transform.position.y;
    //        }
    //        Vector3 pos = itemBtns[i].transform.position;
    //        pos.y = beforePos;
    //        beforePos = itemBtns[i].transform.position.y;
    //        itemBtns[i].transform.position = pos;
    //    }
    //}
}
