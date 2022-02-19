using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{

    public Text statusPanel;
    private string statusText;
    // Start is called before the first frame update
    void Start()
    {
        //statusPanel = GameObject.Find("StatusPanel").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        statusText = "プレイヤー名 : " + GManager.instance.playerName;
        statusText += "\n";
        statusText += "レベル : " + GManager.instance.playerLevel;
        statusText += "\n";
        statusText += "HP : " + GManager.instance.playerHp;
        statusText += "\n";
        statusText += "攻撃力 : " + GManager.instance.playerAttack;
        statusText += "\n";
        statusText += "防御力 : " + GManager.instance.playerDefence;
        statusText += "\n";
        statusText += "満腹度 : " + GManager.instance.playerFoodPoints;
        statusText += "\n";
        statusText += "所持金 : " + GManager.instance.playerMoney;
        statusText += "\n";
        statusText += "武器 : " + GManager.instance.weaponName;
        statusText += "\n";
        statusText += "盾 : " + GManager.instance.shieldName;
        statusPanel.text = statusText;
    }
}
