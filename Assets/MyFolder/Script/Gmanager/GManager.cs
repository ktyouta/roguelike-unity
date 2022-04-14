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

    public float levelStartDelay = 2f;                        //レベルを開始する前に待機する時間（秒単位）。
    public float turnDelay = 0.2f;                            //各プレイヤーのターン間の遅延。
    public static GManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public int enemyDefeatNum = -1;
    [Header("プレイヤーのHP")] public int playerHp;
    [Header("プレイヤーの名前")] public string playerName;
    [Header("プレイヤーの所持金")] public int playerMoney;
    [Header("プレイヤーの攻撃力")] public int playerAttack;
    [Header("プレイヤーの防御力")] public int playerDefence;
    [Header("プレイヤーのレベル")] public int playerLevel = 1;
    [Header("プレイヤーの魅力")] public int playerCharm = 0;
    [HideInInspector] public int nowExprience;
    [Header("次のレベルまでの経験値")] public int nowMaxExprience;
    [HideInInspector] public int mostRecentExperience;
    [HideInInspector] public int beforeLevelupExperience;
    public string weaponName = "なし";
    public string shieldName = "なし";
    [HideInInspector] public List<string> logMessage = new List<string>();
    [Header("プレイヤーの現在の上限HP")] public int nowPlayerMaxHp = 100;
    [HideInInspector] public player playerObj;
    [Header("アイテムの所持数制限")] public int nowMaxPosession;

    //public GameObject canvas;
    public GameObject levelText;
    public GameObject levelImage;
    public GameObject commandPanel;
    public GameObject statusText;
    public GameObject itemText;
    public GameObject itemUsePanel;
    public GameObject itemDescriptionPanel;
    private Button statusButton;
    private Button itemButton;
    private Button closeButton;
    public Text playerStatusPanel;
    public GameObject npcWindowImage;
    public GameObject npcImage;
    public GameObject choisePanel;
    public GameObject shopPanel;
    public GameObject shopItemListPanel;
    public GameObject shopSelectPanel;
    public Text npcMessageText;
    public Text npcNameText;
    public Text playerMoneyText;
    [HideInInspector]public int level;
    [Header("インベントリーに展開されるアイテムボタン")] public GameObject itemBtn;


    private List<Enemy> enemies;                            //移動コマンドを発行するために使用されるすべての敵ユニットのリスト。
    private bool enemiesMoving;                                //enemyのターンかチェック
    private bool doingSetup;
    private bool loadFlg = false;
    [HideInInspector] public bool isMenuOpen = false;
    [HideInInspector] public bool isCloseCommand = true;
    private bool spaceKey = false;
    private GameObject itemObj;
    public List<GameObject> itemList = new List<GameObject>();
    private IEnumerator coroutine;
    [Header("インベントリーの子オブジェクト(テキスト)")]public Text itemPanel;

    //宝箱用のアイテムリスト
    public List<Item> treasureItemList = new List<Item>();

    TreasureItem treasureLotteryList = new TreasureItem(new List<Item>(), new List<Item>());

    //抽選用アイテムのリスト(TreasureクラスのlotteryIdによりインデックスを切り替える)
    public List<List<GameObject>> lotteryitemList = new List<List<GameObject>>();

    //Start is called before the first frame update
    void Awake()
    {
        //インスタンスの確認
        if (instance == null)
        {
            //ない場合、インスタンスに設定する
            instance = this;
        }
        //インスタンスが存在するが、これではない場合
        else if (instance != this)
        {
            //破壊します。 これにより、シングルトンパターンが適用されます。
            //つまり、GameManagerのインスタンスは1つしか存在できません。
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
        instance.level++;
        instance.InitGame();
    }

    //各レベルのゲームを初期化します。
    public void InitGame()
    {
        DontDestroyOnLoad(gameObject);
        levelText = GameObject.Find("LevelText");
        enemies = new List<Enemy>();
        //itemList = new List<GameObject>();
        boardScript = GetComponent<BoardManager>();
        commandPanel = GameObject.Find("CommandPanel");
        statusText = GameObject.Find("StatusPanel");
        itemText = GameObject.Find("ItemPanel");
        statusButton = GameObject.Find("StatusButton").GetComponent<Button>();
        statusButton.onClick.AddListener(()=> openStatus());
        itemButton = GameObject.Find("ItemButton").GetComponent<Button>();
        itemButton.onClick.AddListener(() => openItem());
        //itemButton.onClick.AddListener(() => closeMenu());
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
        //GManager.instance.level++;
        //Debug.Log("level" + level);
        //Debug.Log("food" + playerFoodPoints);
        //doingSetupがtrueの場合、プレーヤーは移動できません。タイトルカードがアップしている間はプレーヤーが移動しないようにします。
        doingSetup = true;

        //名前で検索して、画像Levelimageへの参照を取得します。
        //levelImage = GameObject.Find("Levelimage");

        //名前で検索してGetComponentを呼び出すことにより、テキストLevelTextのテキストコンポーネントへの参照を取得します。
        //levelText = GameObject.Find("LevelText");
        //Debug.Log(levelText);
        if (levelText != null)
        {
           levelText.SetActive(false);
        }
        

        //levelTextのテキストを文字列「Day」に設定し、現在のレベル番号を追加します。
        //levelText.text = "Day " + level;

        //セットアップ中にlevelImageをゲームボードのアクティブなブロッキングプレーヤーのビューに設定します。
        //levelImage.SetActive(true);

        //levelStartDelayを秒単位で遅延させてHideLevelImage関数を呼び出します。
        Invoke("HideLevelImage", levelStartDelay);

        //リスト内の敵オブジェクトをすべてクリアして、次のレベルに備えます。
        enemies.Clear();

        //Debug.Log("num"+GManager.instance.level);
        //BoardManagerスクリプトのSetupScene関数を呼び出し、現在のレベル番号を渡します。
        boardScript.SetupScene(GManager.instance.level);

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(enemyObjects);
        //Debug.Log("enemyObject" + enemyObjects.Length);
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        //作成したエネミーにidを割り振り、リストに格納する
        for (var i = 0; i < enemyObjects.Length; i++)
        {
            Enemy enemyObj = enemyObjects[i].GetComponent<Enemy>();
            enemyObj.enemyNumber = i;
            //Debug.Log(enemyObj.enemyNumber);
            //Debug.Log("add" + i);
            AddEnemyToList(enemyObj);
        }
    }

    //レベル間で使用される黒い画像を非表示にします
    public void HideLevelImage()
    {
        //levelImage gameObjectを無効にします。
        //levelImage.SetActive(false);

        //プレーヤーが再び移動できるように、doingSetupをfalseに設定します
        doingSetup = false;
    }

    //更新はフレームごとに呼び出されます。
    void Update()
    {
        if (!isCloseCommand)
        {
            return;
        }
        //spaceKey = Input.GetKeyDown("space");
        //コマンドパネル開閉
        if (Input.GetKeyDown("space"))
        {
            //isMenuOpen = !isMenuOpen;
            //commandPanel.SetActive(isMenuOpen);
            //statusText.SetActive(false);
            //itemText.SetActive(false);
            //itemUsePanel.SetActive(false);
            //itemDescriptionPanel.SetActive(false);

            //TODO:コルーチンを用いる
            playerObj.setPlayerState(player.playerState.Command);
            isCloseCommand = false;
            coroutine = deploymentMyCommandPanel();
            StartCoroutine(coroutine);
        }

        
       
        //コルーチンを用いたメニュー処理の実装により削除される予定
        //if (isMenuOpen)
        //{
        //    return;
        //}
        //playersTurnまたはenemiesMovingまたはdoingSetupが現在trueでないことを確認してください。
        if (playersTurn || enemiesMoving || doingSetup)
        {
            //これらのいずれかがtrueの場合は戻り、MoveEnemiesを開始しないでください。
            return;
        }
        else
        {
            //Debug.Log("enemydefnum" + enemyDefeatNum);
            if (enemyDefeatNum != -1)
            {
                removeEnemyToList(enemyDefeatNum);
                enemyDefeatNum = -1;
            }
            //Debug.Log("enemyTurn");
            //エネミーの行動関数実行
            StartCoroutine(MoveEnemies());
        }
        
    }

    /**
     * コマンドメニュー展開(実装中)
     */
    IEnumerator deploymentMyCommandPanel()
    {
        isMenuOpen = false;
        commandPanel.SetActive(true);
        yield return null;
        //閉じるボタンが押下されるか、スペースキーが押下された場合にメニューを閉じる
        yield return new WaitUntil(() => isCloseCommand || Input.GetKeyDown("space"));
        isMenuOpen = true;
        isCloseCommand = true;
        StopCoroutine(coroutine);
        coroutine = null;
        commandPanel.SetActive(false);
        statusText.SetActive(false);
        itemText.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        itemPanel.enabled = false;
        playerObj.setPlayerState(player.playerState.Normal);
    }

    //hpが0になった敵をリストから削除
    public void removeEnemyToList(int index)
    {
        int listIndex = -1;
        for (int i=0;i<enemies.Count;i++)
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

    //これを呼び出して、渡された敵を敵オブジェクトのリストに追加します。
    public void AddEnemyToList(Enemy script)
    {
        //Listにエネミーを追加する
        enemies.Add(script);
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

    //敵を順番に動かすコルーチン。
    IEnumerator MoveEnemies()
    {
        //enemiesMovingはtrueですが、プレイヤーは移動できません。
        enemiesMoving = true;

        //turnDelay秒待機します。デフォルトは.1（100ミリ秒）です。
        yield return new WaitForSeconds(0.6f);

        //スポーンされた敵がいない場合（第1レベルのIE）：
        if (enemies.Count == 0)
        {
            //移動の間にturnDelay秒待機し、何もないときに移動する敵によって引き起こされる遅延を置き換えます。
            yield return new WaitForSeconds(turnDelay);
        }
        //Debug.Log("enemies.Count" + enemies.Count);
        //敵オブジェクトのリストをループします。
        //Debug.Log("enemycount2:" + enemies.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            
            //敵のmoveTimeを待ってから、次の敵を移動します。
            yield return new WaitForSeconds(turnDelay);

            //敵リストのインデックスiにある敵のMoveEnemy関数を呼び出します。
            enemies[i].MoveEnemy();
        }
        //敵の移動が完了したら、playersTurnをtrueに設定して、プレーヤーが移動できるようにします。
        playersTurn = true;

        //敵の移動が完了したら、enemiesMovingをfalseに設定します。
        enemiesMoving = false;
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

    public void wrightAttackLog(string attackerName,string enemyName,int damage)
    {
        string newMessage;
        newMessage = attackerName + "が" + enemyName + "に" + damage + "のダメージを与えた";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightUseFoodLog(string foodName, string name,int foodPoint)
    {
        string newMessage;
        newMessage = foodName + "を使用した";
        newMessage += "\n";
        newMessage += name + "の満腹度が" + foodPoint + "回復した";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightDeadLog(string name)
    {
        string newMessage;
        newMessage = name + "は倒れた";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightLevelupLog(string name)
    {
        string newMessage;
        newMessage = name + "のレベルが" + GManager.instance.playerLevel + "になった";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightInventoryFullLog()
    {
        string newMessage;
        newMessage = "荷物がいっぱいです。";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightLog(string message)
    {
        GManager.instance.logMessage.Add(message);
    }

    /**
     * コマンドパネルを閉じる
     */
    public void closeMenu()
    {
        //commandPanel.SetActive(false);
        //statusText.SetActive(false);
        //itemText.SetActive(false);
        //itemUsePanel.SetActive(false);
        //itemDescriptionPanel.SetActive(false);
        //isMenuOpen = false;
        isMenuOpen = true;
        isCloseCommand = true;
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
        status = "プレイヤー名 : " + GManager.instance.playerName;
        status += "\n";
        status += "レベル : " + GManager.instance.playerLevel;
        status += "\n";
        status += "HP : " + GManager.instance.playerHp;
        status += "\n";
        status += "攻撃力 : " + GManager.instance.playerAttack;
        status += "\n";
        status += "防御力 : " + GManager.instance.playerDefence;
        status += "\n";
        status += "満腹度 : " + GManager.instance.playerFoodPoints;
        status += "\n";
        status += "所持金 : " + GManager.instance.playerMoney;
        status += "\n";
        status += "武器 : " + GManager.instance.weaponName;
        status += "\n";
        status += "盾 : " + GManager.instance.shieldName;
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
            GameObject listButton = Instantiate(itemBtn, new Vector3(parentPosition.x -10, parentPosition.y + 80 - i * 35, 0f), Quaternion.identity) as GameObject;
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
        GManager.instance.itemUsePanel.SetActive(true);
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = true;
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = true;
        //消費系アイテム
        if (item.type == "foodRecovery" || item.type == "portion")
        {
            GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "つかう";
        }
        //装備系アイテム
        else if (item.type == "Equipment")
        {
            if (((EquipmentBase)item).isEquip)
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "はずす";
                GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = false;
                GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = false;
            }
            else
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "そうび";
            }
        }
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item, listButton); });
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { addPutItemFunc(argItem); });
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("投げる"); GManager.instance.itemUsePanel.SetActive(false); });
    }

    /**
     * アイテムを使用
     */
    public void adduUseItemFunc(Item item, GameObject useBtnObj)
    {
        item.useItem();
        isCloseCommand = true;
    }

    //足元にアイテムを置く
    public void addPutItemFunc(GameObject item)
    {
        playerObj.putItemFloor(item);
        isCloseCommand = true;
    }

    /**
     * 取得したアイテムをリストに追加する
     */
    public void addItem(GameObject item)
    {
        GManager.instance.itemList.Add(item);
    }

    /**
     * レベルアップ
     */
    public void updateLevel()
    {
        GManager.instance.nowExprience = GManager.instance.mostRecentExperience - (GManager.instance.nowMaxExprience - GManager.instance.beforeLevelupExperience);
        GManager.instance.playerLevel++;
        GManager.instance.nowMaxExprience = GManager.instance.nowMaxExprience + 10 * GManager.instance.playerLevel;
        GManager.instance.wrightLevelupLog(GManager.instance.playerName);
    }

    /**
     * ステータスの更新
     */
    public void updateStatus()
    {
        GManager.instance.playerAttack += 2;
        GManager.instance.playerDefence += 2;
    }

    /**
     * HPの回復処理
     */
    public void recoveryHp(int recoveryValue)
    {
        GManager.instance.playerHp += recoveryValue;
        if (GManager.instance.playerHp >= nowPlayerMaxHp)
        {
            GManager.instance.playerHp = nowPlayerMaxHp;
        }
    }

    /**
     * 満腹度を消費する
     */
    public void consumeFoodPoint(int consumeValue)
    {
        GManager.instance.playerFoodPoints -= consumeValue;
    }

    /**
     * プレイヤーのHPを減らす
     */
    public void damagePlayerHp(int damagePoint)
    {
        GManager.instance.playerHp -= damagePoint;
    }
}
