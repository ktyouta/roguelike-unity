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

    [Header("エネミーの攻撃力")] public int enemyAttackValue;
    [Header("エネミーの所持金")] public int enemyMoney;
    [Header("エネミーを倒した際の経験値")] public int experiencePoint;
    [Header("エネミーのHP")] public int enemyHp = 10;
    [HideInInspector] public string enemyName;
    [HideInInspector] public int enemyNumber;            //敵に付与される連番
    [HideInInspector] public bool isAction = false;
    [HideInInspector] public SpriteRenderer sr = null;
    private Transform target;                            //各ターンに移動しようとする目的object
    private bool isDefeatEnemy = false;
    List<Vector2> trackingNodeList = new List<Vector2>();


    struct EnemyNextPosition
    {
        public float xPosition;
        public float yPosition;
    }

    //Startは、基本クラスの仮想Start関数をオーバーライドします。
    protected override void Start()
    {
        // Enemyオブジェクトのリストに追加して、この敵をGameManagerのインスタンスに登録します。
        //これにより、GameManagerが移動コマンドを発行できるようになります。
        //GManager.instance.AddEnemyToList(this);

        //タグを使用してPlayer GameObjectを見つけ、transformを保存します。
        target = GameObject.FindGameObjectWithTag("Player").transform;

        enemyName = "エネミー" + (enemyNumber + 1);
        sr = GetComponent<SpriteRenderer>();
        
        //スタート関数を抽象クラスから呼ぶ
        base.Start();
    }

    protected void Update()
    {
        if (enemyHp <= 0 && !isDefeatEnemy)
        {
            enemyDefeat();
        }
    }

    //moveEnemyは毎ターンGameMangerによって呼び出され、各敵にプレイヤーに向かって移動するように指示します。
    public void moveEnemy()
    {
        if (enemyHp <= 0)
        {
            return;
        }
        //画面内にいる場合のみ移動
        if (!sr.isVisible)
        {
            GManager.instance.enemyActionEndCount++;
            return;
        }
        isAction = true;
        int xDir = 0;
        int yDir = 0;

        //移動先のリストが空の場合はA-starで経路探索する
        if (trackingNodeList.Count < 1 && GManager.instance.enemyNextPosition.Count > 0)
        {
            //A-starアルゴリズム
            PositionNodeClass startNode = setStartNode(GManager.instance.enemyNextPosition[0]);
            PositionNodeClass goalNode = astarSearch(startNode, GManager.instance.enemyNextPosition[0]);
            //経路探索の結果ゴールまでたどり着けない場合
            if (goalNode == null)
            {
                GManager.instance.enemyActionEndCount++;
                return;
            }
            trackingNextPostion(goalNode);
            //敵の位置とプレイヤーの位置が被っている場合
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
        //x軸がイプシロン(ほぼ)の方が大きい場合
        //if (Mathf.Abs(GManager.instance.enemyNextPosition[0].x - transform.position.x) < float.Epsilon)
        //{
        //    //ターゲット（プレーヤー）の位置のy座標がこの敵の位置のy座標より大きい場合は、y方向1（上に移動）を設定します。 そうでない場合は、-1に設定します（下に移動します）。
        //    yDir = GManager.instance.enemyNextPosition[0].y > transform.position.y ? 1 : -1;
        //}
        ////y軸が同じ場合
        //else
        //{
        //    //ターゲットのx位置が敵のx位置より大きいかどうかを確認します。そうであれば、x方向を1（右に移動）に設定し、そうでなければ-1（左に移動）に設定します。
        //    xDir = GManager.instance.enemyNextPosition[0].x > transform.position.x ? 1 : -1;
        //}
        Vector2 start = transform.position;
        Vector2 next = start + new Vector2(xDir, yDir);
        //移動先がプレイヤーの移動先と被った場合は攻撃
        if (next == GManager.instance.enemyNextPosition[0])
        {
            StartCoroutine(enemyAttack());
            return;
        }
        //移動点が他の敵と被れば移動できない
        if (checkNextPosition(next))
        {
            return;
        }
        GManager.instance.enemyNextPosition.Add(next);
        AttemptMove(xDir, yDir);
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
    PositionNodeClass astarSearch(PositionNodeClass startNode,Vector2 goalPosition)
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
        if (node.parentInfo == null)
        {
            return;
        }
        trackingNodeList.Add(node.position);
        trackingNextPostion(node.parentInfo);
    }

    /**
     * オープンリスト、クローズリスト、移動不可地点リストに同じノードが存在するかをチェック
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
     * キャラクターの移動
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        //boxColliderを無効にして、ラインキャストがこのオブジェクト自身のコライダーに当たらないようにする。
        boxCollider.enabled = false;

        //始点から終点までラインをキャストして、blockingLayerの衝突をチェックします。(ここで自分のオブジェクトとの接触判定が出ないようにfalseしている)
        hit = Physics2D.Linecast(transform.position, end, enemyLayer);

        //ラインキャスト後にboxColliderを再度有効にする
        boxCollider.enabled = true;
        //ヒットしなかった場合は行動開始
        if (hit.transform == null)
        {
            trackingNodeList.RemoveAt(0);
            StartCoroutine(enemySmoothMovement(end));
            return;
        }
        Enemy otherEnemy = hit.collider.GetComponent<Enemy>();
        //Enemyコンポーネントの取得に失敗
        if (otherEnemy == null)
        {
            Debug.Log("failed hitname:" + hit.transform.name);
            trackingNodeList.Clear();
            return;
        }
        bool isAbleToMove = false;
        //ヒットした敵が既に行動を終えている場合
        if (otherEnemy.isAction)
        {
            //移動している場合は自身も移動可能
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        else
        {
            //ヒットした敵が行動していない場合は自身より先に行動させる
            otherEnemy.moveEnemy();
            //行動させた敵が移動した場合は自身も移動可能
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        //行動不可
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
     * 敵が次の移動点に移動できるか判定(既に他の敵の先約がないかチェック)
     */
    protected bool checkNextPosition(Vector2 next)
    {
        for (int i = 1; i < GManager.instance.enemyNextPosition.Count; i++)
        {
            //移動点が被った場合
            if (GManager.instance.enemyNextPosition[i] == next)
            {
                GManager.instance.enemyActionEndCount++;
                return true;
            }
        }
        return false;
    }

    /**
     * 敵の攻撃処理
     */
    protected IEnumerator enemyAttack()
    {
        //攻撃の場合はプレイヤーの行動完了を待つ
        yield return new WaitUntil(() => GManager.instance.isEndPlayerAction);
        animator.Play("EnemyAttack");
        GManager.instance.playerHp -= enemyAttackValue;
        GManager.instance.wrightAttackLog(enemyName, GManager.instance.playerName, enemyAttackValue);
        GManager.instance.enemyActionEndCount++;
    }

    /**
     * 敵が倒された時の処理
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