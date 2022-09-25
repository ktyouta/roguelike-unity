using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusBase
{
    [Header("HP")] public HpClass charHp;
    [Header("���O")] public NameClass charName;
    [Header("�U����")] public AttackClass charAttack;
    [Header("�h���")] public DefenceClass charDefence;

    /**
     * �X�e�[�^�X�̍X�V
     */
    public void updateStatus()
    {
        charHp.addHp(10);
        charHp.addMaxHp(10);
        charAttack.addAttack(2);
        charDefence.adddefence(2);
    }
}