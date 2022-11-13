using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveActionComponentAstar : MoveActionComponentBase
{
    public class PositionNodeClass
    {
        public Vector2 position;
        public PositionNodeClass parentInfo;
        public int fCost;
        public int gCost;
        public int hCost;
    }
    List<Vector2> trackingNodeList = new List<Vector2>();
    [Header("�u���b�L���O���C���[(���L���C���[�ȊO�Ői�s�s�ɂ���������)")] public LayerMask blockingLayer;  //�Փ˂��`�F�b�N����郌�C���[
    [Header("�v���C���[���C���[")] public LayerMask playerLayer;
    [Header("�`�F�X�g���C���[")] public LayerMask treasureLayer;
    [Header("NPC���C���[")] public LayerMask npcLayer;

    /**
     * �L�����N�^�[�̎��̈ړ��_��ԋp
     */
    public override List<NextMovePositionClass> retNextPosition(Vector2 targetPosition)
    {
        List<NextMovePositionClass> nextMovePosition = new List<NextMovePositionClass>();
        //A-star�A���S���Y��
        PositionNodeClass startNode = setStartNode(targetPosition);
        PositionNodeClass goalNode = astarSearch(startNode, targetPosition);
        trackingNextPostion(goalNode);
        //�o�H�T���̌��ʃS�[���܂ł��ǂ蒅���Ȃ��܂��͓G�̈ʒu�ƃv���C���[�̈ʒu������Ă���ꍇ
        if (trackingNodeList.Count < 1)
        {
            return nextMovePosition;
        }
        trackingNodeList.Reverse();
        float preXTrackingNode = transform.position.x;
        float preYTrackingNode = transform.position.y;
        for (int i=0;i<trackingNodeList.Count;i++)
        {
            nextMovePosition.Add(new NextMovePositionClass
            {
                xDir = (int)(trackingNodeList[i].x - preXTrackingNode),
                yDir = (int)(trackingNodeList[i].y - preYTrackingNode),
            });
            preXTrackingNode = trackingNodeList[i].x;
            preYTrackingNode = trackingNodeList[i].y;
        }
        trackingNodeList.Clear();
        return nextMovePosition;
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
    PositionNodeClass astarSearch(PositionNodeClass startNode, Vector2 goalPosition)
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
        if (node == null || node.parentInfo == null)
        {
            return;
        }
        trackingNodeList.Add(node.position);
        trackingNextPostion(node.parentInfo);
    }

    /**
     * �I�[�v�����X�g�A�N���[�Y���X�g�A�ړ��s�n�_���X�g�ɓ����m�[�h�����݂��邩���`�F�b�N
     */
    bool checkDuplicate(List<PositionNodeClass> openNodeList, List<PositionNodeClass> closeNodeList, Vector2 node)
    {
        for (int i = 0; i < openNodeList.Count; i++)
        {
            if (node == openNodeList[i].position)
            {
                return true;
            }
        }
        for (int i = 0; i < closeNodeList.Count; i++)
        {
            if (node == closeNodeList[i].position)
            {
                return true;
            }
        }
        return false;
    }
}
