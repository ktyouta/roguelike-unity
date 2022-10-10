using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    [HideInInspector] public StatusComponentPlayer statusObj;

    [Header("アイテムレイヤー")] public LayerMask itemLayer;
    [HideInInspector] public Vector2 playerBeforePosition;
    [HideInInspector] public playerState plState = playerState.Normal;
    [HideInInspector] public float restartLevelDelay = 1f;        //レベルを再始動するまでの秒単位の遅延時間。(ステージのこと)
    [HideInInspector] public GameObject levelText;
    [HideInInspector] public int nextHorizontalKey = 1;
    [HideInInspector] public int nextVerticalkey = 0;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public Enemy enemyObject;
    [HideInInspector] public int horizontal = 0;
    [HideInInspector] public int vertical = 0;
    [HideInInspector] public bool leftShift = false;
    private Treasure treasureObject;
    private bool isDefeat = false;
    private int reduceFoodCounter = 0;

    public enum playerState
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    //この関数は、動作が無効または非アクティブになったときに呼び出されます。（エリア移動の時に呼び出される）
    private void OnDisable()
    {
        //Playerオブジェクトが無効になっている場合は、現在のローカルフードの合計をGameManagerに保存して、次のレベルで再ロードできるようにします。
        //GManager.instance.playerFoodPoints = GManager.instance.playerFoodPoints;
    }

    protected override void Start()
    {
        base.Start();
        // 1Fの時のみ取得し、以降は前のシーンから引き継ぐ
        setPlayerStatus();
    }

    /**
     * キャラクターのステータスを取得
     */
    protected virtual void setPlayerStatus()
    {
        // キャストする型をキャラクターごとに変える
        statusObj = (StatusComponentPlayer)GetComponent<StatusComponentBase>();
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

        //プレイヤーのターンでない、移動中、攻撃中はコマンド入力を受け付けない
        if (!GManager.instance.playersTurn || isMoving || isAttack)
        {
            return;
        }
        GManager.instance.isEndPlayerAction = false;
        horizontal = 0;      //水平移動方向を格納するために使用されます
        vertical = 0;        //垂直移動方向を格納するために使用されます。
        leftShift = false;       //攻撃

        //入力マネージャーから入力を取得し、整数に丸め、水平に保存してx軸の移動方向を設定します
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        //入力マネージャーから入力を取得し、整数に丸め、垂直に保存してy軸の移動方向を設定します
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        //攻撃
        leftShift = Input.GetKeyDown("left shift");
        //プレイヤーを行動させる
        playerMove(horizontal,vertical,leftShift);
    }

    public void playerMove(int inHorizontal,int inVertical,bool inLeftShift)
    {
        //水平に移動するかどうかを確認し、移動する場合は垂直にゼロに設定します。(ズレ防止)
        if (inHorizontal != 0)
        {
            nextHorizontalKey = inHorizontal > 0 ? 1 : -1;
            nextVerticalkey = 0;
        }
        else if (inVertical != 0)
        {
            nextVerticalkey = inVertical > 0 ? 1 : -1;
            nextHorizontalKey = 0;
        }
        //水平または垂直にゼロ以外の値があるかどうかを確認します
        if (inHorizontal != 0 || inVertical != 0)
        {
            //Debug.Log("inHorizontal" + inHorizontal);
            //Debug.Log("inVertical" + inVertical);
            //プレーヤーを移動する方向を指定するパラメーターとして、水平方向と垂直方向に渡します。
            AttemptMove(inHorizontal, inVertical);
        }
        //攻撃
        else if (inLeftShift)
        {
            Attack();
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
        //始点から終点までラインをキャストして、blockingLayerの衝突をチェックします。(ここで自分のオブジェクトとの接触判定が出ないようにfalseしている)
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | enemyLayer | treasureLayer | npcLayer);
        //ヒットした場合は移動不可
        if (hit.transform != null)
        {
            return;
        }
        //SmoothMovementコルーチンを開始
        StartCoroutine(playerSmoothMovement(end));
        //移動後の処理
        playerMoved(end);
    }

    private IEnumerator playerSmoothMovement(Vector2 end)
    {
        yield return SmoothMovement(end);
        //移動完了後にフラグをオンにする
        GManager.instance.isEndPlayerAction = true;
    }

    /**
     * 移動後の処理(満腹度の減算等)
     * ※playerSmoothMovementのコルーチンと同時に実行される
     */
    private void playerMoved(Vector2 end)
    {
        //プレイヤーの移動前の位置を保存する
        playerBeforePosition = transform.position;
        reduceFoodCounter++;
        //プレイヤーが移動するたびに、フードポイントの合計から減算
        if (reduceFoodCounter == 5)
        {
            statusObj.charFood.subFoodPoint(1);
            reduceFoodCounter = 0;
        }

        //プレイヤーが移動してフードポイントを失ったので、ゲームが終了したかどうかを確認。
        CheckIfGameOver();
        //プレイヤーの位置情報は必ずリストの先頭になる
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
        //if (playerStatusObj.playerFoodPoint <= 0 || playerStatusObj.playerHp <= 0)
        if (statusObj.charFood.foodPoint <= 0 || statusObj.charHp.hp <= 0)
        {
            //GManager.instance.wrightDeadLog(playerStatusObj.playerName);
            GManager.instance.wrightDeadLog(statusObj.charName.name);
            //GameManagerのGameOver関数を呼び出します。
            GManager.instance.GameOver();
            isDefeat = true;
        }
    }

    /**
     * プレイヤーの攻撃
     */
    public void Attack()
    {
        animator.Play("PlayerAttack");
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(nextHorizontalKey, nextVerticalkey);
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer | blockingLayer);
        //攻撃の場合は現在地を追加
        GManager.instance.enemyNextPosition.Add(transform.position);
        //時間差で実行されるフラグをオンにする処理
        StartCoroutine(waitAttackEnemy());
        isAttack = true;
        //ヒットしていない場合
        if (hit.transform == null)
        {
            return;
        }
        //対象オブジェクトのダメージ処理を行う
        OutAccessComponentBase outAccessObj = hit.transform.gameObject?.GetComponent<OutAccessComponentBase>();
        if (outAccessObj == null)
        {
            return;
        }
        //ダメージ処理
        outAccessObj.callCalculateDamage(statusObj.charAttack.totalAttack, statusObj.charName.name);
    }

    /*
     * 攻撃コマンド入力後に一定時間待ってターンを終了する
     */
    private IEnumerator waitAttackEnemy()
    {
        //攻撃に関しては攻撃後にターンを終了する
        yield return new WaitForSeconds(0.3f);
        GManager.instance.isEndPlayerAction = true;
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
        GameObject newPutItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
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
            return;
        }
        if (item.GetComponent<Rigidbody2D>() == null)
        {
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