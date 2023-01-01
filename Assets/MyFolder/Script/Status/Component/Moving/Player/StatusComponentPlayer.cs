using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComponentSettingManager;

[System.Serializable]
public class StatusComponentPlayer : StatusComponentMoving
{
    //������
    [HideInInspector] public WalletClass charWallet = new WalletClass();
    //�����x
    [HideInInspector] public FoodPointClass charFood = new FoodPointClass();
    //�o���l
    [HideInInspector] public ExperienceClass charExperience = new ExperienceClass();
    //���͓x
    [HideInInspector] public CharmClass charCarm = new CharmClass();
    //�������Ă���A�C�e����
    [HideInInspector] public string weaponName = "�Ȃ�";
    [HideInInspector] public string shieldName = "�Ȃ�";

    protected override void Start()
    {
        //2F�ȍ~�̓Z�b�g���Ȃ�
        if (!string.IsNullOrWhiteSpace(charName.name))
        {
            return;
        }

        base.Start();

        //���x���A�b�v���̃X�e�[�^�X�A�b�v�p
        charExperience.charHp = charHp;
        charExperience.charAttack = charAttack;
        charExperience.charDefence = charDefence;
    }

    /**
     * �X�e�[�^�X�����擾
     */
    protected override CommonStatusInfoClass getStatusInfo(int jsonId)
    {
        //ID�̈�v����v���C���[�̃f�[�^���擾
        PlayerStatusInfoClass playerStatusInfo = ComponentSettingManager.roguelikePlayerInfoList
                                            .Where(e => e.id == jsonId).FirstOrDefault()?.status;

        //�f�[�^�̎擾�Ɏ��s�����ꍇ�͍ŏ��̗v�f���擾����
        if (playerStatusInfo == null)
        {
            playerStatusInfo = ComponentSettingManager.roguelikePlayerInfoList.FirstOrDefault()?.status;
        }

        return playerStatusInfo;
    }

    /**
     * �X�e�[�^�X�����Z�b�g
     */
    protected override void setParam(CommonStatusInfoClass statusInfo)
    {
        base.setParam(statusInfo);
        //������
        charWallet.money = ((PlayerStatusInfoClass)statusInfo).wallet;
        //�����x
        charFood.foodPoint = ((PlayerStatusInfoClass)statusInfo).food;
        //���͓x
        charCarm.charmPoint = ((PlayerStatusInfoClass)statusInfo).charm;
    }

    /**
     * �e�X�e�[�^�X��MAX�l�̏����ݒ�
     */
    protected override void initializeStatus()
    {
        base.initializeStatus();
        charFood.initializeMaxFoodPoint();
    }
}
