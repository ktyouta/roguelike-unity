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

    public enum EnemyType
    {
        Skelton1,
        Ghost1
    }

    //プレイヤーのステータス系
    [Header("プレイヤーのHP")] public int playerHp;
    [Header("プレイヤーの現在の上限HP")] public int nowPlayerMaxHp = 100;
    [Header("プレイヤーの名前")] public string playerName;
    [Header("プレイヤーの所持金")] public int playerMoney;
    [Header("プレイヤーの攻撃力")] public int playerAttack;
    [Header("プレイヤーの防御力")] public int playerDefence;
    [Header("プレイヤーのレベル")] public int playerLevel = 1;
    [Header("プレイヤーの魅力")] public int playerCharm = 10;
    [Header("次のレベルまでの経験値")] public int nowMaxExprience;
    [Header("アイテムの所持数制限")] public int nowMaxPosession;
    [Header("プレイヤーの満腹度")] public int playerFoodPoint;
    [Header("プレイヤーの満腹度の上限値")] public int playerMaxFoodPoint = 100;
    [HideInInspector] public int mostRecentExperience;
    [HideInInspector] public int beforeLevelupExperience;
    [HideInInspector] public int nowExprience;
    [HideInInspector] public player playerObj;

    //パネル制御系
    [Header("インベントリーに展開されるアイテムボタン")] public GameObject itemBtn;
    [Header("インベントリーの子オブジェクト(テキスト)")] public Text itemPanel;
    public GameObject levelText;
    public GameObject levelImage;
    public GameObject commandPanel;
    public GameObject statusText;
    public GameObject itemText;
    public GameObject itemUsePanel;
    public GameObject npcWindowImage;
    public GameObject npcImage;
    public GameObject choisePanel;
    public GameObject shopPanel;
    public GameObject shopItemListPanel;
    public GameObject shopSelectPanel;
    public GameObject itemDescriptionPanel;
    public GameObject grayImage;
    public GameObject mapLoadingImage;
    public Text playerStatusPanel;
    public Text npcMessageText;
    public Text npcNameText;
    public Text playerMoneyText;
    public Text loadingText;
    public Text mapLoadingText;
    public Text nowStairs;
    private Button statusButton;
    private Button itemButton;
    private Button closeButton;
    private Button manualButton;

    //リスト
    //敵ユニットのリスト。
    public List<Enemy> enemies = new List<Enemy>();
    //NPC用のリスト
    public List<NpcFellow> fellows = new List<NpcFellow>();
    //宝箱用のアイテムリスト
    public List<Item> treasureItemList = new List<Item>();
    //抽選用アイテムのリスト(TreasureクラスのlotteryIdによりインデックスを切り替える)
    public List<List<GameObject>> lotteryitemList = new List<List<GameObject>>();
    //インベントリー内のリスト
    public List<GameObject> itemList = new List<GameObject>();
    //ログ
    [HideInInspector] public List<string> logMessage = new List<string>();
    //移動不可オブジェクトの座標を格納するリスト
    [HideInInspector] public List<Vector2> unmovableList = new List<Vector2>();

    //ゲーム内イベント用
    EventManager eManager;

    //その他
    [Header("レベルアップによるHPの上昇値")] public int riseValueHp;
    //敵が動く際に次の移動点を保持する
    [HideInInspector] public List<Vector2> enemyNextPosition = new List<Vector2>();
    [HideInInspector] public bool isCloseCommand = true;
    [HideInInspector] public int hierarchyLevel = 1;
    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public int latestNpcId = 0;
    [HideInInspector] public int enemyActionEndCount;
    [HideInInspector] public bool isEndPlayerAction = false;
    [HideInInspector] public int latestEnemyNumber = 0;
    //レベルを開始する前に待機する時間（秒単位）。
    public float levelStartDelay = 2f;
    //各プレイヤーのターン間の遅延。
    public float turnDelay = 0.05f;
    public static GManager instance = null;
    public BoardManager boardScript;
    public string weaponName = "なし";
    public string shieldName = "なし";
    private bool enemiesMoving;
    private bool doingSetup;
    private bool loadFlg = false;
    private IEnumerator coroutine;
    private bool isSwitchPlayerStatus;

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

        if (!instance.loadFlg)
        {
            //シーンをリロードするときにこれが破棄されないように設定します

            InitGame();
            instance.loadFlg = true;
        }

    }
    //これは1回だけ呼び出され、パラメータはシーンがロードされた後にのみ呼び出されるように指示します//（そうでない場合、Scene Loadコールバックは最初のロードと呼ばれ、必要ありません）
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //シーンが読み込まれるたびに呼び出されるコールバックを登録します
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
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
            mapLoadingText.text = hierarchyLevel + " F";
        }
        levelText = GameObject.Find("LevelText");
        commandPanel = GameObject.Find("CommandPanel");
        statusText = GameObject.Find("StatusPanel");
        itemText = GameObject.Find("ItemPanel");
        statusButton = GameObject.Find("StatusButton").GetComponent<Button>();
        statusButton.onClick.AddListener(() => openStatus());
        itemButton = GameObject.Find("ItemButton").GetComponent<Button>();
        itemButton.onClick.AddListener(() => openItem());
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(() => closeMenu());
        itemUsePanel = GameObject.Find("ItemUseList");
        itemDescriptionPanel = GameObject.Find("ItemDescriptionPanel");
        npcWindowImage = GameObject.FindWithTag("NpcTalkPanel");
        npcImage = GameObject.FindWithTag("NpcImage");
        choisePanel = GameObject.FindWithTag("ChioseMessagePanel");
        shopPanel = GameObject.FindWithTag("ShopPanelTag");
        shopItemListPanel = GameObject.FindWithTag("ShopItemPanelTag");
        shopSelectPanel = GameObject.FindWithTag("ShopSelectPanelTag");
        grayImage = GameObject.FindWithTag("GrayImageTag");
        eManager = GetComponent<EventManager>();
        nowStairs = GameObject.Find("NowStairs").GetComponent<Text>();
        manualButton = GameObject.Find("ManualButton").GetComponent<Button>();
        manualButton.onClick.AddListener(() => openManual());
        if (commandPanel != null)
        {
            commandPanel.SetActive(false);
        }
        if (statusText != null)
        {
            statusText.SetActive(false);
            playerStatusPanel = statusText.transform.Find("PlayerStatusText").GetComponent<Text>();
        }
        if (itemText != null)
        {
            itemText.SetActive(false);
            itemPanel = itemText.transform.Find("ItemPanelText").GetComponent<Text>();
        }
        if (itemUsePanel != null)
        {
            itemUsePanel.SetActive(false);
        }
        if (itemDescriptionPanel != null)
        {
            itemDescriptionPanel.SetActive(false);
        }
        if (npcWindowImage != null)
        {
            npcMessageText = npcWindowImage.transform.Find("TalkText").gameObject.GetComponent<Text>();
            npcNameText = npcWindowImage.transform.Find("NpcNameText").gameObject.GetComponent<Text>();
            npcWindowImage.SetActive(false);
        }
        if (npcImage != null)
        {
            npcImage.SetActive(false);
        }
        if (choisePanel != null)
        {
            choisePanel.SetActive(false);
        }
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        if (shopItemListPanel != null)
        {
            shopItemListPanel.SetActive(false);
            playerMoneyText = shopItemListPanel.transform.Find("PlayerMoneyText").gameObject.GetComponent<Text>();
        }
        if (shopSelectPanel != null)
        {
            shopSelectPanel.SetActive(false);
        }
        if (grayImage != null)
        {
            grayImage.SetActive(false);
            loadingText = grayImage.transform.Find("LoadingText").gameObject.GetComponent<Text>();
        }
        if (levelText != null)
        {
            levelText.SetActive(false);
        }

        boardScript = GetComponent<BoardManager>();
        //マップのランダム生成
        boardScript.SetupScene();
        // 生成された敵オブジェクトを取得
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
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
        if (playerObj.plState != player.playerState.Normal)
        {
            return;
        }
        //コマンドパネル開閉
        if (!isCloseCommand || Input.GetKeyDown("space"))
        {
            playerObj.setPlayerState(player.playerState.Command);
            isCloseCommand = false;
            coroutine = null;
            coroutine = deploymentMyCommandPanel();
            StartCoroutine(coroutine);
        }

        if (playersTurn || enemiesMoving || doingSetup)
        {
            //NPCおよび敵の移動を開始しない。
            return;
        }
        //仲間のNPCが存在する場合
        if (fellows.Count > 0)
        {
            for (int i = 0; i < fellows.Count; i++)
            {
                //前方のキャラの位置を代入(先頭の場合はプレイヤーの位置)
                Vector2 refPosition = i == 0 ? playerObj.playerBeforePosition : fellows[i - 1].npcBeforePosition;
                fellows[i].fellowAction(refPosition);
            }

        }
        //敵を行動させる
        //StartCoroutine(MoveEnemies());
        StartCoroutine(moveEnemies());
    }

    /**
     * コマンドメニュー展開
     */
    IEnumerator deploymentMyCommandPanel()
    {
        isSwitchPlayerStatus = true;
        commandPanel.SetActive(true);
        yield return null;
        //閉じるボタンが押下されるか、スペースキーが押下された場合にメニューを閉じる
        yield return new WaitUntil(() => isCloseCommand || Input.GetKeyDown("space"));
        isCloseCommand = true;
        StopCoroutine(coroutine);
        coroutine = null;
        commandPanel.SetActive(false);
        statusText.SetActive(false);
        itemText.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        itemPanel.enabled = false;
        if (isSwitchPlayerStatus)
        {
            playerObj.setPlayerState(player.playerState.Normal);
        }
    }

    //hpが0になった敵をリストから削除
    public void removeEnemyToList(int index)
    {
        int listIndex = -1;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].enemyNumber == index)
            {
                listIndex = i;
            }
        }
        if (listIndex != -1)
        {
            enemies.RemoveAt(listIndex);
        }
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
        enemyObj.enemyNumber = enemyNumber;
        enemyObj.enemyName = "エネミー" + (enemyNumber + 1);
        //Listにエネミーを追加する
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
     *敵を順番に動かすコルーチン 
     */
    IEnumerator MoveEnemies()
    {
        //プレイヤーは移動できない。
        enemiesMoving = true;
        //移動点を空にする
        enemyNextPosition.Clear();
        //turnDelay秒待機します。デフォルトは.1（100ミリ秒）です。
        yield return new WaitForSeconds(0.6f);

        //スポーンされた敵がいない場合
        if (enemies.Count == 0)
        {
            //移動の間にturnDelay秒待機し、何もないときに移動する敵によって引き起こされる遅延を置き換えます。
            yield return new WaitForSeconds(turnDelay);
        }
        //敵オブジェクトのリストをループ
        for (int i = 0; i < enemies.Count; i++)
        {
            //敵のmoveTimeを待ってから、次の敵を移動
            yield return new WaitForSeconds(turnDelay);

            //敵リストのインデックスiにある敵のmoveEnemy関数を呼び出す。
            enemies[i].moveEnemy();
        }
        //敵の移動が完了したら、playersTurnをtrueに設定して、プレーヤーが移動できるようにする。
        playersTurn = true;

        //敵の移動が完了したら、enemiesMovingをfalseに設定
        enemiesMoving = false;
    }

    /**
     * 敵を順番に動かすコルーチン(現在使用中)
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
        playerObj.isAttack = false;
        //敵の移動が完了したら、enemiesMovingをfalseに設定
        enemiesMoving = false;
        //移動点を空にする
        enemyNextPosition.Clear();
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

    /**
     * 引数で指定したパネルの子オブジェクト(ボタン)をすべて削除する
     */
    public void deleteChildButton(Transform childButtons)
    {
        //選択肢ボタンをすべて削除
        foreach (Transform child in childButtons)
        {
            if (child.name != "ItemPanelText" && child.name != "Scrollbar")
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void wrightAttackLog(string attackerName, string enemyName, int damage)
    {
        string newMessage;
        deleteLog();
        newMessage = attackerName + "が" + enemyName + "に" + damage + "のダメージを与えた";
        logMessage.Add(newMessage);
    }

    public void wrightUseFoodLog(string foodName, string name, int foodPoint)
    {
        string newMessage;
        deleteLog();
        newMessage = foodName + "を使用した";
        newMessage += "\n";
        newMessage += name + "の満腹度が" + foodPoint + "回復した";
        logMessage.Add(newMessage);
    }

    public void wrightDeadLog(string name)
    {
        string newMessage;
        deleteLog();
        newMessage = name + "は倒れた";
        logMessage.Add(newMessage);
    }

    public void wrightLevelupLog(string name)
    {
        string newMessage;
        deleteLog();
        newMessage = name + "のレベルが" + playerLevel + "になった";
        logMessage.Add(newMessage);
    }

    public void wrightInventoryFullLog()
    {
        string newMessage;
        deleteLog();
        newMessage = "荷物がいっぱいです。";
        logMessage.Add(newMessage);
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

    /**
     * コマンドパネルを閉じる
     */
    public void closeMenu()
    {
        isCloseCommand = true;
    }

    /**
     * 操作方法を開く
     */
    public void openManual()
    {
        itemText.SetActive(false);
        //ステータスパネルを共有して使う
        statusText.SetActive(true);
        //マニュアルを表示
        string manual;
        manual = "移動 : 方向キー";
        manual += "\n";
        manual += "メニューを開く : スペースキー";
        manual += "\n";
        manual += "攻撃 : シフトキー";
        manual += "\n";
        manual += "NPCとの会話 : 左クリック";
        manual += "\n";
        playerStatusPanel.text = manual;
    }

    /**
     * ステータスパネルを開く
     */
    public void openStatus()
    {
        itemText.SetActive(false);
        statusText.SetActive(true);
        //プレイヤーのステータスを表示
        string status;
        status = "プレイヤー名 : " + playerObj.statusObj.charName.showName();
        status += "\n";
        status += "レベル : " + playerObj.statusObj.charExperience.showLevel();
        status += "\n";
        status += "HP : " + playerObj.statusObj.charHp.showHp();
        status += "\n";
        status += "攻撃力 : " + playerObj.statusObj.charAttack.showAttack();
        status += "\n";
        status += "防御力 : " + playerObj.statusObj.charDefence.showDefence();
        status += "\n";
        status += "満腹度 : " + playerObj.statusObj.charFood.showFoodPoint();
        status += "\n";
        status += "所持金 : " + playerObj.statusObj.charWallet.showMoney();
        status += "\n";
        status += "魅力度 : " + playerObj.statusObj.charCarm.showCharmPoint();
        status += "\n";
        status += "武器 : " + weaponName;
        status += "\n";
        status += "盾 : " + shieldName;
        playerStatusPanel.text = status;
    }

    /**
     * アイテムボタン押下
     */
    public void openItem()
    {
        deleteChildButton(itemText.transform);
        statusText.SetActive(false);
        itemText.SetActive(true);
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        for (int i = 0; i < itemBtns.Length; i++)
        {
            itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
        }
        Vector3 parentPosition = itemText.transform.position;
        deploymentMyInventory(parentPosition);
    }

    /**
     * インベントリー内のアイテムを展開する
     */
    void deploymentMyInventory(Vector3 parentPosition)
    {
        //アイテムを所持していない
        if (itemList.Count < 1 && itemPanel != null)
        {
            itemPanel.enabled = true;
            itemPanel.text = "アイテムを所持していません";
            return;
        }
        for (var i = 0; i < itemList.Count; i++)
        {
            //メニューボタンの生成
            GameObject listButton = Instantiate(itemBtn, new Vector3(parentPosition.x - 20, parentPosition.y + 60 - i * 35, 0f), Quaternion.identity) as GameObject;
            listButton.transform.SetParent(itemText.transform, false);
            listButton.transform.Find("Text").GetComponent<Text>().text = itemList[i].GetComponent<Item>().name;
            int index = i;
            listButton.GetComponent<ItemNameButton>().itemNameButtonId = index;
            //アイテムをクリックしたときの関数を設定
            listButton.GetComponent<Button>().onClick.AddListener(() => clickItemButton(itemList[index], listButton, index));
        }
    }

    /**
     * itemPanelに展開されたアイテム名をクリック
     */
    public void clickItemButton(GameObject argItem, GameObject listButton, int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        Item item = argItem.GetComponent<Item>();
        for (int i = 0; i < itemBtns.Length; i++)
        {
            if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId == index)
            {
                itemBtns[i].GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
            }
        }
        itemDescriptionPanel.SetActive(true);
        string description = "アイテム名：" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        itemUsePanel.SetActive(true);
        itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = true;
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = true;
        //消費系アイテム
        if (item.type.ToString() == "Consume")
        {
            itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "つかう";
        }
        //装備系アイテム
        else if (item.type.ToString() == "Equipment")
        {
            if (((EquipmentBase)item).isEquip)
            {
                itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "はずす";
                itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = false;
                itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = false;
            }
            else
            {
                itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "そうび";
            }
        }
        itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item, listButton); });
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { addPutItemFunc(argItem); });
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { addThrowItem(argItem); });
    }

    /**
     * アイテムを使用
     */
    public void adduUseItemFunc(Item item, GameObject useBtnObj)
    {
        item.useItem();
        isCloseCommand = true;
        playersTurn = false;
    }

    /**
     * 足元にアイテムを置く
     */
    public void addPutItemFunc(GameObject item)
    {
        playerObj.putItemFloor(item);
        isCloseCommand = true;
    }

    /**
     * アイテムを投げる
     */
    public void addThrowItem(GameObject item)
    {
        playerObj.throwItem(item);
        isSwitchPlayerStatus = false;
        isCloseCommand = true;
    }

    /**
     * 取得したアイテムをリストに追加する
     */
    public bool addItem(GameObject item)
    {
        //アイテムの所持制限を超えている場合
        if (itemList.Count + 1 > nowMaxPosession)
        {
            wrightInventoryFullLog();
            return false;
        }
        //インベントリーが空の状態なら0を割り当てる
        int itemId = itemList.Count == 0 ? 0 : itemList[itemList.Count - 1].GetComponent<Item>().id + 1;
        //IDの割り当て
        item.GetComponent<Item>().id = itemId;
        //イベントフラグのチェック

        itemList.Add(item);
        return true;
    }

    /**
     * レベルアップ
     */
    public void updateLevel()
    {
        nowExprience = mostRecentExperience - (nowMaxExprience - beforeLevelupExperience);
        playerLevel++;
        nowMaxExprience = nowMaxExprience + 10 * playerLevel;
        wrightLevelupLog(playerName);
    }

    /**
     * ステータスの更新
     */
    public void updateStatus()
    {
        playerAttack += 2;
        playerDefence += 2;
        playerHp += riseValueHp == 0 ? 10 : riseValueHp;
        nowPlayerMaxHp += riseValueHp == 0 ? 10 : riseValueHp;
    }

    /**
     * HPの回復処理
     */
    public void recoveryHp(int recoveryValue)
    {
        playerHp += recoveryValue;
        if (playerHp >= nowPlayerMaxHp)
        {
            playerHp = nowPlayerMaxHp;
        }
    }

    /**
     * 満腹度の回復処理
     */
    public void recoveryFoodPoint(int recoveryValue)
    {
        playerFoodPoint += recoveryValue;
        if (playerFoodPoint >= playerMaxFoodPoint)
        {
            playerFoodPoint = playerMaxFoodPoint;
        }
    }

    /**
     * 満腹度を消費する
     */
    public void consumeFoodPoint(int consumeValue)
    {
        playerFoodPoint -= consumeValue;
    }

    /**
     * プレイヤーのHPを減らす
     */
    public void damagePlayerHp(int damagePoint)
    {
        playerHp -= damagePoint;
    }
}
