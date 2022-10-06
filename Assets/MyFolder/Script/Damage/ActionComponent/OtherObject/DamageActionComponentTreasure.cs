using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentTreasure : DamageActionComponentBase
{
    public override void reciveDamageAction(int hp)
    {
        if (hp <= 0)
        {
            openTreasure();
        }
        else
        {
            GManager.instance.wrightLog("宝箱の残り耐久値：" + hp);
        }
    }

    //宝箱開封時の処理
    private void openTreasure()
    {
        //GameObject getItem = Instantiate(GManager.instance.lotteryitemList[lotteryId][Random.Range(0, GManager.instance.lotteryitemList[lotteryId].Count - 1)]) as GameObject;
        //アイテムの所持制限の判定
        //if (GManager.instance.addItem(getItem))
        //{
        //    GManager.instance.wrightLog("宝箱が開いた。");
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    statusObj.charHp.setHp(1);
        //}
    }
}
