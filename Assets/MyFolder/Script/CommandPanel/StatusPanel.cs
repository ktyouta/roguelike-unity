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
        statusText = "�v���C���[�� : " + GManager.instance.playerName;
        statusText += "\n";
        statusText += "���x�� : " + GManager.instance.playerLevel;
        statusText += "\n";
        statusText += "HP : " + GManager.instance.playerHp;
        statusText += "\n";
        statusText += "�U���� : " + GManager.instance.playerAttack;
        statusText += "\n";
        statusText += "�h��� : " + GManager.instance.playerDefence;
        statusText += "\n";
        statusText += "�����x : " + GManager.instance.playerFoodPoints;
        statusText += "\n";
        statusText += "������ : " + GManager.instance.playerMoney;
        statusText += "\n";
        statusText += "���� : " + GManager.instance.weaponName;
        statusText += "\n";
        statusText += "�� : " + GManager.instance.shieldName;
        statusPanel.text = statusText;
    }
}
