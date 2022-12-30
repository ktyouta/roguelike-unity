using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ExperienceClass
{
    [Header("キャラのレベル")] public int level;
    [Header("現在の経験値")] public int experience;
    [Header("経験値の上限")] public int maxExperience;
    [HideInInspector] public HpClass charHp;
    [HideInInspector] public AttackClass charAttack;
    [HideInInspector] public DefenceClass charDefence;

    /**
     * 経験値獲得
     */
    public void addExperience(int point)
    {
        //経験値加算
        while (point > 0)
        {
            int differenceValue = point >= (maxExperience - experience) ? maxExperience - experience :point;
            experience += differenceValue;
            if (experience >= maxExperience)
            {
                updateLevel();
            }
            point -= differenceValue;
        }
    }

    /**
     * レベルアップ
     */
    public void updateLevel()
    {
        level++;
        updateStatus();
        //GManager.instance.wrightLog("レベル" + level +"になった。");
        LogMessageManager.wrightLog(MessageManager.createMessage("4", level.ToString()));
        maxExperience += 50;
        experience = 0;
    }

    /**
     * ステータスの更新
     */
    public void updateStatus()
    {
        charHp.addHp(10);
        charHp.addMaxHp(10);
        charAttack.addAttack(2);
        charDefence.adddefence(2);
    }
}
