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
            GManager.instance.wrightLog("�󔠂̎c��ϋv�l�F" + hp);
        }
    }

    //�󔠊J�����̏���
    private void openTreasure()
    {
        //GameObject getItem = Instantiate(GManager.instance.lotteryitemList[lotteryId][Random.Range(0, GManager.instance.lotteryitemList[lotteryId].Count - 1)]) as GameObject;
        //�A�C�e���̏��������̔���
        //if (GManager.instance.addItem(getItem))
        //{
        //    GManager.instance.wrightLog("�󔠂��J�����B");
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    statusObj.charHp.setHp(1);
        //}
    }
}
