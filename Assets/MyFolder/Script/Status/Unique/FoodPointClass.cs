using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodPointClass
{
    [Header("�����x")] public int foodPoint;
    [Header("�����x�̏���l")] public int maxFoodPoint;

    /**
     * �����x�̉��Z
     */
    public void addFoodPoint(int point)
    {
        int afterFoodPoint = foodPoint + point;
        foodPoint = afterFoodPoint > maxFoodPoint ? maxFoodPoint : afterFoodPoint;
    }

    /**
     * �����x�̌��Z
     */
    public void subFoodPoint(int point)
    {
        int afterFoodPoint = foodPoint - point;
        foodPoint = afterFoodPoint < 0 ? 0 : afterFoodPoint;
    }

    /**
     * �����x�̏���l�̉��Z
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
