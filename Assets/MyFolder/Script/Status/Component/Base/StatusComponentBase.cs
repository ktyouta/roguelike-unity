using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatusComponentBase : MonoBehaviour
{
    [Header("HP")] public HpClass charHp;
    [Header("���O")] public NameClass charName;
    [Header("�U����")] public AttackClass charAttack;
    [Header("�h���")] public DefenceClass charDefence;

    protected virtual void Start()
    {
        // �����l���ݒ肳��Ă��ȏꍇ�ɏ�����
        if (charHp.maxHp == 0)
        {
            charHp.initializeMaxHp();
        }
        if (charAttack.totalAttack == 0)
        {
            charAttack.initializeTotalAttack();
        }
        if (charDefence.totalDefence == 0)
        {
            charDefence.initializeTotalDefence();
        }
    }
}