using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    //�󔠂̒��I�ɗp����A�C�e�����X�g�̃N���X
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

    //�v���C���[�̃X�e�[�^�X�n
    [Header("�v���C���[��HP")] public int playerHp;
    [Header("�v���C���[�̌��݂̏��HP")] public int nowPlayerMaxHp = 100;
    [Header("�v���C���[�̖��O")] public string playerName;
    [Header("�v���C���[�̏�����")] public int playerMoney;
    [Header("�v���C���[�̍U����")] public int playerAttack;
    [Header("�v���C���[�̖h���")] public int playerDefence;
    [Header("�v���C���[�̃��x��")] public int playerLevel = 1;
    [Header("�v���C���[�̖���")] public int playerCharm = 0;
    [Header("���̃��x���܂ł̌o���l")] public int nowMaxExprience;
    [Header("�A�C�e���̏���������")] public int nowMaxPosession;
    [Header("�v���C���[�̖����x")] public int playerFoodPoints;
    [HideInInspector] public int mostRecentExperience;
    [HideInInspector] public int beforeLevelupExperience;
    [HideInInspector] public player playerObj;
    [HideInInspector] public int nowExprience;

    //�p�l������n
    [Header("�C���x���g���[�ɓW�J�����A�C�e���{�^��")] public GameObject itemBtn;
    [Header("�C���x���g���[�̎q�I�u�W�F�N�g(�e�L�X�g)")] public Text itemPanel;
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
    public Text playerStatusPanel;
    public Text npcMessageText;
    public Text npcNameText;
    public Text playerMoneyText;
    private Button statusButton;
    private Button itemButton;
    private Button closeButton;

    //���X�g
    //�ړ��R�}���h�𔭍s���邽�߂Ɏg�p����邷�ׂĂ̓G���j�b�g�̃��X�g�B
    public List<Enemy> enemies = new List<Enemy>();                            
    //�󔠗p�̃A�C�e�����X�g
    public List<Item> treasureItemList = new List<Item>();
    //���I�p�A�C�e���̃��X�g(Treasure�N���X��lotteryId�ɂ��C���f�b�N�X��؂�ւ���)
    public List<List<GameObject>> lotteryitemList = new List<List<GameObject>>();
    //�C���x���g���[���̃��X�g
    public List<GameObject> itemList = new List<GameObject>();
    //���O
    [HideInInspector] public List<string> logMessage = new List<string>();

    //���̑�
    //enemy�̃^�[�����`�F�b�N
    [HideInInspector] public bool isCloseCommand = true;
    [HideInInspector] public int level;
    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public int enemyDefeatNum = -1;
    //���x�����J�n����O�ɑҋ@���鎞�ԁi�b�P�ʁj�B
    public float levelStartDelay = 2f;
    //�e�v���C���[�̃^�[���Ԃ̒x���B
    public float turnDelay = 0.05f;
    public static GManager instance = null;
    public BoardManager boardScript;
    public string weaponName = "�Ȃ�";
    public string shieldName = "�Ȃ�";
    private bool enemiesMoving;
    private bool doingSetup;
    private bool loadFlg = false;
    private IEnumerator coroutine;
    private bool isSwitchPlayerStatus;

    //Start is called before the first frame update
    void Awake()
    {
        //�C���X�^���X�̊m�F
        if (instance == null)
        {
            //�Ȃ��ꍇ�A�C���X�^���X�ɐݒ肷��
            instance = this;
        }
        //�C���X�^���X�����݂��邪�A����ł͂Ȃ��ꍇ
        else if (instance != this)
        {
            //�j�󂵂܂��B ����ɂ��A�V���O���g���p�^�[�����K�p����܂��B
            //�܂�AGameManager�̃C���X�^���X��1�������݂ł��܂���B
            Destroy(gameObject);
        }

        if (!instance.loadFlg)
        {
            //�V�[���������[�h����Ƃ��ɂ��ꂪ�j������Ȃ��悤�ɐݒ肵�܂�
            
            InitGame();
            instance.loadFlg = true;
        }
        
    }
    //�����1�񂾂��Ăяo����A�p�����[�^�̓V�[�������[�h���ꂽ��ɂ̂݌Ăяo�����悤�Ɏw�����܂�//�i�����łȂ��ꍇ�AScene Load�R�[���o�b�N�͍ŏ��̃��[�h�ƌĂ΂�A�K�v����܂���j
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //�V�[�����ǂݍ��܂�邽�тɌĂяo�����R�[���o�b�N��o�^���܂�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }

    //�e���x���̃Q�[�������������܂��B
    public void InitGame()
    {
        DontDestroyOnLoad(gameObject);
        levelText = GameObject.Find("LevelText");
        boardScript = GetComponent<BoardManager>();
        commandPanel = GameObject.Find("CommandPanel");
        statusText = GameObject.Find("StatusPanel");
        itemText = GameObject.Find("ItemPanel");
        statusButton = GameObject.Find("StatusButton").GetComponent<Button>();
        statusButton.onClick.AddListener(()=> openStatus());
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
        //doingSetup��true�̏ꍇ�A�v���[���[�͈ړ��ł��܂���B�^�C�g���J�[�h���A�b�v���Ă���Ԃ̓v���[���[���ړ����Ȃ��悤�ɂ��܂��B
        doingSetup = true;

        //���O�Ō������āA�摜Levelimage�ւ̎Q�Ƃ��擾���܂��B
        //levelImage = GameObject.Find("Levelimage");

        //���O�Ō�������GetComponent���Ăяo�����Ƃɂ��A�e�L�X�gLevelText�̃e�L�X�g�R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
        //levelText = GameObject.Find("LevelText");
        //Debug.Log(levelText);
        if (levelText != null)
        {
           levelText.SetActive(false);
        }
        

        //levelText�̃e�L�X�g�𕶎���uDay�v�ɐݒ肵�A���݂̃��x���ԍ���ǉ����܂��B
        //levelText.text = "Day " + level;

        //�Z�b�g�A�b�v����levelImage���Q�[���{�[�h�̃A�N�e�B�u�ȃu���b�L���O�v���[���[�̃r���[�ɐݒ肵�܂��B
        //levelImage.SetActive(true);

        //levelStartDelay��b�P�ʂŒx��������HideLevelImage�֐����Ăяo���܂��B
        Invoke("HideLevelImage", levelStartDelay);

        //���X�g���̓G�I�u�W�F�N�g�����ׂăN���A���āA���̃��x���ɔ����܂��B
        enemies.Clear();

        //Debug.Log("num"+GManager.instance.level);
        //BoardManager�X�N���v�g��SetupScene�֐����Ăяo���A���݂̃��x���ԍ���n���܂��B
        boardScript.SetupScene(GManager.instance.level);

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(enemyObjects);
        //Debug.Log("enemyObject" + enemyObjects.Length);
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        //�쐬�����G�l�~�[��id������U��A���X�g�Ɋi�[����
        for (var i = 0; i < enemyObjects.Length; i++)
        {
            Enemy enemyObj = enemyObjects[i].GetComponent<Enemy>();
            enemyObj.enemyNumber = i;
            //Debug.Log(enemyObj.enemyNumber);
            //Debug.Log("add" + i);
            AddEnemyToList(enemyObj);
        }
    }

    //���x���ԂŎg�p����鍕���摜���\���ɂ��܂�
    public void HideLevelImage()
    {
        //levelImage gameObject�𖳌��ɂ��܂��B
        //levelImage.SetActive(false);

        //�v���[���[���Ăшړ��ł���悤�ɁAdoingSetup��false�ɐݒ肵�܂�
        doingSetup = false;
    }

    //�X�V�̓t���[�����ƂɌĂяo����܂��B
    void Update()
    {
        if (!isCloseCommand || playerObj.plState != player.playerState.Normal)
        {
            return;
        }
        //�R�}���h�p�l���J��
        if (Input.GetKeyDown("space"))
        {
            playerObj.setPlayerState(player.playerState.Command);
            isCloseCommand = false;
            coroutine = deploymentMyCommandPanel();
            StartCoroutine(coroutine);
        }
     
        //playersTurn�܂���enemiesMoving�܂���doingSetup������true�łȂ����Ƃ��m�F���Ă��������B
        if (playersTurn || enemiesMoving || doingSetup)
        {
            //�����̂����ꂩ��true�̏ꍇ�͖߂�AMoveEnemies���J�n���Ȃ��ł��������B
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
            //�G�l�~�[�̍s���֐����s
            StartCoroutine(MoveEnemies());
        }
        
    }

    /**
     * �R�}���h���j���[�W�J
     */
    IEnumerator deploymentMyCommandPanel()
    {
        isSwitchPlayerStatus = true;
        commandPanel.SetActive(true);
        yield return null;
        //����{�^������������邩�A�X�y�[�X�L�[���������ꂽ�ꍇ�Ƀ��j���[�����
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

    //hp��0�ɂȂ����G�����X�g����폜
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

    //������Ăяo���āA�n���ꂽ�G��G�I�u�W�F�N�g�̃��X�g�ɒǉ����܂��B
    public void AddEnemyToList(Enemy script)
    {
        //List�ɃG�l�~�[��ǉ�����
        enemies.Add(script);
    }


    //�v���C���[���t�[�h�|�C���g0�ɓ��B����ƁAGameOver���Ăяo����܂�
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        //levelText.text = "After " + level + " days, you starved.";
        levelText.SetActive(true);

        //Enable black background image gameObject.
        //levelImage.SetActive(true);

        //boardScript.destroyBoard();
        //����GameManager�𖳌��ɂ��܂��B
        enabled = false;
    }

    //�G�����Ԃɓ������R���[�`���B
    IEnumerator MoveEnemies()
    {
        //enemiesMoving��true�ł����A�v���C���[�͈ړ��ł��܂���B
        enemiesMoving = true;

        //turnDelay�b�ҋ@���܂��B�f�t�H���g��.1�i100�~���b�j�ł��B
        yield return new WaitForSeconds(0.6f);

        //�X�|�[�����ꂽ�G�����Ȃ��ꍇ�i��1���x����IE�j�F
        if (enemies.Count == 0)
        {
            //�ړ��̊Ԃ�turnDelay�b�ҋ@���A�����Ȃ��Ƃ��Ɉړ�����G�ɂ���Ĉ����N�������x����u�������܂��B
            yield return new WaitForSeconds(turnDelay);
        }
        //Debug.Log("enemies.Count" + enemies.Count);
        //�G�I�u�W�F�N�g�̃��X�g�����[�v���܂��B
        //Debug.Log("enemycount2:" + enemies.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            //�G��moveTime��҂��Ă���A���̓G���ړ����܂��B
            yield return new WaitForSeconds(turnDelay);

            //�G���X�g�̃C���f�b�N�Xi�ɂ���G��MoveEnemy�֐����Ăяo���܂��B
            enemies[i].MoveEnemy();
        }
        //�G�̈ړ�������������AplayersTurn��true�ɐݒ肵�āA�v���[���[���ړ��ł���悤�ɂ��܂��B
        playersTurn = true;

        //�G�̈ړ�������������AenemiesMoving��false�ɐݒ肵�܂��B
        enemiesMoving = false;
    }

    /**
     * �����Ŏw�肵���p�l���̎q�I�u�W�F�N�g(�{�^��)�����ׂč폜����
     */
    public void deleteChildButton(Transform childButtons)
    {
        //�I�����{�^�������ׂč폜
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
        newMessage = attackerName + "��" + enemyName + "��" + damage + "�̃_���[�W��^����";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightUseFoodLog(string foodName, string name,int foodPoint)
    {
        string newMessage;
        newMessage = foodName + "���g�p����";
        newMessage += "\n";
        newMessage += name + "�̖����x��" + foodPoint + "�񕜂���";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightDeadLog(string name)
    {
        string newMessage;
        newMessage = name + "�͓|�ꂽ";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightLevelupLog(string name)
    {
        string newMessage;
        newMessage = name + "�̃��x����" + GManager.instance.playerLevel + "�ɂȂ���";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightInventoryFullLog()
    {
        string newMessage;
        newMessage = "�ו��������ς��ł��B";
        GManager.instance.logMessage.Add(newMessage);
    }

    public void wrightLog(string message)
    {
        GManager.instance.logMessage.Add(message);
    }

    /**
     * �R�}���h�p�l�������
     */
    public void closeMenu()
    {
        isCloseCommand = true;
    }

    /**
     * ������@���J��
     */
    public void openManual()
    {
        itemText.SetActive(false);
        //�X�e�[�^�X�p�l�������L���Ďg��
        statusText.SetActive(true);
        //�}�j���A����\��
        string manual;
        manual = "�ړ� : �����L�[";
        manual += "\n";
        manual += "���j���[���J�� : �X�y�[�X�L�[";
        manual += "\n";
        manual += "�U�� : �V�t�g�L�[";
        manual += "\n";
        manual += "NPC�Ƃ̉�b : z�{�^��";
        manual += "\n";
        playerStatusPanel.text = manual;
    }

    /**
     * �X�e�[�^�X�p�l�����J��
     */
    public void openStatus()
    {
        itemText.SetActive(false);
        statusText.SetActive(true);
        //�v���C���[�̃X�e�[�^�X��\��
        string status;
        status = "�v���C���[�� : " + GManager.instance.playerName;
        status += "\n";
        status += "���x�� : " + GManager.instance.playerLevel;
        status += "\n";
        status += "HP : " + GManager.instance.playerHp;
        status += "\n";
        status += "�U���� : " + GManager.instance.playerAttack;
        status += "\n";
        status += "�h��� : " + GManager.instance.playerDefence;
        status += "\n";
        status += "�����x : " + GManager.instance.playerFoodPoints;
        status += "\n";
        status += "������ : " + GManager.instance.playerMoney;
        status += "\n";
        status += "���� : " + GManager.instance.weaponName;
        status += "\n";
        status += "�� : " + GManager.instance.shieldName;
        playerStatusPanel.text = status;
    }

    /**
     * �A�C�e���{�^������
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
     * �C���x���g���[���̃A�C�e����W�J����
     */
    void deploymentMyInventory(Vector3 parentPosition)
    {
        //�A�C�e�����������Ă��Ȃ�
        if (itemList.Count < 1 && itemPanel != null)
        {
            itemPanel.enabled = true;
            itemPanel.text = "�A�C�e�����������Ă��܂���";
            return;
        }
        for (var i = 0; i < itemList.Count; i++)
        {
            //���j���[�{�^���̐���
            GameObject listButton = Instantiate(itemBtn, new Vector3(parentPosition.x -10, parentPosition.y + 80 - i * 35, 0f), Quaternion.identity) as GameObject;
            listButton.transform.SetParent(itemText.transform, false);
            listButton.transform.Find("Text").GetComponent<Text>().text = itemList[i].GetComponent<Item>().name;
            int index = i;
            listButton.GetComponent<ItemNameButton>().itemNameButtonId = index;
            //�A�C�e�����N���b�N�����Ƃ��̊֐���ݒ�
            listButton.GetComponent<Button>().onClick.AddListener(() => clickItemButton(itemList[index], listButton, index));
        }
    }

    /**
     * itemPanel�ɓW�J���ꂽ�A�C�e�������N���b�N
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
        string description = "�A�C�e�����F" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        GManager.instance.itemUsePanel.SetActive(true);
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = true;
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = true;
        //����n�A�C�e��
        if (item.type.ToString() == "Consume")
        {
            GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "����";
        }
        //�����n�A�C�e��
        else if (item.type.ToString() == "Equipment")
        {
            if (((EquipmentBase)item).isEquip)
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "�͂���";
                GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = false;
                GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = false;
            }
            else
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "������";
            }
        }
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item, listButton); });
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { addPutItemFunc(argItem); });
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { addThrowItem(argItem); });
    }

    /**
     * �A�C�e�����g�p
     */
    public void adduUseItemFunc(Item item, GameObject useBtnObj)
    {
        item.useItem();
        isCloseCommand = true;
        GManager.instance.playersTurn = false;
    }

    /**
     * �����ɃA�C�e����u��
     */
    public void addPutItemFunc(GameObject item)
    {
        playerObj.putItemFloor(item);
        isCloseCommand = true;
    }

    /**
     * �A�C�e���𓊂���
     */
    public void addThrowItem(GameObject item)
    {
        playerObj.throwItem(item);
        isSwitchPlayerStatus = false;
        isCloseCommand = true;
    }

    /**
     * �擾�����A�C�e�������X�g�ɒǉ�����
     */
    public bool addItem(GameObject item)
    {
        bool isAbleAdd = false;
        //�A�C�e���̏��������𒴂��Ă���ꍇ
        if (GManager.instance.itemList.Count + 1 > GManager.instance.nowMaxPosession)
        {
            GManager.instance.wrightInventoryFullLog();
        }
        else
        {
            //�C���x���g���[����̏�ԂȂ�0�����蓖�Ă�
            int itemId = GManager.instance.itemList.Count == 0 ? 0 : GManager.instance.itemList[GManager.instance.itemList.Count - 1].GetComponent<Item>().id + 1;
            //ID�̊��蓖��
            item.GetComponent<Item>().id = itemId;
            GManager.instance.itemList.Add(item);
            isAbleAdd = true;
        }
        return isAbleAdd;
    }

    /**
     * ���x���A�b�v
     */
    public void updateLevel()
    {
        GManager.instance.nowExprience = GManager.instance.mostRecentExperience - (GManager.instance.nowMaxExprience - GManager.instance.beforeLevelupExperience);
        GManager.instance.playerLevel++;
        GManager.instance.nowMaxExprience = GManager.instance.nowMaxExprience + 10 * GManager.instance.playerLevel;
        GManager.instance.wrightLevelupLog(GManager.instance.playerName);
    }

    /**
     * �X�e�[�^�X�̍X�V
     */
    public void updateStatus()
    {
        GManager.instance.playerAttack += 2;
        GManager.instance.playerDefence += 2;
    }

    /**
     * HP�̉񕜏���
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
     * �����x�������
     */
    public void consumeFoodPoint(int consumeValue)
    {
        GManager.instance.playerFoodPoints -= consumeValue;
    }

    /**
     * �v���C���[��HP�����炷
     */
    public void damagePlayerHp(int damagePoint)
    {
        GManager.instance.playerHp -= damagePoint;
    }
}
