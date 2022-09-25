using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HpClass
{
    [Header("�L�����N�^�[��HP")] public int hp;
    [Header("�L�����N�^�[�̌��݂̏��HP")] public int maxHp;

    /**
     * HP�̉��Z
     */
    public int addHp(int recovery)
    {
        int afterHp = hp + recovery;
        hp = afterHp > maxHp ? maxHp : afterHp;
        return hp;
    }

    /**
     * HP�̌��Z
     */
    public int subHp(int damage)
    {
        int afterHp = hp - damage;
        hp = afterHp < 0 ? 0 : afterHp;
        return hp;
    }

    /**
     * MAXHP�̉��Z
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
