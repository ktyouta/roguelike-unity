using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    [Header("�L�����̍U����")] public int attack;

    /**
     * �U���͂̉��Z
     */
    public void addAttack(int point)
    {
        attack += point;
    }
}
