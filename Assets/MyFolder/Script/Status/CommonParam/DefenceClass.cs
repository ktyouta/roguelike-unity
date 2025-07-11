using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenceClass 
{
    [Header("キャラの防御力")] public int defence;
    //キャラの防御力と装備等による上昇値を加えた防御力
    [HideInInspector] public int totalDefence;

    /**
     * 防御力の加算
     */
    public void adddefence(int point)
    {
        defence += point;
    }

    /**
     * totalDefenceの初期化用
     */
    public void initializeTotalDefence()
    {
        totalDefence = defence;
    }

    /**
     * totalDefenceの再設定
     */
    public void settotalDefence(int value)
    {
        totalDefence = defence + value;
    }
}
