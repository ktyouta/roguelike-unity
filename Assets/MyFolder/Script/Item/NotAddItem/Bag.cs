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
        GManager.instance.wrightLog(name+"���E�����B\r\n�A�C�e���̏�������"+tempAddNum+"������");
    }
}
