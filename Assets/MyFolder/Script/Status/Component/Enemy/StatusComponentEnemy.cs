using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComponentSettingManager;

public class StatusComponentEnemy : StatusComponentBase
{
    [Header("������")] public WalletClass charWallet = new WalletClass();
    [Header("�o���l������")] public int experience;

    protected override void Start()
    {
        //josn����Y������f�[�^���擾���ăp�����[�^���Z�b�g
        int? enemyId = GetComponent<Enemy>()?.enemyId;
        if (enemyId == null)
        {
            Destroy(gameObject);
        }

        //ID�̈�v����G�̃f�[�^���擾
        EnemyStatusInfoClass enemyStatusInfo = ComponentSettingManager.roguelikeEnemyInfoList
                                            .Where(e => e.id == enemyId).FirstOrDefault()?.status;

        // �f�[�^�̎擾�Ɏ��s�����ꍇ�̓I�u�W�F�N�g���폜����
        if (enemyStatusInfo == null)
        {
            Destroy(gameObject);
        }

        //���O
        charName.name = enemyStatusInfo.name;
        //HP
        charHp.hp = enemyStatusInfo.hp;
        //�U����
        charAttack.totalAttack = enemyStatusInfo.attack;
        //�h���
        charDefence.totalDefence = enemyStatusInfo.defence;
        //������
        charWallet.money = enemyStatusInfo.wallet;
        //�o���l
        experience = enemyStatusInfo.experience;
    }
}
