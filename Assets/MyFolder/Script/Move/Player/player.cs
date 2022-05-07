using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    [Header("アイテムレイヤー")] public LayerMask itemLayer;
    [HideInInspector] public Vector2 playerBeforePosition;
    [HideInInspector] public playerState plState = playerState.Normal;
    [HideInInspector] public bool isAttackEnemey = false;
    [HideInInspector] public float restartLevelDelay = 1f;        //レベルを再始動するまでの秒単位の遅延時間。(ステージのこと)
    [HideInInspector] public GameObject levelText;
    [HideInInspector] public int nextHorizontalKey = 1;
    [HideInInspector] public int nextVerticalkey = 0;
    private Animator animator;                    //プレーヤーのアニメーターコンポーネントへの参照を格納するために使用されます。
    private bool isAttack = false;
    private Enemy enemyObject;
    private Treasure treasureObject;
    private bool isDefeat = false;

    public enum playerState
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    protected override void Start()
    {
        //プレーヤーのアニメーターコンポーネントへのコンポーネント参照を取得する
        animator = GetComponent<Animator>();
        //MovingObject基本クラスのStart関数を呼び出します。
        base.Start();
    }


    //この関数は、動作が無効または非アクティブになったときに呼び出されます。（エリア移動の時に呼び出される）
    private void OnDisable()
    {
        //Playerオブジェクトが無効になっている場合は、現在のローカルフードの合計をGameManagerに保存して、次のレベルで再ロードできるようにします。
        //GManager.instance.playerFoodPoints = GManager.instance.playerFoodPoints;
    }


    private void Update()
    {
        //プレイヤーの状態が通常以外
        if (plState != playerState.Normal)
        {
            return;
        }

        //ゲームオーバーまたはメニューオープン時
        if (isDefeat || !GManager.instance.isCloseCommand)
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
            nextHorizontalKey = horizontal > 0 ?1:-1;
            nextVerticalkey = 0;
        }
        else if (vertical != 0)
        {
            horizontal = 0;
            nextVerticalkey = vertical > 0 ?1:-1;
            nextHorizontalKey = 0;
        }
        //水平または垂直にゼロ以外の値があるかどうかを確認します
        if (horizontal != 0 || vertical != 0)
        {
            //プレーヤーを移動する方向を指定するパラメーターとして、水平方向と垂直方向に渡します。
            AttemptMove(horizontal, vertical);
        }
        else if (leftShift && !isAttack)
        {
            Attack();
            isAttack = true;
        }
        else if (!leftShift)
        {
            isAttack = false;
        }
        
    }

    /**
     * プレイヤーの状態を更新
     */
    public void setPlayerState(playerState state)
    {
        plState = state;
    }

    /**
     * キャラクターの移動
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        //boxColliderを無効にして、ラインキャストがこのオブジェクト自身のコライダーに当たらないようにします。
        boxCollider.enabled = false;

        //始点から終点までラインをキャストして、blockingLayerの衝突をチェックします。(ここで自分のオブジェクトとの接触判定が出ないようにfalseしている)
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | enemyLayer | treasureLayer);

        //ラインキャスト後にboxColliderを再度有効にします
        boxCollider.enabled = true;

        //ヒットした場合は移動不可
        if (hit.transform != null)
        {
            return;
        }
        //SmoothMovementコルーチンを開始
        StartCoroutine(SmoothMovement(end));
        //移動後の処理
        playerMoved(end);
    }

    /**
     * 移動後の処理(満腹度の減算等)
     */
    private void playerMoved(Vector2 end)
    {
        //仲間のNPCが存在する場合はプレイヤーの移動前の位置を保存する
        if (GManager.instance.fellows.Count > 0)
        {
            playerBeforePosition = transform.position;
        }
        //プレイヤーが移動するたびに、フードポイントの合計から減算
        GManager.instance.playerFoodPoint--;

        //プレイヤーが移動してフードポイントを失ったので、ゲームが終了したかどうかを確認。
        CheckIfGameOver();
        GManager.instance.enemyNextPosition.Add(end);
        //プレーヤーのターンを終了させる
        GManager.instance.playersTurn = false;
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
        //ゲームが終了したかどうかを確認します
        CheckIfGameOver();
    }


    //CheckIfGameOverは、プレーヤーがフードポイントを超えているかどうかをチェックし、足りない場合はゲームを終了します。
    private void CheckIfGameOver()
    {
        //フードポイントの残りが0より低い、または同じ場合
        if (GManager.instance.playerFoodPoint <= 0 || GManager.instance.playerHp <= 0)
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
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer);
        boxCollider.enabled = true;
        if (hit.transform)
        {
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.layer == Define.ENEMY_LAYER)
            {
                enemyObject = hitObj.GetComponent<Enemy>();
                enemyObject.enemyHp -= GManager.instance.playerAttack;
                GManager.instance.wrightAttackLog(GManager.instance.playerName, enemyObject.enemyName, GManager.instance.playerAttack);
            }
            else if (hit.collider.gameObject.layer == Define.TREASURE_LAYER)
            {
                treasureObject = hitObj.GetComponent<Treasure>();
                treasureObject.treasureHp -= GManager.instance.playerAttack;                
            }
        }
        GManager.instance.playersTurn = false;
    }

    /**
     * アイテムを使用
     */
    public void useItem(GameObject item)
    {
        item.GetComponent<Item>().useItem();
    }

    /**
     * アイテムをプレイヤーの足元に置く
     */
    public void putItemFloor(GameObject item)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position, itemLayer);
        if (hit.transform != null)
        {
            GManager.instance.wrightLog("アイテムを置けません。");
            return;
        }
        Item tempItem = item.GetComponent<Item>();
        GameObject newPutItem = Instantiate(item, new Vector3(transform.position.x,transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newPutItem.SetActive(true);
        newPutItem.GetComponent<Item>().isEnter = true;
        newPutItem.GetComponent<Item>().isPut = true;
        tempItem.deleteSelectedItem(tempItem.id);
    }

    /**
     * アイテムを投げる
     */
    public void throwItem(GameObject item)
    {
        if (item.GetComponent<ThrowObject>() == null)
        {
            Debug.Log("オブジェクトにThrowObjectがついていません");
            return;
        }
        if (item.GetComponent<Rigidbody2D>() == null)
        {
            Debug.Log("オブジェクトにRigidbody2Dがついていません");
            return;
        }
        setPlayerState(playerState.Wait);
        Item tempItem = item.GetComponent<Item>();
        //投げられるアイテムを生成
        GameObject newThrownItemObj = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newThrownItemObj.SetActive(true);
        newThrownItemObj.GetComponent<Item>().isEnter = true;
        ThrowObject th = newThrownItemObj.GetComponent<ThrowObject>();
        //プレイヤーが向いている方向をセット
        th.playerHorizontalKey = nextHorizontalKey;
        th.playerVerticalKey = nextVerticalkey;
        //ThrowObjectのupdateが走る(アイテムが移動する)
        th.isThrownObj = true;
        Item throwItem = newThrownItemObj.GetComponent<Item>();
        //インベントリーから削除
        tempItem.deleteSelectedItem(tempItem.id);
        StartCoroutine(movingItem(th));
    }

    /**
     * アイテムを投げた際のコルーチン
     */
    IEnumerator movingItem(ThrowObject th)
    {
        yield return new WaitUntil(() => !th.isThrownObj);
        //アイテムが画面外に出るか、障害物に当たるまで行動不可
        setPlayerState(playerState.Normal);
        GManager.instance.playersTurn = false;
        yield break;
    }
}