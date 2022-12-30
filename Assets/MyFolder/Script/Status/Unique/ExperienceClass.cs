using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ExperienceClass
{
    [Header("�L�����̃��x��")] public int level;
    [Header("���݂̌o���l")] public int experience;
    [Header("�o���l�̏��")] public int maxExperience;
    [HideInInspector] public HpClass charHp;
    [HideInInspector] public AttackClass charAttack;
    [HideInInspector] public DefenceClass charDefence;

    /**
     * �o���l�l��
     */
    public void addExperience(int point)
    {
        //�o���l���Z
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
     * ���x���A�b�v
     */
    public void updateLevel()
    {
        level++;
        updateStatus();
        //GManager.instance.wrightLog("���x��" + level +"�ɂȂ����B");
        LogMessageManager.wrightLog(MessageManager.createMessage("4", level.ToString()));
        maxExperience += 50;
        experience = 0;
    }

    /**
     * �X�e�[�^�X�̍X�V
     */
    public void updateStatus()
    {
        charHp.addHp(10);
        charHp.addMaxHp(10);
        charAttack.addAttack(2);
        charDefence.adddefence(2);
    }
}
