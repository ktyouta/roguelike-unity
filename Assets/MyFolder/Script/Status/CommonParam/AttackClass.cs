using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    [Header("ƒLƒƒƒ‰‚ÌUŒ‚—Í")] public int attack;

    /**
     * UŒ‚—Í‚Ì‰ÁZ
     */
    public void addAttack(int point)
    {
        attack += point;
    }
}
