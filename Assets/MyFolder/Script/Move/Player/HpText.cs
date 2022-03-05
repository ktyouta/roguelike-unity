using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpText : MonoBehaviour
{
    private int beforeHpValue;
    [Header("HP�\���p�e�L�X�g")] public Text hpText;

    // Update is called once per frame
    void Update()
    {
        if (beforeHpValue != GManager.instance.playerHp)
        {
            hpText.text = "HP:" + GManager.instance.playerHp;
            beforeHpValue = GManager.instance.playerHp;
        }
    }
}
