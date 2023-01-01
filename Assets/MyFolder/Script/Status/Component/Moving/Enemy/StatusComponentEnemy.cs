using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComponentSettingManager;

public class StatusComponentEnemy : StatusComponentMoving
{
    [Header("������")] public WalletClass charWallet = new WalletClass();
    [Header("�o���l������")] public int experience;

    /**
     * �X�e�[�^�X�����擾
     */
    protected override CommonStatusInfoClass getStatusInfo(int jsonId)
    {
        //ID�̈�v����G�̃f�[�^���擾
        EnemyStatusInfoClass enemyStatusInfo = ComponentSettingManager.roguelikeEnemyInfoList
                                            .Where(e => e.id == jsonId).FirstOrDefault()?.status;

        //�f�[�^�̎擾�Ɏ��s�����ꍇ�͍ŏ��̗v�f���擾����
        if (enemyStatusInfo == null)
        {
            enemyStatusInfo = ComponentSettingManager.roguelikeEnemyInfoList.FirstOrDefault()?.status;
        }

        return enemyStatusInfo;
    }

    /**
     * �X�e�[�^�X�����Z�b�g
     */
    protected override void setParam(CommonStatusInfoClass statusInfo)
    {
        base.setParam(statusInfo);
        //������
        charWallet.money = ((EnemyStatusInfoClass)statusInfo).wallet;
        //�o���l
        experience = ((EnemyStatusInfoClass)statusInfo).experience;
    }
}
