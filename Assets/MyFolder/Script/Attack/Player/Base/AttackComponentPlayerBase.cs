using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentPlayerBase : AttackComponentBase
{
    [Header("�u���b�L���O���C���[(���L���C���[�ȊO�Ői�s�s�ɂ���������)")] public LayerMask blockingLayer;  //�Փ˂��`�F�b�N����郌�C���[
    [Header("�G���C���[")] public LayerMask enemyLayer;
    [Header("�`�F�X�g���C���[")] public LayerMask treasureLayer;
    [HideInInspector] public StatusComponentPlayer statusObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // �L���X�g����^���L�����N�^�[���Ƃɕς���
        statusObj = (StatusComponentPlayer)GetComponent<StatusComponentBase>();
    }
}
