using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUp : BookBase
{
    [Header("防御力の上昇値")] public int defenceRiseValue;
    [SerializeField, Header("キャラのステータス用コンポーネント")] private StatusComponentBase statusComponentObj;

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
        LogMessageManager.wrightLog(name + "を使用した");
        base.useItem(statusObj);
    }
}
