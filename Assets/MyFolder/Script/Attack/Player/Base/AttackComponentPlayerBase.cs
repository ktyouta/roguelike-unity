using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentPlayerBase : AttackComponentBase
{
    [HideInInspector] public StatusComponentPlayer statusObj;
    protected player playerObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // キャストする型をキャラクターごとに変える
        statusObj = GetComponent<StatusComponentPlayer>();
        playerObj = GetComponent<player>();
    }
}
