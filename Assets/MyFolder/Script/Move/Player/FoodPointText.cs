using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodPointText : MonoBehaviour
{
    private int beforeFoodPoints;
    [Header("満腹度表示用テキスト")] public Text foodPointText;

    private void Start()
    {
        foodPointText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeFoodPoints != GManager.instance.playerFoodPoint)
        {
            foodPointText.text = "満腹度:" + GManager.instance.playerFoodPoint + " / " + GManager.instance.playerMaxFoodPoint;
            beforeFoodPoints = GManager.instance.playerFoodPoint;
        }
    }
}
