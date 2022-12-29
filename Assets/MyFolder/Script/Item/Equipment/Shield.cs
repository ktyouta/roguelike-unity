using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EquipmentBase
{
    [Header("�h���")] public int defenceParam;
    [SerializeField, Header("�L�����̃X�e�[�^�X�p�R���|�[�l���g")] private StatusComponentPlayer statusComponentObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    public override void useItem()
    {
        string shieldName;
        base.useItem();
        if (isEquip)
        {
            //GManager.instance.playerDefence += defenceParam;
            statusComponentObj?.charDefence.settotalDefence(defenceParam);
            shieldName = name;
        }
        else
        {
            //GManager.instance.playerDefence -= defenceParam;
            statusComponentObj?.charDefence.initializeTotalDefence();
            shieldName = "�Ȃ�";
        }
        statusComponentObj.shieldName = shieldName;
    }
}
