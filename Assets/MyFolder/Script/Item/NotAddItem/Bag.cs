using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : NotAddItem
{
    [Header("アイテム所持数の増加数")] public int addNum;
    /**
     * プレイヤーのアイテム所持数を増やす
     */
    protected override void getItem()
    {
        int tempAddNum = addNum == 0 ? 1 : addNum;
        ItemManager.nowMaxPosession += tempAddNum;
        GManager.instance.wrightLog(name+"を拾った。\r\nアイテムの所持数が"+tempAddNum+"増えた");
    }
}
