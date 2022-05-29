using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellow : MovingObject
{
    [Header("NPC�̖��O")] public string npcName;
    [Header("NPC�̍U����")] public int npcAttack;
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
     * NPC�̍s��
     */
    public void fellowAction(Vector2 frontCharPosition)
    {
        //�ړ�
        if (playerObj.isMoving)
        {
            moveChar(frontCharPosition);
        }
        //�U��
        else if (playerObj.isAttack)
        {
            attack();
        }
    }

    /**
     * NPC�������ʒu�ɃZ�b�g����
     */
    protected void setFirstPosition()
    {
        //�v���C���[�̈ʒu���Q��
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
     * �U������
     */
    protected void attack()
    {
        //�v���C���[���G��|���Ă��邩�A�G�ȊO�ɑ΂��čU�������ꍇ
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
        npcBeforePosition = transform.position;
        StartCoroutine(SmoothMovement(end));
    }
}
