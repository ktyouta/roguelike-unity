using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    [Header("エネミーの攻撃力")] public int enemyAttackValue;
    [Header("エネミーの所持金")] public int enemyMoney;
    [Header("エネミーを倒した際の経験値")] public int experiencePoint;
    [Header("エネミーのHP")] public int enemyHp = 10;
    [Header("NPCレイヤー")] public LayerMask npcLayer;
    [HideInInspector] public string enemyName;
    [HideInInspector] public int enemyNumber;            //敵に付与される連番
    [HideInInspector] public bool isAction = false;
    private Animator animator;                            //敵のAnimatorコンポーネントへの参照を格納するAnimator型の変数。
    private Transform target;                            //各ターンに移動しようとする目的object
    private bool isDefeatEnemy = false;
    private SpriteRenderer sr = null;

    //Startは、基本クラスの仮想Start関数をオーバーライドします。
    protected override void Start()
    {
        // Enemyオブジェクトのリストに追加して、この敵をGameManagerのインスタンスに登録します。
        //これにより、GameManagerが移動コマンドを発行できるようになります。
        //GManager.instance.AddEnemyToList(this);

        //添付されたAnimatorコンポーネントへの参照を取得して保存します。
        animator = GetComponent<Animator>();

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

    //MoveEnemyは毎ターンGameMangerによって呼び出され、各敵にプレイヤーに向かって移動するように指示します。
    public void MoveEnemy()
    {
        //画面内にいる場合のみ移動
        if (!sr.isVisible)
        {
            return;
        }
        isAction = true;
        // X軸とY軸の移動方向の変数を宣言します。これらの範囲は-1から1です。
        //これらの値により、基本的な方向（上、下、左、右）を選択できます。
        int xDir = 0;
        int yDir = 0;

        //x軸がイプシロン(ほぼ)の方が大きい場合
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            //ターゲット（プレーヤー）の位置のy座標がこの敵の位置のy座標より大きい場合は、y方向1（上に移動）を設定します。 そうでない場合は、-1に設定します（下に移動します）。
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        //y軸が同じ場合
        else
        {
            //ターゲットのx位置が敵のx位置より大きいかどうかを確認します。そうであれば、x方向を1（右に移動）に設定し、そうでなければ-1（左に移動）に設定します。
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        Vector2 start = transform.position;
        Vector2 next = start + new Vector2(xDir, yDir);
        //移動先がプレイヤーの移動先と被った場合は攻撃
        if (next == GManager.instance.enemyNextPosition[0])
        {
            enemyAttack(xDir, yDir);
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
     * キャラクターの移動
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        Enemy otherEnemy;
        //boxColliderを無効にして、ラインキャストがこのオブジェクト自身のコライダーに当たらないようにする。
        boxCollider.enabled = false;

        //始点から終点までラインをキャストして、blockingLayerの衝突をチェックします。(ここで自分のオブジェクトとの接触判定が出ないようにfalseしている)
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | enemyLayer | playerLayer | treasureLayer | npcLayer);

        //ラインキャスト後にboxColliderを再度有効にする
        boxCollider.enabled = true;

        //何かがヒットしたかどうかを確認します
        if (hit.transform == null)
        {
            //何もヒットしなかった場合は、Vector2エンドを宛先として渡してSmoothMovementコルーチンを開始します。
            StartCoroutine(SmoothMovement(end));
            return;
        }
        //自分以外の敵にヒットした場合
        if ((otherEnemy = hit.collider.GetComponent<Enemy>()) != null)
        {
            //ヒットした先の敵が移動中の場合
            if (otherEnemy.isMoving)
            {
                StartCoroutine(SmoothMovement(end));
            }
            else
            {
                //ヒットした敵が既に行動している場合は行動させない
                if (otherEnemy.isAction)
                {
                    return;
                }
                otherEnemy.MoveEnemy();
                boxCollider.enabled = false;
                hit = Physics2D.Linecast(transform.position, end,enemyLayer);
                boxCollider.enabled = true;
                otherEnemy = hit.collider.GetComponent<Enemy>();
                //行動させた敵が移動した場合は自身も移動する
                if (hit.transform == null || (otherEnemy != null && otherEnemy.isAction && otherEnemy.isMoving))
                {
                    StartCoroutine(SmoothMovement(end));
                }
            }
        }
    }

    /**
     * 敵が次の移動点に移動できるか判定(既に他の敵の先約がないかチェック)
     */
    protected bool checkNextPosition(Vector2 next)
    {
        for (int i=0;i<GManager.instance.enemyNextPosition.Count;i++)
        {
            if (GManager.instance.enemyNextPosition[i] == next)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * 敵の攻撃処理
     */
    protected void enemyAttack(int x,int y)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(x, y);
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, playerLayer);
        boxCollider.enabled = true;
        if (hit.transform)
        {
            animator.Play("EnemyAttack");
            GManager.instance.playerHp -= enemyAttackValue;
            GManager.instance.wrightAttackLog(enemyName,GManager.instance.playerName,enemyAttackValue);
        }
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