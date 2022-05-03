using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpText : MonoBehaviour
{
    private int beforeHpValue;
    [Header("HP表示用テキスト")] public Text hpText;

    private void Start()
    {
        hpText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeHpValue != GManager.instance.playerHp)
        {
            hpText.text = "HP:" + GManager.instance.playerHp + " / " + GManager.instance.nowPlayerMaxHp;
            beforeHpValue = GManager.instance.playerHp;
        }
    }
}
