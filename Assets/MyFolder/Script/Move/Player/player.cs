using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    // プレイヤーのステータス用オブジェクト
    [HideInInspector] public StatusComponentPlayer statusObj;

    [HideInInspector] public Vector2 playerBeforePosition;
    [HideInInspector] public playerState plState = playerState.Normal;
    [HideInInspector] public Enemy enemyObject;
    [HideInInspector] public int horizontal = 0;
    [HideInInspector] public int vertical = 0;
    [HideInInspector] public bool leftShift = false;
    private bool isDefeat = false;
    private int reduceFoodCounter = 0;

    // 機能コンポーネント
    [HideInInspector] public AttackComponentBase attackComponentObj;
    [HideInInspector] public ThrowComponentBase throwComponentObj;
    [HideInInspector] public UseItemComponentBase useItemComponentObj;

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

        //機能コンポーネントをセット
        //ステータスコンポーネント
        statusObj = GetComponent<StatusComponentPlayer>();

        //攻撃用コンポーネント
        attackComponentObj = GetComponent<AttackComponentBase>();
        if (attackComponentObj == null)
        {
            gameObject.AddComponent<AttackComponentPlayer>();
            attackComponentObj = GetComponent<AttackComponentBase>();
        }

        //アイテムの投擲用コンポーネント
        throwComponentObj = GetComponent<ThrowComponentBase>();
        if (throwComponentObj == null)
        {
            gameObject.AddComponent<ThrowComponentPlayer>();
            throwComponentObj = GetComponent<ThrowComponentBase>();
        }

        //アイテム使用のコンポーネント
        useItemComponentObj = GetComponent<UseItemComponentBase>();
        if (useItemComponentObj == null)
        {
            gameObject.AddComponent<UseItemComponentPlayer>();
            useItemComponentObj = GetComponent<UseItemComponentBase>();
        }

        //ダメージアクションコンポーネント
        if (GetComponent<DamageActionComponentBase>() == null)
        {
            gameObject.AddComponent<DamageActionComponentPlayer>();
        }

        //外部アクセス用コンポーネント
        if (GetComponent<OutAccessComponentBase>() == null)
        {
            gameObject.AddComponent<OutAccessComponentBase>();
        }

        //リスポーン時に下方向を向く
        nextHorizontalKey = 0;
        nextVerticalkey = -1;
    }

    private void Update()
    {
        CheckIfGameOver();

        if (!isAttack)
        {
            if (nextHorizontalKey > 0)
            {
                animator?.Play("PlayerRightWalk");
            }
            else if (nextHorizontalKey < 0)
            {
                animator?.Play("PlayerLeftWalk");
            }
            else if (nextVerticalkey > 0)
            {
                animator?.Play("PlayerUpWalk");
            }
            else if (nextVerticalkey < 0)
            {
                animator?.Play("PlayerDownWalk");
            }
        }
        
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
        //プレイヤーの状態が通常以外
        if (plState != playerState.Normal)
        {
            return;
        }
        //ゲームオーバーまたはメニューオープン時
        if (isDefeat)
        {
            return;
        }
        //プレイヤーのターンでない、移動中、攻撃中は行動不可
        if (!GManager.instance.playersTurn || isMoving || isAttack)
        {
            return;
        }

        GManager.instance.isEndPlayerAction = false;
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
            //プレーヤーを移動する方向を指定するパラメーターとして、水平方向と垂直方向に渡します。
            AttemptMove(inHorizontal, inVertical);
        }
        //攻撃
        else if (inLeftShift)
        {
            StartCoroutine(attackComponentObj?.attack(nextHorizontalKey, nextVerticalkey));
            //attackComponentObj?.attack(nextHorizontalKey, nextVerticalkey);
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
        hit = Physics2D.Linecast(transform.position, end, LayerUtil.blockingLayer | LayerUtil.enemyLayer | LayerUtil.treasureLayer | LayerUtil.npcLayer);
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
        yield return StartCoroutine(SmoothMovement(end));
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
        GManager.instance.charsNextPosition.Add(end);
        //プレーヤーのターンを終了させる
        GManager.instance.playersTurn = false;
    }

    //CheckIfGameOverは、プレーヤーがフードポイントを超えているかどうかをチェックし、足りない場合はゲームを終了します。
    private void CheckIfGameOver()
    {
        //フードポイントの残りが0より低い、または同じ場合
        //if (playerStatusObj.playerFoodPoint <= 0 || playerStatusObj.playerHp <= 0)
        if (statusObj.charFood.foodPoint <= 0 || statusObj.charHp.hp <= 0)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("6", statusObj.charName.name));
            GManager.instance.GameOver();
            isDefeat = true;
            Destroy(gameObject);
        }
    }

    /**
     * アイテムを使用
     */
    public void useItem(Item item)
    {
        useItemComponentObj.use(item,statusObj);
    }

    /**
     * アイテムをプレイヤーの足元に置く
     */
    public void putItemFloor(GameObject item)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position, LayerUtil.itemLayer);
        if (hit.transform != null)
        {
            LogMessageManager.wrightLog(MessageManager.createMessage("16"));
            return;
        }
        Item tempItem = item.GetComponent<Item>();
        //プレイヤーの足元にアイテムを生成する
        GameObject newPutItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newPutItem.SetActive(true);
        newPutItem.GetComponent<Item>().isPut = true;
        tempItem.deleteSelectedItem(tempItem.id);
    }

    /**
     * アイテムを投げる
     */
    public void throwItem(GameObject item)
    {
        StartCoroutine(throwComponentObj.throwObject(nextVerticalkey, nextHorizontalKey, item));
    }
}