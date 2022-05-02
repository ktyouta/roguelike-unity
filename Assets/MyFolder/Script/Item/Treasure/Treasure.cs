using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [HideInInspector] public int treasureHp = 100;
    [Header("アイテムの抽選に用いるID")] public int lotteryId;

    // Update is called once per frame
    void Update()
    {
        if (treasureHp <= 0)
        {
            openTreasure();
        }
    }

    //宝箱開封時の処理
    private void openTreasure()
    {
        GameObject getItem = Instantiate(GManager.instance.lotteryitemList[lotteryId][Random.Range(0, GManager.instance.lotteryitemList[lotteryId].Count - 1)]) as GameObject;
        //アイテムの所持制限の判定
        if (GManager.instance.addItem(getItem))
        {
            Destroy(this.gameObject);
        }
        else
        {
            treasureHp = 1;
        }
    }
}
