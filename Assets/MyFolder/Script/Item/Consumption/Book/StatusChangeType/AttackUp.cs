using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : BookBase
{
    [Header("�U���͂̏㏸�l")] public int attackRiseValue;
    [SerializeField,Header("�L�����̃X�e�[�^�X�p�R���|�[�l���g")] private StatusComponentBase statusComponentObj;

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
        GManager.instance.wrightLog(name+"���g�p����");
        base.useItem();
    }
}
