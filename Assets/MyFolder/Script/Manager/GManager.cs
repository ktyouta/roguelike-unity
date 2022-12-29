using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    //宝箱の抽選に用いるアイテムリストのクラス
    class TreasureItem
    {
        public List<Item> itemList;
        public List<Item> itemList2;
        public TreasureItem(List<Item> list, List<Item> list2)
        {
            itemList = list;
            itemList2 = list2;
        }
    }

    //プレイヤーのステータス系
    [HideInInspector] public player playerObj;
    [HideInInspector] public StatusComponentPlayer statusComponentPlayer;

    //パネル制御系
    public GameObject levelText;
    public GameObject grayImage;
    public GameObject mapLoadingImage;
    public Text loadingText;
    public Text mapLoadingText;
    public Text nowStairs;

    //リスト
    //敵ユニットのリスト。
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();
    //NPC用のリスト
    [HideInInspector] public List<NpcFellow> fellows = new List<NpcFellow>();
    //宝箱用のアイテムリスト
    [HideInInspector] public List<Item> treasureItemList = new List<Item>();
    //抽選用アイテムのリスト(TreasureクラスのlotteryIdによりインデックスを切り替える)
    [HideInInspector] public List<List<GameObject>> lotteryitemList = new List<List<GameObject>>();
    //ログ
    [HideInInspector] public List<string> logMessage = new List<string>();
    //移動不可オブジェクトの座標を格納するリスト
    [HideInInspector] public List<Vector2> unmovableList = new List<Vector2>();

    //マネージャー
    private BoardManager boardScript;
    private EventManager eManager;
    private PanelManager pManager;

    //その他
    [Header("レベルアップによるHPの上昇値")] public int riseValueHp;
    //敵が動く際に次の移動点を保持する
    [HideInInspector] public List<Vector2> charsNextPosition = new List<Vector2>();
    [HideInInspector] public bool isCloseCommand = true;
    [HideInInspector] public int hierarchyLevel = 1;
    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public int latestNpcId = 0;
    [HideInInspector] public int enemyActionEndCount;
    [HideInInspector] public bool isEndPlayerAction = false;
    [HideInInspector] public bool isEndPlayerMoving = false;
    [HideInInspector] public int latestEnemyNumber = 0;
    [HideInInspector] public bool doingSetup;
    //レベルを開始する前に待機する時間（秒単位）。
    public float levelStartDelay = 2f;
    //各プレイヤーのターン間の遅延。
    public float turnDelay = 0.05f;
    public static GManager instance = null;
    private bool enemiesMoving;

    //Start is called before the first frame update
    void Awake()
    {
        //インスタンスの確認
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //インスタンスが存在するが、これではない場合
        else if (instance != this)
        {
            //GameManagerのインスタンスは1つしか存在できない
            Destroy(gameObject);
        }

        //ゲームの開始準備
        instance.InitGame();
    }

    //各レベルのゲームを初期化します。
    public void InitGame()
    {
        //リストのリセット
        unmovableList.Clear();
        enemies.Clear();
        doingSetup = true;
        StartCoroutine(settingMapAndEnemies());
    }

    /**
     * 初期設定処理
     */
    private IEnumerator settingMapAndEnemies()
    {
        //パネルのインスタンスを取得
        mapLoadingImage = GameObject.Find("MapLoadingImage");
        if (mapLoadingImage != null)
        {
            //現在の階数を画面に表示
            mapLoadingText = GameObject.Find("MapLoadingText").GetComponent<Text>();
            mapLoadingText.text = instance.hierarchyLevel + " F";
        }

        nowStairs = GameObject.Find("NowStairs").GetComponent<Text>();

        //マネージャーのインスタンスを取得
        //マップのランダム生成
        boardScript = GetComponent<BoardManager>();
        boardScript.Init();

        //パネルマネージャー
        pManager = GetComponent<PanelManager>();
        pManager.Init();

        //イベントマネージャー
        eManager = GetComponent<EventManager>();

        // 生成された敵オブジェクトを取得
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        statusComponentPlayer = playerObj.GetComponent<StatusComponentPlayer>();
        //作成したエネミーにidを割り振り、リストに格納する
        for (var i = 0; i < enemyObjects.Length; i++)
        {
            AddEnemyToList(enemyObjects[i], i);
        }
        yield return new WaitForSeconds(2.0f);
        //処理完了後に画面表示
        hideLevelImage();
    }

    //初期表示の黒い画像を非表示にする
    public void hideLevelImage()
    {
        mapLoadingImage.SetActive(false);
        doingSetup = false;
        mapLoadingText.text = "";
        if (nowStairs != null)
        {
            nowStairs.text = hierarchyLevel + "F";
        }
    }

    //更新はフレームごとに呼び出されます。
    void Update()
    {
        // セットアップ中
        if (doingSetup)
        {
            return;
        }
        // プレイヤーのターンまたは敵の行動中
        if (playersTurn || enemiesMoving)
        {
            //NPCおよび敵の移動を開始しない。
            return;
        }
        //仲間のNPCが存在する場合
        if (fellows.Count > 0)
        {
            Vector2 tmpRefPosition = playerObj.playerBeforePosition;
            for (int i = 0; i < fellows.Count; i++)
            {
                //前方のキャラの位置を代入(先頭の場合はプレイヤーの位置)
                Vector2 refPosition = tmpRefPosition;
                fellows[i].fellowAction(refPosition);
                tmpRefPosition = fellows[i].npcBeforePosition;
            }

        }
        //敵を行動させる
        StartCoroutine(moveEnemies());
    }


    //hpが0になった敵をリストから削除
    public void removeEnemyToList(Enemy targetEnemy)
    {
        enemies.Remove(targetEnemy);
    }

    //これを呼び出して、渡された敵を敵オブジェクトのリストに追加します。
    public void AddEnemyToList(GameObject enemy,int enemyNumber)
    {
        Enemy enemyObj = enemy.GetComponent<Enemy>();
        //enemyObj.enemyNumber = enemyNumber;
        //string enemyName = enemy.GetComponent<StatusComponentBase>().charName.name;
        //if (string.IsNullOrEmpty(enemyName))
        //{
        //    enemyName = "エネミー";
        //}
        //enemy.GetComponent<StatusComponentBase>().charName.name = enemyName + (enemyNumber + 1);
        ////enemyObj.enemyName = "エネミー" + (enemyNumber + 1);
        ////Listにエネミーを追加する
        enemies.Add(enemyObj);
        latestEnemyNumber++;
    }


    //プレイヤーがフードポイント0に到達すると、GameOverが呼び出されます
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        //levelText.text = "After " + level + " days, you starved.";
        levelText.SetActive(true);

        //Enable black background image gameObject.
        //levelImage.SetActive(true);

        //boardScript.destroyBoard();
        //このGameManagerを無効にします。
        enabled = false;
    }

    /**
     * 敵を順番に動かすコルーチン
     */
    IEnumerator moveEnemies()
    {
        enemyActionEndCount = 0;
        //プレイヤーは移動できない
        enemiesMoving = true;
        //敵オブジェクトのリストをループ
        for (int i = 0; i < enemies.Count; i++)
        {
            //isActionがtrue(既に行動している)の場合は行動させない
            if (enemies[i].isAction)
            {
                continue;
            }
            //yield return null;
            if (i > enemies.Count -1)
            {
                break;
            }
            //敵リストのインデックスiにある敵のmoveEnemy関数を呼び出す
            enemies[i].moveEnemy();
        }
        Invoke("forciblyAdvanceTurn", 1.5f);
        //全ての敵が行動を終えるまで待つ
        yield return new WaitUntil(() => enemyActionEndCount >= enemies.Count);
        CancelInvoke();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].isAction = false;
        }
        //敵の移動が完了したら、playersTurnをtrueに設定して、プレーヤーが移動できるようにする
        playersTurn = true;
        //プレイヤーの攻撃フラグをオフにする
        //playerObj.isAttack = false;
        //敵の移動が完了したら、enemiesMovingをfalseに設定
        enemiesMoving = false;
        //移動点を空にする
        charsNextPosition.Clear();
        //ゲーム内イベント制御
        if (eManager.skeltonAppearanceFlg)
        {
            eManager.skeltonAppearanceEventTurnNum++;
        }
        if (eManager.ghostAppearanceFlg)
        {
            eManager.ghostAppearanceEventTurnNum++;
        }
        eManager.nomalEnemyAppearanceTurnNum++;
    }

    /*
     * 何らかの問題で敵のターンが終了しなかった場合に強制的にターンを進める
     */
    private void forciblyAdvanceTurn()
    {
        StartCoroutine(waitTurnFinish());
    }

    private IEnumerator waitTurnFinish()
    {
        grayImage.SetActive(true);
        loadingText.text = "待機中...";
        yield return new WaitForSeconds(1.0f);
        //全ての敵の行動が完了していない場合は強制的にターンを進める
        if (enemyActionEndCount < enemies.Count)
        {
            enemyActionEndCount = enemies.Count;
        }
        grayImage.SetActive(false);
    }

    public void wrightLog(string message)
    {
        deleteLog();
        logMessage.Add(message);
    }

    private void deleteLog()
    {
        if (logMessage.Count > 5)
        {
            logMessage.Clear();
        }
    }

}
