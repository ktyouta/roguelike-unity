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

    public override void useItem(StatusComponentMoving statusObj)
    {
        //GManager.instance.playerDefence += defenceRiseValue==0?10: defenceRiseValue;
        statusObj?.charDefence.adddefence(defenceRiseValue);
        LogMessageManager.wrightLog(name + "���g�p����");
        base.useItem(statusObj);
    }
}
