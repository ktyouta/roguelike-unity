using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUp : BookBase
{
    [Header("�h��͂̏㏸�l")] public int defenceRiseValue;
    [SerializeField, Header("�L�����̃X�e�[�^�X�p�R���|�[�l���g")] private StatusComponentBase statusComponentObj;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    public override void useItem()
    {
        //GManager.instance.playerDefence += defenceRiseValue==0?10: defenceRiseValue;
        statusComponentObj?.charDefence.adddefence(defenceRiseValue);
        GManager.instance.wrightLog(name + "���g�p����");
        base.useItem();
    }
}
