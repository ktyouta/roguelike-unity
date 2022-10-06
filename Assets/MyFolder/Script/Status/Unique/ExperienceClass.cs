using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExperienceClass
{
    [Header("�L�����̃��x��")] public int level;
    [Header("���݂̌o���l")] public int experience;
    [Header("�o���l�̏��")] public int maxExperience;

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
