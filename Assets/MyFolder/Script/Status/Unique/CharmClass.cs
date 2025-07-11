using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharmClass
{
    [Header("魅力度")] public int charmPoint;
    [Header("魅力度上限値")] public int maxCharmPoint = 100;

    public void addCharmPoint(int point)
    {
        int afterCharmPoint = charmPoint + point;
        charmPoint = afterCharmPoint > maxCharmPoint ? maxCharmPoint : afterCharmPoint;
    }

    public void subCharmPoint(int point)
    {
        int afterCharmPoint = charmPoint - point;
        charmPoint = afterCharmPoint < 0 ? 0 : afterCharmPoint;
    }
}
