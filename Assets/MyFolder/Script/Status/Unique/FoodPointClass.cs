using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodPointClass
{
    [Header(" x")] public int foodPoint;
    [Header(" xΜγΐl")] public int maxFoodPoint;

    /**
     *  xΜΑZ
     */
    public void addFoodPoint(int point)
    {
        int afterFoodPoint = foodPoint + point;
        foodPoint = afterFoodPoint > maxFoodPoint ? maxFoodPoint : afterFoodPoint;
    }

    /**
     *  xΜΈZ
     */
    public void subFoodPoint(int point)
    {
        int afterFoodPoint = foodPoint - point;
        foodPoint = afterFoodPoint < 0 ? 0 : afterFoodPoint;
    }

    /**
     *  xΜγΐlΜΑZ
     */
    public void addMaxFoodPoint(int point)
    {
        maxFoodPoint += point;
    }

    /**
     * maxFoodPointπξlΙί·
     */
    public void initializeMaxFoodPoint()
    {
        maxFoodPoint = foodPoint;
    }
}
