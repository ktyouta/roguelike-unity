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
        base.Start();
        foodText = GameObject.Find("Food").GetComponent<Text>();
    }

    public override void useItem(StatusComponentMoving statusObj)
    {
        //プレイヤーの満腹度が満タンの場合は回復しない
        if (((StatusComponentPlayer)statusObj).charFood.foodPoint >= ((StatusComponentPlayer)statusObj).charFood.maxFoodPoint)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("11", statusObj.charName.name));
            return;
        }
        ((StatusComponentPlayer)statusObj).charFood.addFoodPoint(foodPoint);
        base.useItem(statusObj);
    }
}
