using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenceClass 
{
    [Header("�L�����̖h���")] public int defence;

    /**
     * �h��͂̉��Z
     */
    public void adddefence(int point)
    {
        defence += point;
    }

    public int showDefence()
    {
        return defence;
    }
}
