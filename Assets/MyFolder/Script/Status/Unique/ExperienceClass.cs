using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExperienceClass
{
    [Header("キャラのレベル")] public int level;
    [Header("現在の経験値")] public int experience;
    [Header("経験値の上限")] public int maxExperience;

    public void getExperience(int point)
    {
        experience += point;
    }

    public void updateLevel(int point)
    {
        level++;
        experience = point - (maxExperience - experience);
        maxExperience += 50;
    }
}
