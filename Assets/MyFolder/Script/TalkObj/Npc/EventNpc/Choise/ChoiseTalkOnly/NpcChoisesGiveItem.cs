using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcChoisesGiveItem : NpcChoices
{
    [Header("プレイヤーに渡すアイテム")] public GameObject giveItem;
    protected override void selectMessage(int funcNumber)
    {
        //メッセージノードの切り替え
        nowNodeIndex = funcNumber;
        switch (funcNumber)
        {
            case 0:
                giveItemToPlayer();
                break;
            case 1:
                break;
            default:
                break;
        }
        //選択肢押下後に止めていた処理(コルーチン)を進める
        clickChioseButtonFlag = true;
    }

    /*
     * プレイヤーにアイテムを渡す
     */
    protected void giveItemToPlayer()
    {
        if (!ItemManager.addItem(giveItem))
        {
            nowNodeIndex = 2;
        }
    }
}
