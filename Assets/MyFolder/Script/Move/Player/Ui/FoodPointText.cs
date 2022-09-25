using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static player;

public class FoodPointText : MonoBehaviour
{
    private int beforeFoodPoints;
    StatusComponentPlayer playerStatus;
    [Header("�����x�\���p�e�L�X�g")] public Text foodPointText;

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
            foodPointText.text = "�����x:" + playerStatus.charFood.showFoodPoint() + " / " + playerStatus.charFood.showMaxFoodPoint();
            beforeFoodPoints = playerStatus.charFood.showFoodPoint();
        }
    }
}
