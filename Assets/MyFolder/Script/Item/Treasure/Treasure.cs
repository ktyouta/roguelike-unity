using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [HideInInspector] public int treasureHp = 100;
    [Header("�A�C�e���̒��I�ɗp����ID")] public int lotteryId;

    //�󔠊J�����̏���
    private void openTreasure()
    {
        GameObject getItem = Instantiate(GManager.instance.lotteryitemList[lotteryId][Random.Range(0, GManager.instance.lotteryitemList[lotteryId].Count - 1)]) as GameObject;
        //�A�C�e���̏��������̔���
        if (ItemManager.addItem(getItem))
        {
            Destroy(this.gameObject);
        }
        else
        {
            treasureHp = 1;
        }
    }

    /**
     * �󔠂̃_���[�W�v�Z
     */
    public void calculateDamage(int damage)
    {
        treasureHp -= damage;
        if (treasureHp <= 0)
        {
            openTreasure();
        }
    }
}
