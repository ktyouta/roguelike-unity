using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    [Header("LΜUΝ")] public int attack;
    //LΜUΝΖυΙζιγΈlπΑ¦½UΝ
    [HideInInspector] public int totalAttack;

    /**
     * UΝΜΑZ
     */
    public void addAttack(int point)
    {
        attack += point;
    }

    /**
     * totalAttackπξlΙί·
     */
    public void initializeTotalAttack()
    {
        totalAttack = attack;
    }

    /**
     * totalAttackΜΔέθ
     */
    public void setTotalAttack(int value)
    {
        totalAttack = attack + value;
    }
}
