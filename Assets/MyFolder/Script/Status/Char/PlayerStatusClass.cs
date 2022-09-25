using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatusClass
{
    [Header("�v���C���[��HP")] public int playerHp;
    [Header("�v���C���[�̌��݂̏��HP")] public int nowPlayerMaxHp;
    [Header("�v���C���[�̖��O")] public string playerName;
    [Header("�v���C���[�̏�����")] public int playerMoney;
    [Header("�v���C���[�̍U����")] public int playerAttack;
    [Header("�v���C���[�̖h���")] public int playerDefence;
    [Header("�v���C���[�̃��x��")] public int playerLevel = 1;
    [Header("�v���C���[�̖���")] public int playerCharm = 10;
    [Header("���̃��x���܂ł̌o���l")] public int nowMaxExprience;
    [Header("�A�C�e���̏���������")] public int nowMaxPosession;
    [Header("�v���C���[�̖����x")] public int playerFoodPoint;
    [Header("�v���C���[�̖����x�̏���l")] public int playerMaxFoodPoint = 100;

    /**
     * �_���[�W�v�Z
     */
    public void calculateDamage(int damage)
    {
        playerHp -= damage;
    }

    /**
     * �����x�̌��Z
     */
    public void consumeFoodPoint(int consume)
    {
        playerFoodPoint -= consume;
    }

    /**
     * �����x�̉�
     */
    public void recoveryFoodPoint(int recovery)
    {
        int afterPlayerHp = playerFoodPoint + recovery;
        playerFoodPoint = afterPlayerHp > playerMaxFoodPoint ? playerMaxFoodPoint : afterPlayerHp;
    }

    /**
     * HP�̉�
     */
    public void recoveryHp(int recovery)
    {
        int afterPlayerFoodPoint = playerFoodPoint + recovery;
        playerFoodPoint = afterPlayerFoodPoint > playerMaxFoodPoint ? playerMaxFoodPoint : afterPlayerFoodPoint;
    }

    /**
     * �X�e�[�^�X�̍X�V
     */
    public void updateStatus()
    {
        playerAttack += 2;
        playerDefence += 2;
        playerHp += 10;
        nowPlayerMaxHp += 10;
    }
}
