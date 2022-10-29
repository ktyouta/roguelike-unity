using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    private player playerObj;
    private Button attackButton;
    private AttackComponentBase attackComponentObj;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        attackButton = GetComponent<Button>();
        attackButton.onClick.AddListener(attackAction);
        attackComponentObj = playerObj.GetComponent<AttackComponentBase>();
    }

    /**
     * 攻撃ボタン押下
     */
    private void attackAction()
    {
        //プレイヤーの状態が通常以外
        if (playerObj.plState != player.playerState.Normal)
        {
            return;
        }
        //プレイヤーのターンでない、移動中、攻撃中はコマンド入力を受け付けない
        if (!GManager.instance.playersTurn || playerObj.isMoving || playerObj.isAttack)
        {
            return;
        }
        //playerObj.Attack();
        attackComponentObj.attack(playerObj.nextHorizontalKey,playerObj.nextVerticalkey);
    }
}
