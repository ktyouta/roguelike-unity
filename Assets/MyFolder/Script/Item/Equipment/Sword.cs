using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : EquipmentBase
{

    [Header("�U����")] public int attackParam;
    [SerializeField,Header("�L�����̃X�e�[�^�X�p�R���|�[�l���g")] private StatusComponentPlayer statusComponentObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    public override void useItem()
    {
        string weaponName;
        base.useItem();
        if (isEquip)
        {
            //GManager.instance.playerAttack += attackParam;
            statusComponentObj?.charAttack.setTotalAttack(attackParam);
            weaponName = name;
        }
        else
        {
            //GManager.instance.playerAttack -= attackParam;
            statusComponentObj?.charAttack.initializeTotalAttack();
            weaponName = "�Ȃ�";
        }
        statusComponentObj.weaponName = weaponName;
    }
}
