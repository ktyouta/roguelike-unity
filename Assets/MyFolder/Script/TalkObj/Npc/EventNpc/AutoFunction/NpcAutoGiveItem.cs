using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAutoGiveItem : NpcAutoFunction
{
    [Header("プレイヤーに渡すアイテム")] public GameObject giveItem;

    /**
     * メッセージが表示された際に自動で実行される関数
     */
    protected override void autoFunc(int funcNumber)
    {
        switch (funcNumber)
        {
            //何も実行しない
            case 0:
                break;
            //プレイヤーにアイテムを渡す
            case 1:
                giveItemToPlayer();
                break;
            default:
                break;
        }
    }

    /**
     * プレイヤーにアイテムを渡す
     */
    protected void giveItemToPlayer()
    {
        GameObject tempItem = Instantiate(giveItem) as GameObject;
        //所持制限のチェック
        if (GManager.instance.addItem(tempItem))
        {
            blockMessageNodeNumber = 1;
        }
        else
        {
            //ノードを切り替える
            nowMessageNodeNumber = 1;
        }
    }
}
