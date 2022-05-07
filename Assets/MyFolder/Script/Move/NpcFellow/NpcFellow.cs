using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellow : MovingObject
{
    protected player playerObj;
    protected Vector2 playerPosition;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerObj = GameObject.FindWithTag("Player").GetComponent<player>();
        playerPosition = playerObj.transform.position;
        setFirstPosition();
    }

    /**
     * NPCの行動
     */
    public void fellowAction()
    {
        //移動
        //if (playerObj.isMoving)
        //{
        //    moveNpc();
        //    return;
        //}
        ////攻撃
        //if (playerObj.isAttackEnemey)
        //{
        //    attack();
        //    return;
        //}
        moveNpc();
    }

    /**
     * NPCを初期位置(プレイヤーの後ろ)にセットする
     */
    protected void setFirstPosition()
    {
        float nextXPosition = playerObj.transform.position.x;
        float nextYPosition = playerObj.transform.position.y;
        //プレイヤーの真後ろに移動する
        if (playerObj.nextHorizontalKey > 0)
        {
            nextXPosition = playerObj.transform.position.x - 1;
        }
        else if (playerObj.nextHorizontalKey < 0)
        {
            nextXPosition = playerObj.transform.position.x + 1;
        }
        else if (playerObj.nextVerticalkey > 0)
        {
            nextYPosition = playerObj.transform.position.y - 1;
        }
        else if (playerObj.nextVerticalkey < 0)
        {
            nextYPosition = playerObj.transform.position.y + 1;
        }
        else
        {
            nextYPosition = playerObj.transform.position.y - 1;
        }
        Vector2 end = new Vector2(nextXPosition, nextYPosition);
        transform.position = end;
    }

    /**
     * 攻撃処理
     */
    protected void attack()
    {

    }

    /**
     * 移動処理
     */
    protected void moveNpc()
    {
        moveChar(playerObj.playerBeforePosition);
    }

    /**
     * キャラクターの移動
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | treasureLayer);
        boxCollider.enabled = true;
        if (hit.transform != null)
        {
            return;
        }
        StartCoroutine(SmoothMovement(end));
    }
}
