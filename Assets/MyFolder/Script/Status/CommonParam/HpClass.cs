using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HpClass
{
    [Header("キャラクターのHP")] public int hp;
    [Header("キャラクターの現在の上限HP")] public int maxHp;

    /**
     * HPの加算
     */
    public int addHp(int recovery)
    {
        int afterHp = hp + recovery;
        hp = afterHp > maxHp ? maxHp : afterHp;
        return hp;
    }

    /**
     * HPの減算
     */
    public int subHp(int damage)
    {
        int afterHp = hp - damage;
        hp = afterHp < 0 ? 0 : afterHp;
        return hp;
    }

    /**
     * MAXHPの加算
     */
    public void addMaxHp(int addPoint)
    {
        maxHp += addPoint;
    }

    public int showHp()
    {
        return hp;
    }

    public int showMaxHp()
    {
        return maxHp;
    }
}
