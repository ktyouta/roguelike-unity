using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static player;

public class HpText : MonoBehaviour
{
    private int beforeHpValue;
    StatusComponentPlayer playerStatus;
    [Header("HP表示用テキスト")] public Text hpText;

    private void Start()
    {
        hpText = GetComponent<Text>();
        playerStatus = (StatusComponentPlayer)GameObject.FindGameObjectWithTag("Player").GetComponent<player>().statusObj;
    }

    // Update is called once per frame
    void Update()
    {
        //if (beforeHpValue != GManager.instance.playerHp)
        if (beforeHpValue != playerStatus.charHp.showHp())
        {
            hpText.text = "HP:" + playerStatus.charHp.showHp() + " / " + playerStatus.charHp.showMaxHp();
            beforeHpValue = playerStatus.charHp.showHp();
        }
    }
}
