using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentEnemyBase : AttackComponentBase
{
    [Header("�v���C���[���C���[")] public LayerMask playerLayer;
    [HideInInspector] public StatusComponentEnemy statusObj;

    protected override void Start()
    {
        base.Start();
        // �L���X�g����^���L�����N�^�[���Ƃɕς���
        statusObj = (StatusComponentEnemy)GetComponent<StatusComponentBase>();
    }
}
