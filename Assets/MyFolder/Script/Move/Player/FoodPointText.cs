using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodPointText : MonoBehaviour
{
    private int beforeFoodPoints;
    [Header("�����x�\���p�e�L�X�g")] public Text foodPointText;

    private void Start()
    {
        foodPointText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeFoodPoints != GManager.instance.playerFoodPoint)
        {
            foodPointText.text = "�����x:" + GManager.instance.playerFoodPoint + " / " + GManager.instance.playerMaxFoodPoint;
            beforeFoodPoints = GManager.instance.playerFoodPoint;
        }
    }
}
