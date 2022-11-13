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
    [Header("ブロッキングレイヤー(下記レイヤー以外で進行不可にしたいもの)")] public LayerMask blockingLayer;  //衝突がチェックされるレイヤー
    [Header("プレイヤーレイヤー")] public LayerMask playerLayer;
    [Header("チェストレイヤー")] public LayerMask treasureLayer;
    [Header("NPCレイヤー")] public LayerMask npcLayer;

    /**
     * キャラクターの次の移動点を返却
     */
    public override List<NextMovePositionClass> retNextPosition(Vector2 targetPosition)
    {
        List<NextMovePositionClass> nextMovePosition = new List<NextMovePositionClass>();
        //A-starアルゴリズム
        PositionNodeClass startNode = setStartNode(targetPosition);
        PositionNodeClass goalNode = astarSearch(startNode, targetPosition);
        trackingNextPostion(goalNode);
        //経路探索の結果ゴールまでたどり着けないまたは敵の位置とプレイヤーの位置が被っている場合
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
     * スタート地点のノードに必要パラメータをセット
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
     * A-starアルゴリズムによる経路探索
     */
    PositionNodeClass astarSearch(PositionNodeClass startNode, Vector2 goalPosition)
    {
        float xPosition;
        float yPosition;
        int searchCount = 0;
        List<PositionNodeClass> openNodeList = new List<PositionNodeClass>();
        List<PositionNodeClass> closeNodeList = new List<PositionNodeClass>();
        openNodeList.Add(startNode);
        //オープンリストが空になったら探索終了(結果ゴールまでたどり着けない場合)
        while (openNodeList.Count > 0)
        {
            searchCount++;
            openNodeList.Sort((a, b) => a.hCost - b.hCost);
            PositionNodeClass minCostNode = openNodeList[0];
            //ゴール地点が見つかるかもしくは指定回数ループした場合
            if (minCostNode.position == goalPosition || searchCount == Define.ASTAR_LOOPNUM)
            {
                return minCostNode;
            }
            //上下左右方向
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
                //現在位置
                Vector2 start = minCostNode.position;
                //移動後の位置
                Vector2 next = start + new Vector2(xPosition, yPosition);
                //オープン、クローズされているか移動不可地点リストに存在すればオープン用のリストに追加不可
                if (checkDuplicate(openNodeList, closeNodeList, next))
                {
                    continue;
                }
                RaycastHit2D hit = Physics2D.Linecast(start, next, blockingLayer | treasureLayer | npcLayer);
                //他のオブジェクトに当たる場合
                if (hit.transform != null)
                {
                    //linecastを複数回行わないようにリストに追加する
                    GManager.instance.unmovableList.Add(next);
                    continue;
                }
                //オープンリストに追加するための設定
                PositionNodeClass positionNode = new PositionNodeClass();
                positionNode.position = next;
                //実コスト
                positionNode.fCost = minCostNode.fCost + 1;
                float absXDifference = Mathf.Abs(goalPosition.x - next.x);
                float absYDifference = Mathf.Abs(goalPosition.y - next.y);
                //推定コスト
                positionNode.gCost = (int)(absXDifference + absYDifference);
                //トータルコスト
                positionNode.hCost = positionNode.fCost + positionNode.gCost;
                positionNode.parentInfo = minCostNode;
                openNodeList.Add(positionNode);
            }
            //クローズリストへの追加とオープンリストからの削除
            closeNodeList.Add(minCostNode);
            openNodeList.RemoveAt(0);
        }
        return null;
    }

    /**
     * ゴールノードから次の移動点を再帰的に探す
     */
    void trackingNextPostion(PositionNodeClass node)
    {
        //親ノードが存在しない(開始地点の)場合は呼び出し終了
        if (node == null || node.parentInfo == null)
        {
            return;
        }
        trackingNodeList.Add(node.position);
        trackingNextPostion(node.parentInfo);
    }

    /**
     * オープンリスト、クローズリスト、移動不可地点リストに同じノードが存在するかをチェック
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
