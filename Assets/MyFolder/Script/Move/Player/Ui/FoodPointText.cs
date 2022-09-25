using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static player;

public class FoodPointText : MonoBehaviour
{
    private int beforeFoodPoints;
    StatusComponentPlayer playerStatus;
    [Header("満腹度表示用テキスト")] public Text foodPointText;

    private void Start()
    {
        foodPointText = GetComponent<Text>();
        playerStatus = (StatusComponentPlayer)GameObject.FindGameObjectWithTag("Player").GetComponent<player>().statusObj;
    }

    // Update is called once per frame
    void Update()
    {
        //if (beforeFoodPoints != GManager.instance.playerFoodPoint)
        if (beforeFoodPoints != playerStatus.charFood.showFoodPoint())
        {
            foodPointText.text = "満腹度:" + playerStatus.charFood.showFoodPoint() + " / " + playerStatus.charFood.showMaxFoodPoint();
            beforeFoodPoints = playerStatus.charFood.showFoodPoint();
        }
    }
}
