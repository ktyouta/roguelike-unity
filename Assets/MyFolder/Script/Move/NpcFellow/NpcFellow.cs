using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellow : MovingObject
{
    [Header("NPC�̖��O")] public string npcName;
    [Header("NPC�̍U����")] public int npcAttack;
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
     * NPC�̍s��
     */
    public void fellowAction()
    {
        //�ړ�
        if (playerObj.isMoving)
        {
            moveNpc();
            return;
        }
        //�U��
        if (playerObj.isAttack)
        {
            attack();
            return;
        }
    }

    /**
     * NPC�������ʒu(�v���C���[�̌��)�ɃZ�b�g����
     */
    protected void setFirstPosition()
    {
        float nextXPosition = playerObj.transform.position.x;
        float nextYPosition = playerObj.transform.position.y;
        //�v���C���[�̐^���Ɉړ�����
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
     * �U������
     */
    protected void attack()
    {
        Debug.Log("attack");
        //�v���C���[���G��|���Ă��邩�A�G�ȊO�ɑ΂��čU�������ꍇ
        if (playerObj.enemyObject == null)
        {
            return;
        }
        int tempNpcAttack = npcAttack == 0 ?10:npcAttack;
        playerObj.enemyObject.enemyHp -= tempNpcAttack;
        GManager.instance.wrightAttackLog(npcName, playerObj.enemyObject.enemyName, tempNpcAttack);
    }

    /**
     * �ړ�����
     */
    protected void moveNpc()
    {
        moveChar(playerObj.playerBeforePosition);
    }

    /**
     * �L�����N�^�[�̈ړ�
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
