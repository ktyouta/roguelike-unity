using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodItem : RecoveryItem
{
    [Header("–ž• “x‰ñ•œ—Ê")] public int foodPoint;
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
        GManager.instance.wrightUseFoodLog(name,GManager.instance.playerName,foodPoint);
        base.useItem();
    }
}
