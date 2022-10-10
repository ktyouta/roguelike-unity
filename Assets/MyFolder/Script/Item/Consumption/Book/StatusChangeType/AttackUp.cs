using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : BookBase
{
    [Header("攻撃力の上昇値")] public int attackRiseValue;
    [SerializeField,Header("キャラのステータス用コンポーネント")] private StatusComponentBase statusComponentObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    public override void useItem()
    {
        //GManager.instance.playerAttack += attackRiseValue==0?10: attackRiseValue;
        statusComponentObj?.charAttack.addAttack(attackRiseValue);
        GManager.instance.wrightLog(name+"を使用した");
        base.useItem();
    }
}
