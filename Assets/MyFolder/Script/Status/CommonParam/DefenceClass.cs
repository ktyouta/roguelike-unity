using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenceClass 
{
    [Header("�L�����̖h���")] public int defence;
    //�L�����̖h��͂Ƒ������ɂ��㏸�l���������h���
    [HideInInspector] public int totalDefence;

    /**
     * �h��͂̉��Z
     */
    public void adddefence(int point)
    {
        defence += point;
    }

    /**
     * totalDefence�̏������p
     */
    public void initializeTotalDefence()
    {
        totalDefence = defence;
    }

    /**
     * totalDefence�̍Đݒ�
     */
    public void settotalDefence(int value)
    {
        totalDefence = defence + value;
    }
}
