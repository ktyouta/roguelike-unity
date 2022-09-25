using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodPointClass
{
    [Header("満腹度")] public int foodPoint;
    [Header("満腹度の上限値")] public int maxFoodPoint;

    /**
     * 満腹度の加算
     */
    public void addFoodPoint(int point)
    {
        int afterFoodPoint = foodPoint + point;
        foodPoint = afterFoodPoint > maxFoodPoint ? maxFoodPoint : afterFoodPoint;
    }

    /**
     * 満腹度の減算
     */
    public void subFoodPoint(int point)
    {
        int afterFoodPoint = foodPoint - point;
        foodPoint = afterFoodPoint < 0 ? 0 : afterFoodPoint;
    }

    /**
     * 満腹度の上限値の加算
     */
    public void addMaxFoodPoint(int point)
    {
        maxFoodPoint += point;
    }

    public int showFoodPoint()
    {
        return foodPoint;
    }

    public int showMaxFoodPoint()
    {
        return maxFoodPoint;
    }
}
