using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static player;

public class FoodItem : RecoveryItem
{
    [Header("�����x�񕜗�")] public int foodPoint;
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
        //�v���C���[�̖����x�����^���̏ꍇ�͉񕜂��Ȃ�
        //if (GManager.instance.playerFoodPoint == GManager.instance.playerMaxFoodPoint)
        if (playerStatusObj.charFood.foodPoint >= playerStatusObj.charFood.maxFoodPoint)
        {
            GManager.instance.wrightLog("�v���C���[�̖����x�����^���ł��B");
            return;
        }
        playerStatusObj.charFood.addFoodPoint(foodPoint);
        base.useItem();
    }
}
