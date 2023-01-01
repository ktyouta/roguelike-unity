using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemComponentPlayer : UseItemComponentBase
{
    //アイテムを使用
    public override void use(Item item, StatusComponentMoving statusObj)
    {
        base.use(item,statusObj);
        GManager.instance.isEndPlayerAction = true;
        GManager.instance.playersTurn = false;
        //プレイヤーの位置情報は必ずリストの先頭になる
        GManager.instance.charsNextPosition.Add(statusObj.transform.position);
    }
}
