using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : NotAddItem
{
    [Header("�A�C�e���������̑�����")] public int addNum;
    /**
     * �v���C���[�̃A�C�e���������𑝂₷
     */
    protected override void getItem()
    {
        int tempAddNum = addNum == 0 ? 1 : addNum;
        ItemManager.nowMaxPosession += tempAddNum;
        Debug.Log(MessageManager.createMessage("17", name, tempAddNum.ToString()));
        LogMessageManager.wrightLog(MessageManager.createMessage("17",name,tempAddNum.ToString()));
    }
}
