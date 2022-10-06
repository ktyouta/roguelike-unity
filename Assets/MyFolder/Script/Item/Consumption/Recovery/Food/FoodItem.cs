using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static player;

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
   
    public override void useItem()
    {
        StatusComponentPlayer playerStatusObj = (StatusComponentPlayer)GameObject.FindGameObjectWithTag("Player").GetComponent<player>().statusObj;
        //プレイヤーの満腹度が満タンの場合は回復しない
        //if (GManager.instance.playerFoodPoint == GManager.instance.playerMaxFoodPoint)
        if (playerStatusObj.charFood.foodPoint >= playerStatusObj.charFood.maxFoodPoint)
        {
            GManager.instance.wrightLog("プレイヤーの満腹度が満タンです。");
            return;
        }
        playerStatusObj.charFood.addFoodPoint(foodPoint);
        base.useItem();
    }
}
