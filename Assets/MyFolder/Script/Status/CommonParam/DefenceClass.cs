using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefenceClass 
{
    [Header("ƒLƒƒƒ‰‚Ì–hŒä—Í")] public int defence;

    /**
     * –hŒä—Í‚Ì‰ÁŽZ
     */
    public void adddefence(int point)
    {
        defence += point;
    }

    public int showDefence()
    {
        return defence;
    }
}
