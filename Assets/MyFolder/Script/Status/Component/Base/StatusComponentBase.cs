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
        charAttack.initializeTotalAttack();
        charDefence.initializeTotalDefence();
    }
}