using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Enemy : MovingObject
{
    public class PositionNodeClass
    {
        public Vector2 position;
        public PositionNodeClass parentInfo;
        public int fCost;
        public int gCost;
        public int hCost;
    }

    [Header("�G�l�~�[�̍U����")] public int enemyAttackValue;
    [Header("�G�l�~�[�̏�����")] public int enemyMoney;
    [Header("�G�l�~�[��|�����ۂ̌o���l")] public int experiencePoint;
    [Header("�G�l�~�[��HP")] public int enemyHp = 10;
    [HideInInspector] public string enemyName;
    [HideInInspector] public int enemyNumber;            //�G�ɕt�^�����A��
    [HideInInspector] public bool isAction = false;
    [HideInInspector] public SpriteRenderer sr = null;
    private Transform target;                            //�e�^�[���Ɉړ����悤�Ƃ���ړIobject
    private bool isDefeatEnemy = false;
    List<Vector2> trackingNodeList = new List<Vector2>();


    struct EnemyNextPosition
    {
        public float xPosition;
        public float yPosition;
    }

    //Start�́A��{�N���X�̉��zStart�֐����I�[�o�[���C�h���܂��B
    protected override void Start()
    {
        // Enemy�I�u�W�F�N�g�̃��X�g�ɒǉ����āA���̓G��GameManager�̃C���X�^���X�ɓo�^���܂��B
        //����ɂ��AGameManager���ړ��R�}���h�𔭍s�ł���悤�ɂȂ�܂��B
        //GManager.instance.AddEnemyToList(this);

        //�^�O���g�p����Player GameObject�������Atransform��ۑ����܂��B
        target = GameObject.FindGameObjectWithTag("Player").transform;

        enemyName = "�G�l�~�[" + (enemyNumber + 1);
        sr = GetComponent<SpriteRenderer>();
        
        //�X�^�[�g�֐��𒊏ۃN���X����Ă�
        base.Start();
    }

    protected void Update()
    {
        if (enemyHp <= 0 && !isDefeatEnemy)
        {
            enemyDefeat();
        }
    }

    //moveEnemy�͖��^�[��GameManger�ɂ���ČĂяo����A�e�G�Ƀv���C���[�Ɍ������Ĉړ�����悤�Ɏw�����܂��B
    public void moveEnemy()
    {
        if (enemyHp <= 0)
        {
            return;
        }
        //��ʓ��ɂ���ꍇ�݈̂ړ�
        if (!sr.isVisible)
        {
            GManager.instance.enemyActionEndCount++;
            return;
        }
        isAction = true;
        int xDir = 0;
        int yDir = 0;

        //�ړ���̃��X�g����̏ꍇ��A-star�Ōo�H�T������
        if (trackingNodeList.Count < 1 && GManager.instance.enemyNextPosition.Count > 0)
        {
            //A-star�A���S���Y��
            PositionNodeClass startNode = setStartNode(GManager.instance.enemyNextPosition[0]);
            PositionNodeClass goalNode = astarSearch(startNode, GManager.instance.enemyNextPosition[0]);
            //�o�H�T���̌��ʃS�[���܂ł��ǂ蒅���Ȃ��ꍇ
            if (goalNode == null)
            {
                GManager.instance.enemyActionEndCount++;
                return;
            }
            trackingNextPostion(goalNode);
            //�G�̈ʒu�ƃv���C���[�̈ʒu������Ă���ꍇ
            if (trackingNodeList.Count < 1)
            {
                GManager.instance.enemyActionEndCount++;
                return;
            }
            trackingNodeList.Reverse();
        }
        xDir = (int)(trackingNodeList[0].x - transform.position.x);
        yDir = (int)(trackingNodeList[0].y - transform.position.y);
        Debug.Log("xdir"+xDir);
        Debug.Log("ydir" + yDir);
        //x�����C�v�V����(�ق�)�̕����傫���ꍇ
        //if (Mathf.Abs(GManager.instance.enemyNextPosition[0].x - transform.position.x) < float.Epsilon)
        //{
        //    //�^�[�Q�b�g�i�v���[���[�j�̈ʒu��y���W�����̓G�̈ʒu��y���W���傫���ꍇ�́Ay����1�i��Ɉړ��j��ݒ肵�܂��B �����łȂ��ꍇ�́A-1�ɐݒ肵�܂��i���Ɉړ����܂��j�B
        //    yDir = GManager.instance.enemyNextPosition[0].y > transform.position.y ? 1 : -1;
        //}
        ////y���������ꍇ
        //else
        //{
        //    //�^�[�Q�b�g��x�ʒu���G��x�ʒu���傫�����ǂ������m�F���܂��B�����ł���΁Ax������1�i�E�Ɉړ��j�ɐݒ肵�A�����łȂ����-1�i���Ɉړ��j�ɐݒ肵�܂��B
        //    xDir = GManager.instance.enemyNextPosition[0].x > transform.position.x ? 1 : -1;
        //}
        Vector2 start = transform.position;
        Vector2 next = start + new Vector2(xDir, yDir);
        //�ړ��悪�v���C���[�̈ړ���Ɣ�����ꍇ�͍U��
        if (next == GManager.instance.enemyNextPosition[0])
        {
            StartCoroutine(enemyAttack());
            return;
        }
        //�ړ��_�����̓G�Ɣ��Έړ��ł��Ȃ�
        if (checkNextPosition(next))
        {
            return;
        }
        GManager.instance.enemyNextPosition.Add(next);
        AttemptMove(xDir, yDir);
    }

    /**
     * �X�^�[�g�n�_�̃m�[�h�ɕK�v�p�����[�^���Z�b�g
     */
    PositionNodeClass setStartNode(Vector2 diffPosition)
    {
        PositionNodeClass startNode = new PositionNodeClass();
        startNode.position = transform.position;
        startNode.fCost = 0;
        float absXDifference = Mathf.Abs(diffPosition.x - startNode.position.x);
        float absYDifference = Mathf.Abs(diffPosition.y - startNode.position.y);
        startNode.gCost = (int)(absXDifference + absYDifference);
        startNode.hCost = startNode.gCost;
        startNode.parentInfo = null;
        return startNode;
    }

    /**
     * A-star�A���S���Y���ɂ��o�H�T��
     */
    PositionNodeClass astarSearch(PositionNodeClass startNode,Vector2 goalPosition)
    {
        float xPosition;
        float yPosition;
        int searchCount = 0;
        List<PositionNodeClass> openNodeList = new List<PositionNodeClass>();
        List<PositionNodeClass> closeNodeList = new List<PositionNodeClass>();
        openNodeList.Add(startNode);
        //�I�[�v�����X�g����ɂȂ�����T���I��(���ʃS�[���܂ł��ǂ蒅���Ȃ��ꍇ)
        while (openNodeList.Count > 0)
        {
            searchCount++;
            openNodeList.Sort((a, b) => a.hCost - b.hCost);
            PositionNodeClass minCostNode = openNodeList[0];
            //�S�[���n�_�������邩�������͎w��񐔃��[�v�����ꍇ
            if (minCostNode.position == goalPosition || searchCount == Define.ASTAR_LOOPNUM)
            {
                return minCostNode;
            }
            //�㉺���E����
            for (int i = 0; i < 4; i++)
            {
                xPosition = 0;
                yPosition = 0;
                switch (i)
                {
                    case 0:
                        yPosition = 1;
                        break;
                    case 1:
                        xPosition = -1;
                        break;
                    case 2:
                        xPosition = 1;
                        break;
                    case 3:
                        yPosition = -1;
                        break;
                }
                //���݈ʒu
                Vector2 start = minCostNode.position;
                //�ړ���̈ʒu
                Vector2 next = start + new Vector2(xPosition, yPosition);
                //�I�[�v���A�N���[�Y����Ă��邩�ړ��s�n�_���X�g�ɑ��݂���΃I�[�v���p�̃��X�g�ɒǉ��s��
                if (checkDuplicate(openNodeList, closeNodeList, next))
                {
                    continue;
                }
                RaycastHit2D hit = Physics2D.Linecast(start, next, blockingLayer | treasureLayer | npcLayer);
                //���̃I�u�W�F�N�g�ɓ�����ꍇ
                if (hit.transform != null)
                {
                    //linecast�𕡐���s��Ȃ��悤�Ƀ��X�g�ɒǉ�����
                    GManager.instance.unmovableList.Add(next);
                    continue;
                }
                //�I�[�v�����X�g�ɒǉ����邽�߂̐ݒ�
                PositionNodeClass positionNode = new PositionNodeClass();
                positionNode.position = next;
                //���R�X�g
                positionNode.fCost = minCostNode.fCost + 1;
                float absXDifference = Mathf.Abs(goalPosition.x - next.x);
                float absYDifference = Mathf.Abs(goalPosition.y - next.y);
                //����R�X�g
                positionNode.gCost = (int)(absXDifference + absYDifference);
                //�g�[�^���R�X�g
                positionNode.hCost = positionNode.fCost + positionNode.gCost;
                positionNode.parentInfo = minCostNode;
                openNodeList.Add(positionNode);
            }
            //�N���[�Y���X�g�ւ̒ǉ��ƃI�[�v�����X�g����̍폜
            closeNodeList.Add(minCostNode);
            openNodeList.RemoveAt(0);
        }
        return null;
    }

    /**
     * �S�[���m�[�h���玟�̈ړ��_���ċA�I�ɒT��
     */
    void trackingNextPostion(PositionNodeClass node)
    {
        //�e�m�[�h�����݂��Ȃ�(�J�n�n�_��)�ꍇ�͌Ăяo���I��
        if (node.parentInfo == null)
        {
            return;
        }
        trackingNodeList.Add(node.position);
        trackingNextPostion(node.parentInfo);
    }

    /**
     * �I�[�v�����X�g�A�N���[�Y���X�g�A�ړ��s�n�_���X�g�ɓ����m�[�h�����݂��邩���`�F�b�N
     */
    bool checkDuplicate(List<PositionNodeClass> openNodeList, List<PositionNodeClass> closeNodeList,Vector2 node)
    {
        for (int i = 0; i < openNodeList.Count; i++)
        {
            if (node == openNodeList[i].position)
            {
                return true;
            }
        }
        for (int i=0;i< closeNodeList.Count;i++)
        {
            if (node == closeNodeList[i].position)
            {
                return true;
            }
        }
        for (int i=0;i< GManager.instance.unmovableList.Count;i++)
        {
            if (node == GManager.instance.unmovableList[i])
            {
                return true;
            }
        }
        return false;
    }

    /**
     * �L�����N�^�[�̈ړ�
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        //boxCollider�𖳌��ɂ��āA���C���L���X�g�����̃I�u�W�F�N�g���g�̃R���C�_�[�ɓ�����Ȃ��悤�ɂ���B
        boxCollider.enabled = false;

        //�n�_����I�_�܂Ń��C�����L���X�g���āAblockingLayer�̏Փ˂��`�F�b�N���܂��B(�����Ŏ����̃I�u�W�F�N�g�Ƃ̐ڐG���肪�o�Ȃ��悤��false���Ă���)
        hit = Physics2D.Linecast(transform.position, end, enemyLayer);

        //���C���L���X�g���boxCollider���ēx�L���ɂ���
        boxCollider.enabled = true;
        //�q�b�g���Ȃ������ꍇ�͍s���J�n
        if (hit.transform == null)
        {
            trackingNodeList.RemoveAt(0);
            StartCoroutine(enemySmoothMovement(end));
            return;
        }
        Enemy otherEnemy = hit.collider.GetComponent<Enemy>();
        //Enemy�R���|�[�l���g�̎擾�Ɏ��s
        if (otherEnemy == null)
        {
            Debug.Log("failed hitname:" + hit.transform.name);
            trackingNodeList.Clear();
            return;
        }
        bool isAbleToMove = false;
        //�q�b�g�����G�����ɍs�����I���Ă���ꍇ
        if (otherEnemy.isAction)
        {
            //�ړ����Ă���ꍇ�͎��g���ړ��\
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        else
        {
            //�q�b�g�����G���s�����Ă��Ȃ��ꍇ�͎��g����ɍs��������
            otherEnemy.moveEnemy();
            //�s���������G���ړ������ꍇ�͎��g���ړ��\
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        //�s���s��
        if (!isAbleToMove)
        {
            GManager.instance.enemyActionEndCount++;
            trackingNodeList.Clear();
            return;
        }
        trackingNodeList.RemoveAt(0);
        StartCoroutine(enemySmoothMovement(end));
    }

    protected IEnumerator enemySmoothMovement(Vector3 end)
    {
        yield return StartCoroutine(SmoothMovement(end));
        GManager.instance.enemyActionEndCount++;
    }

    /**
     * �G�����̈ړ��_�Ɉړ��ł��邩����(���ɑ��̓G�̐�񂪂Ȃ����`�F�b�N)
     */
    protected bool checkNextPosition(Vector2 next)
    {
        for (int i = 1; i < GManager.instance.enemyNextPosition.Count; i++)
        {
            //�ړ��_��������ꍇ
            if (GManager.instance.enemyNextPosition[i] == next)
            {
                GManager.instance.enemyActionEndCount++;
                return true;
            }
        }
        return false;
    }

    /**
     * �G�̍U������
     */
    protected IEnumerator enemyAttack()
    {
        //�U���̏ꍇ�̓v���C���[�̍s��������҂�
        yield return new WaitUntil(() => GManager.instance.isEndPlayerAction);
        animator.Play("EnemyAttack");
        GManager.instance.playerHp -= enemyAttackValue;
        GManager.instance.wrightAttackLog(enemyName, GManager.instance.playerName, enemyAttackValue);
        GManager.instance.enemyActionEndCount++;
    }

    /**
     * �G���|���ꂽ���̏���
     */
    protected void enemyDefeat()
    {
        GManager.instance.wrightDeadLog(enemyName);
        isDefeatEnemy = true;
        GManager.instance.playerMoney += enemyMoney;
        GManager.instance.beforeLevelupExperience = GManager.instance.nowExprience;
        GManager.instance.nowExprience += experiencePoint;
        GManager.instance.mostRecentExperience = experiencePoint;
        GManager.instance.removeEnemyToList(enemyNumber);
        Destroy(gameObject, 0.5f);
    }
}