using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatusComponentBase : MonoBehaviour
{
    [Header("HP")] public HpClass charHp;
    [Header("–¼‘O")] public NameClass charName;
    [Header("UŒ‚—Í")] public AttackClass charAttack;
    [Header("–hŒä—Í")] public DefenceClass charDefence;

    protected virtual void Start()
    {
        charAttack.initializeTotalAttack();
        charDefence.initializeTotalDefence();
    }
}