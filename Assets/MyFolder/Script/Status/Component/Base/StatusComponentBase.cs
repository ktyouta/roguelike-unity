using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatusComponentBase : MonoBehaviour
{
    [Header("HP")] public HpClass charHp = new HpClass();
    [Header("–¼‘O")] public NameClass charName = new NameClass();
    [Header("UŒ‚—Í")] public AttackClass charAttack = new AttackClass();
    [Header("–hŒä—Í")] public DefenceClass charDefence = new DefenceClass();

    protected virtual void Start()
    {
        // ‰Šú’l‚ªİ’è‚³‚ê‚Ä‚¢‚Èê‡‚É‰Šú‰»
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