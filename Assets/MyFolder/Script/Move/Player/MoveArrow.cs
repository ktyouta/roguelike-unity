using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private player playerObj;
    private bool isPushTop = false;
    private bool isPushBottom = false;
    private bool isPushRight = false;
    private bool isPushLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
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
        if (isPushTop)
        {
            playerObj.playerMove(0, 1, false);
        }
        else if (isPushBottom)
        {
            playerObj.playerMove(0, -1, false);
        }
        else if (isPushRight)
        {
            playerObj.playerMove(1, 0, false);
        }
        else if (isPushLeft)
        {
            playerObj.playerMove(-1, 0, false);
        }
    }

    /**
     * プレイヤーを上方向に移動させる
     */
    public void clickTopArrowBtn()
    {
        isPushTop = true;
    }

    /**
     * プレイヤーを下方向に移動させる
     */
    public void clickBottomArrowBtn()
    {
        isPushBottom = true;
    }

    /**
     * プレイヤーを右方向に移動させる
     */
    public void clickRightArrowBtn()
    {
        isPushRight = true;
    }

    /**
     * プレイヤーを左方向に移動させる
     */
    public void clickLeftArrowBtn()
    {
        isPushLeft = true;
    }

    /**
     * ボタンのクリックを終了
     */
    public void buttonUp()
    {
        isPushTop = false;
        isPushBottom = false;
        isPushRight = false;
        isPushLeft = false;
    }
}
