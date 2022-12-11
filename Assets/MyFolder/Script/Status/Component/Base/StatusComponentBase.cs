using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatusComponentBase : MonoBehaviour
{
    [Header("HP")] public HpClass charHp = new HpClass();
    [Header("���O")] public NameClass charName = new NameClass();
    [Header("�U����")] public AttackClass charAttack = new AttackClass();
    [Header("�h���")] public DefenceClass charDefence = new DefenceClass();

    protected virtual void Start()
    {
        // �����l���ݒ肳��Ă��ȏꍇ�ɏ�����
        if (charHp.maxHp == 0)
        {
            charHp.initializeMaxHp();
        }
        if (string.IsNullOrEmpty(charName.name))
        {
            charName.initializeName();
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