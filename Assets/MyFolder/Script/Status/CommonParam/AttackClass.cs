using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    [Header("ƒLƒƒƒ‰‚ÌUŒ‚—Í")] public int attack;
    //ƒLƒƒƒ‰‚ÌUŒ‚—Í‚Æ‘•”õ“™‚É‚æ‚éã¸’l‚ğ‰Á‚¦‚½UŒ‚—Í
    [HideInInspector] public int totalAttack;

    /**
     * UŒ‚—Í‚Ì‰ÁZ
     */
    public void addAttack(int point)
    {
        attack += point;
    }

    /**
     * totalAttack‚ğŠî€’l‚É–ß‚·
     */
    public void initializeTotalAttack()
    {
        totalAttack = attack;
    }

    /**
     * totalAttack‚ÌÄİ’è
     */
    public void setTotalAttack(int value)
    {
        totalAttack = attack + value;
    }
}
