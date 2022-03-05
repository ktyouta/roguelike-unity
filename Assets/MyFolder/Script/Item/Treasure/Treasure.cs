using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [HideInInspector] public int treasureHp = 100;
    [HideInInspector] public bool isOpen = false;
    [Header("�A�C�e���̒��I�ɗp����ID")] public int lotteryId;

    // Update is called once per frame
    void Update()
    {
        if (treasureHp <= 0 && !isOpen)
        {
            openTreasure();
            isOpen = true;
        }
    }

    private void openTreasure()
    {
        //Debug.Log("trasurelist"+ GManager.instance.treasureItemList[0]);
        //Item getItem = GManager.instance.treasureItemList[0];
        Item getItem = GManager.instance.lotteryitemList[lotteryId][Random.Range(0, GManager.instance.lotteryitemList[lotteryId].Count-1)];
        GManager.instance.addItem(getItem);
        int itemId = GManager.instance.itemList.Count;
        getItem.id = itemId;
        Destroy(this.gameObject);
    }
}
