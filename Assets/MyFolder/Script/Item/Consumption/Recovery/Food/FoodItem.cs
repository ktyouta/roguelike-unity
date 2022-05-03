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
        //プレイヤーの満腹度が満タンの場合は回復しない
        if (GManager.instance.playerFoodPoint == GManager.instance.playerMaxFoodPoint)
        {
            GManager.instance.wrightLog("プレイヤーの満腹度が満タンです。");
            return;
        }
        GManager.instance.recoveryFoodPoint(foodPoint);
        base.useItem();
    }
}
