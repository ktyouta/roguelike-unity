using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenceClass 
{
    [Header("ƒLƒƒƒ‰‚Ì–hŒä—Í")] public int defence;
    //ƒLƒƒƒ‰‚Ì–hŒä—Í‚Æ‘•”õ“™‚É‚æ‚éã¸’l‚ğ‰Á‚¦‚½–hŒä—Í
    [HideInInspector] public int totalDefence;

    /**
     * –hŒä—Í‚Ì‰ÁZ
     */
    public void adddefence(int point)
    {
        defence += point;
    }

    /**
     * totalDefence‚Ì‰Šú‰»—p
     */
    public void initializeTotalDefence()
    {
        totalDefence = defence;
    }

    /**
     * totalDefence‚ÌÄİ’è
     */
    public void settotalDefence(int value)
    {
        totalDefence = defence + value;
    }
}
