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
        playerStatus = (StatusComponentPlayer)GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (beforeHpValue != GManager.instance.playerHp)
        if (beforeHpValue != playerStatus.charHp.hp)
        {
            hpText.text = "HP:" + playerStatus.charHp.hp + " / " + playerStatus.charHp.maxHp;
            beforeHpValue = playerStatus.charHp.hp;
        }
    }
}
