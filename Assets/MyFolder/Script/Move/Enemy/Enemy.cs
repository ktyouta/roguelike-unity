using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;                             //攻撃時にプレイヤーから差し引くフードポイントの量。
    private Animator animator;                            //敵のAnimatorコンポーネントへの参照を格納するAnimator型の変数。
    private Transform target;                            //各ターンに移動しようとする目的object
    private bool skipMove;                                //敵がターンをスキップするか、このターンを移動するかどうかを決定するブール値。
    [HideInInspector] public int enemyNumber;            //敵に付与される連番
    public int enemyHp = 10;
    private bool isDefeatEnemy = false;
    private player playerObj;
    [Header("エネミーの攻撃力")] public int enemyAttackValue;
    [Header("エネミーの所持金")] public int enemyMoney; 
    [HideInInspector] public string enemyName;
    [Header("エネミーを倒した際の経験値")] public int experiencePoint;
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
            enemiesDefeat();
        }
    }

    //Enemyがターンをスキップするために必要な機能を含めるには、MovingObjectのAttemptMove関数をオーバーライドします。
    //基本的なAttemptMove関数の動作の詳細については、MovingObjectのコメントを参照してください。
    protected override void AttemptMove(int xDir, int yDir)
    {
        //skipMoveがtrueかどうかを確認し、trueの場合はfalseに設定して、このターンをスキップします。
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        //MovingObjectからAttemptMove関数を呼び出します。
        base.AttemptMove(xDir, yDir);

        //Enemyが移動したので、skipMoveをtrueに設定して次の移動をスキップします。
        //skipMove = true;
    }
    //MoveEnemyは毎ターンGameMangerによって呼び出され、各敵にプレイヤーに向かって移動するように指示します。
    public void MoveEnemy()
    {
        //画面内にいる場合のみ移動
        if (!sr.isVisible)
        {
            return;
        }
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

        //Debug.Log("enemyMove");
        //エネミーは移動していて、プレーヤーに遭遇する可能性があるため、AttemptMove関数を呼び出してジェネリックパラメーターPlayerを渡します。
        AttemptMove(xDir, yDir);
        if (!canMove)
        {
            enemiesAttack(xDir, yDir);
        }
        
    }


    // OnCantMoveは、Enemyがプレーヤーが占有するスペースに移動しようとすると呼び出され、MovingObjectのOnCantMove関数をオーバーライドします
    //また、遭遇すると予想されるコンポーネント、この場合はPlayerに渡すために使用する汎用パラメーターTを受け取ります
    protected override void OnCantMove<T>(T component)
    {
        //hitPlayerを宣言し、遭遇したコンポーネントと等しくなるように設定します。
        player hitPlayer = component as player;

        //hitPlayerのLoseFood関数を呼び出して、減算するフードポイントの量であるplayerDamageを渡します。
        hitPlayer.LoseFood(playerDamage);

        //アニメータの攻撃トリガーを設定して、敵の攻撃アニメーションをトリガーします。
        animator.SetTrigger("EnemyAttack");

    }

    protected void enemiesAttack(int x,int y)
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

    protected void enemiesDefeat()
    {
        GManager.instance.enemyDefeatNum = enemyNumber;
        GManager.instance.wrightDeadLog(enemyName);
        isDefeatEnemy = true;
        GManager.instance.playerMoney += enemyMoney;
        GManager.instance.beforeLevelupExperience = GManager.instance.nowExprience;
        GManager.instance.nowExprience += experiencePoint;
        GManager.instance.mostRecentExperience = experiencePoint;
        Destroy(gameObject, 0.5f);
    }
}