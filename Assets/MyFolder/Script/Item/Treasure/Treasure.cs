using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [HideInInspector] public int treasureHp = 100;
    [HideInInspector] public bool isOpen = false;
    [Header("アイテムの抽選に用いるID")] public int lotteryId;

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
        GameObject getItem = GManager.instance.lotteryitemList[lotteryId][Random.Range(0, GManager.instance.lotteryitemList[lotteryId].Count-1)];
        int itemId = GManager.instance.itemList.Count;
        getItem.GetComponent<Item>().id = itemId;
        GManager.instance.addItem(getItem);
        Destroy(this.gameObject);
    }
}
