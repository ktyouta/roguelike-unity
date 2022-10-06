using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharmClass
{
    [Header("���͓x")] public int charmPoint;
    [Header("���͓x����l")] public int maxCharmPoint;

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
