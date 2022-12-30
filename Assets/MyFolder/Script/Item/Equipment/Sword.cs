using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : EquipmentBase
{

    [Header("攻撃力")] public int attackParam;
    [SerializeField,Header("キャラのステータス用コンポーネント")] private StatusComponentPlayer statusComponentObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    public override void useItem(StatusComponentBase statusObj)
    {
        string weaponName;
        base.useItem(statusObj);
        if (isEquip)
        {
            //GManager.instance.playerAttack += attackParam;
            statusObj?.charAttack.setTotalAttack(attackParam);
            weaponName = name;
        }
        else
        {
            //GManager.instance.playerAttack -= attackParam;
            statusObj?.charAttack.initializeTotalAttack();
            weaponName = "なし";
        }
        ((StatusComponentPlayer)statusObj).weaponName = weaponName;
    }
}
