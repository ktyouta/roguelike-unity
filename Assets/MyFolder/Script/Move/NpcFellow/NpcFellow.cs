using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellow : MovingObject
{
    [Header("NPCの名前")] public string npcName;
    [Header("NPCの攻撃力")] public int npcAttack;
    [HideInInspector] public Vector2 npcBeforePosition;
    [HideInInspector] public int npcId;
    protected player playerObj;

    // Start is called before the first frame update
    //protected override void Start()
    //{
    //    base.Start();
    //    playerObj = GameObject.FindWithTag("Player").GetComponent<player>();
    //}

    protected void Awake()
    {
        playerObj = GameObject.FindWithTag("Player").GetComponent<player>();
    }

    protected void OnEnable()
    {
        setFirstPosition();
    }

    /**
     * NPCの行動
     */
    public void fellowAction(Vector2 frontCharPosition)
    {
        //移動
        if (playerObj.isMoving)
        {
            moveChar(frontCharPosition);
        }
        //攻撃
        else if (playerObj.isAttack)
        {
            attack();
        }
    }

    /**
     * NPCを初期位置にセットする
     */
    protected void setFirstPosition()
    {
        //プレイヤーの位置を参照
        if (GManager.instance.fellows.Count < 2)
        {
            Debug.Log("playerpositionref");
            transform.position = playerObj.playerBeforePosition;
            return;
        }
        Debug.Log("fellowscount"+ GManager.instance.fellows.Count);
        for (int i=0;i<GManager.instance.fellows.Count;i++)
        {
            if (GManager.instance.fellows[i].npcId == npcId)
            {
                transform.position = GManager.instance.fellows[i - 1].npcBeforePosition;
                break;
            }
        }
    }

    /**
     * 攻撃処理
     */
    protected void attack()
    {
        //プレイヤーが敵を倒しているか、敵以外に対して攻撃した場合
        if (playerObj.enemyObject == null)
        {
            return;
        }
        if (animator != null)
        {
            animator.Play("NpcAttack");
        }
        int tempNpcAttack = npcAttack == 0 ?10:npcAttack;
        playerObj.enemyObject.enemyHp -= tempNpcAttack;
        GManager.instance.wrightAttackLog(npcName, playerObj.enemyObject.enemyName, tempNpcAttack);
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
        npcBeforePosition = transform.position;
        StartCoroutine(SmoothMovement(end));
    }
}
