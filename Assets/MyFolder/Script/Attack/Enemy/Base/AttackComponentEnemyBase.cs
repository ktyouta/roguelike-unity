using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentEnemyBase : AttackComponentBase
{
    [HideInInspector] public StatusComponentEnemy statusObj;

    protected override void Start()
    {
        base.Start();
        // キャストする型をキャラクターごとに変える
        statusObj = (StatusComponentEnemy)GetComponent<StatusComponentBase>();
    }
}
