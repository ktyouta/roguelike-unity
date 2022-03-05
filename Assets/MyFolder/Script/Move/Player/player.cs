using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    public float restartLevelDelay = 1f;        //レベルを再始動するまでの秒単位の遅延時間。(ステージのこと)
    public int pointsPerFood = 10;                //フードオブジェクトを拾うときにプレーヤーのフードポイントに追加するポイントの数。
    public int wallDamage = 1;                    //プレイヤーが壁を割ったときに壁に与えるダメージ。
    public Text foodText;
    public Text playerHpText;
    //[Header("敵オブジェクト")] public LayerMask enemyLayer;
    //[Header("プレイヤーの攻撃力")] public int playerAttackValue;
   
    //private BoxCollider2D boxCollider;
    private Animator animator;                    //プレーヤーのアニメーターコンポーネントへの参照を格納するために使用されます。
    private int food;                       //レベル中(このステージ中)にプレイヤーのフードポイントの合計を保存するために使用されます。
    private bool isAttack = false;
    private int nextHorizontalKey = 1;
    private int nextVerticalkey = 0;
    private Enemy enemyObject;
    private Treasure treasureObject;
    public GameObject levelText;
    private bool isDefeat = false;
    private int nowPlayerState = 0;
    playerState plState = playerState.Normal;

    public enum playerState
    {
        Normal,
        Talk,
        Command,
    }

    //MovingObjectのStart関数をオーバーライドします
    protected override void Start()
    {
        //プレーヤーのアニメーターコンポーネントへのコンポーネント参照を取得する
        animator = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
        //レベル間のGameManager.instanceに保存されている現在のフードポイントの合計を取得します。
        food = GManager.instance.playerFoodPoints;
        //Debug.Log(food);
        foodText = GameObject.Find("Food").GetComponent<Text>();
        foodText.text = "Food:" + GManager.instance.playerFoodPoints;
        playerHpText = GameObject.Find("PlayerHp").GetComponent<Text>();
        playerHpText.text = "HP:" + GManager.instance.playerHp;

        //levelText = GameObject.Find("LevelText");
        //Debug.Log("player" + levelText);

        //MovingObject基本クラスのStart関数を呼び出します。
        base.Start();
    }


    //この関数は、動作が無効または非アクティブになったときに呼び出されます。（エリア移動の時に呼び出される）
    private void OnDisable()
    {
        //Playerオブジェクトが無効になっている場合は、現在のローカルフードの合計をGameManagerに保存して、次のレベルで再ロードできるようにします。
        GManager.instance.playerFoodPoints = GManager.instance.playerFoodPoints;
    }


    private void Update()
    {
        //プレイヤーの状態が通常
        if (plState == playerState.Normal)
        {
            //ゲームオーバーまたはメニューオープン時
            if (isDefeat || GManager.instance.isMenuOpen)
            {
                return;
            }
            CheckIfGameOver();

            //レベルアップ
            if (GManager.instance.nowExprience >= GManager.instance.nowMaxExprience)
            {
                GManager.instance.updateLevel();
                GManager.instance.updateStatus();
            }
            //Debug.Log(GManager.instance.playersTurn);
            //プレイヤーの番でない場合、関数を終了します。
            if (!GManager.instance.playersTurn)
            {
                return;
            }

            int horizontal = 0;      //水平移動方向を格納するために使用されます
            int vertical = 0;        //垂直移動方向を格納するために使用されます。
            bool leftShift = false;       //攻撃

            //入力マネージャーから入力を取得し、整数に丸め、水平に保存してx軸の移動方向を設定します
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            //入力マネージャーから入力を取得し、整数に丸め、垂直に保存してy軸の移動方向を設定します
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            //攻撃
            leftShift = Input.GetKey("left shift");

            //水平に移動するかどうかを確認し、移動する場合は垂直にゼロに設定します。(ズレ防止)
            if (horizontal != 0)
            {
                vertical = 0;
                nextHorizontalKey = 1;
                if (horizontal < 0)
                {
                    nextHorizontalKey *= (-1);
                }
                nextVerticalkey = 0;
            }
            else if (vertical != 0)
            {
                horizontal = 0;
                nextVerticalkey = 1;
                if (vertical < 0)
                {
                    nextVerticalkey *= (-1);
                }
                nextHorizontalKey = 0;
            }
            //水平または垂直にゼロ以外の値があるかどうかを確認します
            if (horizontal != 0 || vertical != 0)
            {
                //ジェネリックパラメーターWallを渡してAttemptMoveを呼び出します。
                //これは、プレイヤーが壁に遭遇した場合プレイヤーがwallクラスを操作するためです。
                //プレーヤーを移動する方向を指定するパラメーターとして、水平方向と垂直方向に渡します。
                AttemptMove(horizontal, vertical);
            }
            else if (leftShift && !isAttack)
            {
                //Debug.Log("leftshift"+leftShift);
                Attack();
                isAttack = true;
            }
            else if (!leftShift)
            {
                isAttack = false;
            }
        }
    }

    public void setPlayerState(playerState state)
    {
        plState = state;
    }

    // AttemptMoveは、基本クラスMovingObjectのAttemptMove関数をオーバーライドします
    // AttemptMoveはジェネリックパラメーターTを受け取ります。これは、プレーヤーの場合はWallタイプであり、xおよびy direcの整数も受け取ります
    protected override void AttemptMove(int xDir, int yDir)
    {
        //基本クラスのAttemptMoveメソッドを呼び出し、コンポーネントT（この場合はWall）と移動するxおよびy方向を渡します。
        base.AttemptMove(xDir, yDir);
        if (canMove)
        {
            //プレイヤーが移動するたびに、フードポイントの合計から減算します。
            //food--;
            //GManager.instance.playerFoodPoints = food;
            GManager.instance.playerFoodPoints--;
            foodText.text = "Food:" + GManager.instance.playerFoodPoints;
            //ヒットにより、Moveで行われたLinecastの結果を参照できます。
            RaycastHit2D hit;

            //プレイヤーが移動してフードポイントを失ったので、ゲームが終了したかどうかを確認します。
            CheckIfGameOver();

            //プレーヤーのターンが終わったので、GameManagerのplayersTurnブール値をfalseに設定します。
            GManager.instance.playersTurn = false;
        }
    }


    //OnCantMoveは、MovingObjectの抽象関数OnCantMoveをオーバーライドします。
    //これは、プレーヤーの場合はプレーヤーが攻撃して破壊できる壁である一般的なパラメーターTを取ります。
    protected override void OnCantMove<T>(T component)
    {
        //hitWallを、パラメーターとして渡されたコンポーネントと等しくなるように設定します。
        //Wall hitWall = component as Wall;

        //攻撃している壁のDamageWall関数を呼び出します。
        //hitWall.DamageWall(wallDamage);

        //プレーヤーの攻撃アニメーションを再生するには、プレーヤーのアニメーションコントローラーの攻撃トリガーを設定します。
        animator.SetTrigger("chop");
    }

    //Playerとトリガーがぶつかった時
    //OnTriggerEnter2Dは、トリガー設定したオブジェクトとぶつかると呼び出される
    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがExitであるか確認してください。
        if (other.tag == "Exit")
        {
            //Debug.Log(levelText);
            //levelText.SetActive(true);
            //1秒後に次のレベル（ステージ）を開始するために、Restart関数を呼び出します。
            Invoke("Restart", restartLevelDelay);

            //レベルが終わったので、プレーヤーオブジェクトを無効にします。
            enabled = false;
        }

        ////衝突したトリガーのタグがFoodであるか確認してください。
        //else if (other.tag == "Food")
        //{
        //    //プレイヤーの現在のフードの合計にpointsPerFoodを追加します。
        //    food += pointsPerFood;

        //    //食べ物を拾った時に加算して表示
        //    foodText.text = "+" + pointsPerFood + "Food:" + food;

        //    //アイテム取得後、非表示
        //    other.gameObject.SetActive(false);
        //    GManager.instance.addItem(other.gameObject);
        //}
    }

    //Restartは呼び出されたときにシーンをリロードします。
    private void Restart()
    {
        //最後にロードされたシーンをロードします。この場合はMain、ゲーム内の唯一のシーンです。 そして、それを「シングル」モードでロードして、既存のものを置き換えます
        //現在のシーンのすべてのシーンオブジェクトをロードしません。
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //SceneManager.LoadScene("Main");
    }


    //LoseFoodは、敵がプレイヤーを攻撃したときに呼び出されます。
    //失うポイントの数を指定するパラメーター損失をとります。
    public void LoseFood(int loss)
    {
        //プレーヤーアニメーターのトリガーを設定して、playerHitアニメーションに遷移します。
        animator.SetTrigger("hit");

        //プレイヤーの合計から失われたフードポイントを差し引きます。
        food -= loss;

        foodText.text = "-" + loss + "Food:" + food;

        //ゲームが終了したかどうかを確認します
        CheckIfGameOver();
    }


    //CheckIfGameOverは、プレーヤーがフードポイントを超えているかどうかをチェックし、足りない場合はゲームを終了します。
    private void CheckIfGameOver()
    {
        //フードポイントの残りが0より低い、または同じ場合
        if (GManager.instance.playerFoodPoints <= 0 || GManager.instance.playerHp <= 0)
        {
            GManager.instance.wrightDeadLog(GManager.instance.playerName);
            //GameManagerのGameOver関数を呼び出します。
            GManager.instance.GameOver();
            isDefeat = true;
        }
    }

    /**
     * 攻撃
     */
    protected void Attack()
    {
        animator.Play("PlayerAttack");
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(nextHorizontalKey, nextVerticalkey);
        boxCollider.enabled = false;
        //Debug.Log("start" + start);
        //Debug.Log("end" + end);
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer);
        boxCollider.enabled = true;
        if (hit.transform)
        {
            Debug.Log(hit.collider.gameObject.layer);
            Debug.Log(hit.collider.gameObject.name);
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.layer == Define.ENEMY_LAYER)
            {
                enemyObject = hitObj.GetComponent<Enemy>();
                int enemyHp = enemyObject.enemyHp;
                //Debug.Log("playerAttack: " + enemyHp);
                enemyObject.enemyHp -= GManager.instance.playerAttack;
                //Debug.Log("playerAttack: " + enemyObject.enemyHp);
                GManager.instance.wrightAttackLog(GManager.instance.playerName, enemyObject.enemyName, GManager.instance.playerAttack);
            }
            else if (hit.collider.gameObject.layer == Define.TREASURE_LAYER)
            {
                treasureObject = hitObj.GetComponent<Treasure>();
                if (!treasureObject.isOpen)
                {
                    Debug.Log("treasurehp"+treasureObject.treasureHp);
                    treasureObject.treasureHp -= GManager.instance.playerAttack;
                }
            }
        }
        GManager.instance.playersTurn = false;
    }
}