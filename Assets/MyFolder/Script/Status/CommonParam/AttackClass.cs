using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    [Header("�L�����̍U����")] public int attack;
    //�L�����̍U���͂Ƒ������ɂ��㏸�l���������U����
    [HideInInspector] public int totalAttack;

    /**
     * �U���͂̉��Z
     */
    public void addAttack(int point)
    {
        attack += point;
    }

    /**
     * totalAttack����l�ɖ߂�
     */
    public void initializeTotalAttack()
    {
        totalAttack = attack;
    }

    /**
     * totalAttack�̍Đݒ�
     */
    public void setTotalAttack(int value)
    {
        totalAttack = attack + value;
    }
}
