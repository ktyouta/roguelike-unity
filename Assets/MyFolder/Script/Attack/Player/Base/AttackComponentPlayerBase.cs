using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentPlayerBase : AttackComponentBase
{
    [Header("ブロッキングレイヤー(下記レイヤー以外で進行不可にしたいもの)")] public LayerMask blockingLayer;  //衝突がチェックされるレイヤー
    [Header("敵レイヤー")] public LayerMask enemyLayer;
    [Header("チェストレイヤー")] public LayerMask treasureLayer;
    [HideInInspector] public StatusComponentPlayer statusObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // キャストする型をキャラクターごとに変える
        statusObj = (StatusComponentPlayer)GetComponent<StatusComponentBase>();
    }
}
