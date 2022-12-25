using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    //初期のアイテム所持制限数
    private const int INIT_MAX_POSESSION = 3;

    //プレイヤーのアイテムリスト
    public static List<GameObject> itemList;
    //アイテムの所持制限数
    public static int nowMaxPosession = INIT_MAX_POSESSION;

    static ItemManager()
    {
        itemList = new List<GameObject>();
    }

    /**
     * 取得したアイテムをリストに追加する
     */
    public static bool addItem(GameObject item)
    {
        //アイテムの所持制限を超えている場合
        if (itemList.Count + 1 > nowMaxPosession)
        {
            GManager.instance.wrightLog(MessageManager.createMessage("3"));
            return false;
        }
        //インベントリーが空の状態なら0を割り当てる
        int itemId = itemList.Count == 0 ? 0 : itemList[itemList.Count - 1].GetComponent<Item>().id + 1;
        //IDの割り当て
        item.GetComponent<Item>().id = itemId;
        itemList.Add(item);
        return true;
    }

}
