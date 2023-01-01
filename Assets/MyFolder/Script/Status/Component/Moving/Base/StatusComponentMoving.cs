using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ComponentSettingManager;


public class StatusComponentMoving : StatusComponentBase
{
    //�U����
    [HideInInspector] public AttackClass charAttack = new AttackClass();
    //�h���
    [HideInInspector] public DefenceClass charDefence = new DefenceClass();

    // Start is called before the first frame update
    protected override void Start()
    {
        //�X�e�[�^�X�����擾
        CommonStatusInfoClass statusInfo = getStatusInfo(getJsonId());

        //�X�e�[�^�X�����Z�b�g
        setParam(statusInfo);

        //�e�X�e�[�^�X��MAX�l�̏����ݒ�
        initializeStatus();
    }

    /**
     * Json����f�[�^���擾���邽�߂�ID���擾
     */
    protected int getJsonId()
    {
        int? jsonId = GetComponent<MovingObject>()?.jsonDataId;
        if (jsonId == null)
        {
            Destroy(gameObject);
        }

        return (int)jsonId;
    }

    /**
     * �X�e�[�^�X�����擾
     */
    protected virtual CommonStatusInfoClass getStatusInfo(int jsonId)
    {
        return new CommonStatusInfoClass();
    }

    /**
     * �X�e�[�^�X�����Z�b�g
     */
    protected virtual void setParam(CommonStatusInfoClass statusInfo)
    {
        //���O
        charName.name = statusInfo.name;
        //HP
        charHp.hp = statusInfo.hp;
        //�U����
        charAttack.attack = statusInfo.attack;
        //�h���
        charDefence.defence = statusInfo.defence;
    }

    /**
     * �e�X�e�[�^�X��MAX�l�̏����ݒ�
     */
    protected virtual void initializeStatus()
    {
        charHp.initializeMaxHp();
        charAttack.initializeTotalAttack();
        charDefence.initializeTotalDefence();
    }
}
