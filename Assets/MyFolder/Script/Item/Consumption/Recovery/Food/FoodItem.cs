using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : RecoveryItem
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
}
