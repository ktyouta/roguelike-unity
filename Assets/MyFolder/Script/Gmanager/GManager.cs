using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public float levelStartDelay = 2f;                        //���x�����J�n����O�ɑҋ@���鎞�ԁi�b�P�ʁj�B
    public float turnDelay = 0.2f;                            //�e�v���C���[�̃^�[���Ԃ̒x���B
    public static GManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public int enemyDefeatNum = -1;
    [Header("�v���C���[��HP")] public int playerHp;
    [Header("�v���C���[�̖��O")] public string playerName;
    [Header("�v���C���[�̏�����")] public int playerMoney;
    [Header("�v���C���[�̍U����")] public int playerAttack;
    [Header("�v���C���[�̖h���")] public int playerDefence;
    [Header("�v���C���[�̃��x��")] public int playerLevel = 1;
    [HideInInspector] public int nowExprience;
    [Header("���̃��x���܂ł̌o���l")] public int nowMaxExprience;
    [HideInInspector] public int mostRecentExperience;
    [HideInInspector] public int beforeLevelupExperience;
    public string weaponName = "�Ȃ�";
    public string shieldName = "�Ȃ�";
    [HideInInspector] public List<string> logMessage = new List<string>();
    [Header("�v���C���[�̌��݂̏��HP")] public int nowPlayerMaxHp = 100;

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
    [HideInInspector]public int level;

    private List<Enemy> enemies;                            //�ړ��R�}���h�𔭍s���邽�߂Ɏg�p����邷�ׂĂ̓G���j�b�g�̃��X�g�B
    private bool enemiesMoving;                                //enemy�̃^�[�����`�F�b�N
    private bool doingSetup;
    private bool loadFlg = false;
    [HideInInspector] public bool isMenuOpen = false;
    private bool spaceKey = false;
    private GameObject itemObj;
    public List<Item> itemList = new List<Item>();

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
        if (commandPanel != null)
        {
            commandPanel.SetActive(false);
        }
        if (statusText != null)
        {
            statusText.SetActive(false);
        }
        if (itemText != null)
        {
            itemText.SetActive(false);
        }
        if (itemUsePanel != null)
        {
            itemUsePanel.SetActive(false);
        }
        if (itemDescriptionPanel != null)
        {
            itemDescriptionPanel.SetActive(false);
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
        spaceKey = Input.GetKeyDown("space");
        //�R�}���h�p�l���J��
        if (spaceKey)
        {
            isMenuOpen = !isMenuOpen;
            commandPanel.SetActive(isMenuOpen);
            statusText.SetActive(false);
            itemText.SetActive(false);
            itemUsePanel.SetActive(false);
            itemDescriptionPanel.SetActive(false);
        }
       
        if (isMenuOpen)
        {
            return;
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

    //hp��0�ɂȂ����G�����X�g����폜
    public void removeEnemyToList(int index)
    {
        //Debug.Log("num"+index);
        //Debug.Log(enemies);
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

    public List<string> wrightAttackLog(string attackerName,string enemyName,int damage)
    {
        string newMessage;
        newMessage = attackerName + "��" + enemyName + "��" + damage + "�̃_���[�W��^����";
        GManager.instance.logMessage.Insert(0, newMessage);
        return GManager.instance.logMessage;
    }

    public List<string> wrightUseFoodLog(string foodName, string name,int foodPoint)
    {
        string newMessage;
        newMessage = foodName + "���g�p����";
        newMessage += "\n";
        newMessage += name + "�̖����x��" + foodPoint + "�񕜂���";
        GManager.instance.logMessage.Insert(0, newMessage);
        return GManager.instance.logMessage;
    }

    public List<string> wrightDeadLog(string name)
    {
        string newMessage;
        newMessage = name + "�͓|�ꂽ";
        GManager.instance.logMessage.Insert(0, newMessage);
        return GManager.instance.logMessage;
    }

    public List<string> wrightLevelupLog(string name)
    {
        string newMessage;
        newMessage = name + "�̃��x����" + GManager.instance.playerLevel + "�ɂȂ���";
        GManager.instance.logMessage.Insert(0, newMessage);
        return GManager.instance.logMessage;
    }

    public void closeMenu()
    {
        commandPanel.SetActive(false);
        statusText.SetActive(false);
        itemText.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        isMenuOpen = false;
    }

    public void openStatus()
    {
        itemText.SetActive(false);
        statusText.SetActive(true);
    }

    public void openItem()
    {
        statusText.SetActive(false);
        itemText.SetActive(true);
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        for (int i = 0; i < itemBtns.Length; i++)
        {
            itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
        }
    }

    /**
     * �擾�����A�C�e�������X�g�ɒǉ�����
     */
    public void addItem(Item item)
    {
        GManager.instance.itemList.Add(item);
    }

    /**
     * �A�C�e���������
     */
    public void consumeItem(int index)
    {
        Debug.Log("beforcount" + GManager.instance.itemList.Count);
        Debug.Log("remove" + index);
        GManager.instance.itemList.RemoveAt(index);
        Debug.Log("aftercount" + GManager.instance.itemList.Count);
    }

    /**
     * ���x���A�b�v
     */
    public void updateLevel()
    {
        //Debug.Log("before" + GManager.instance.beforeLevelupExperience);
        GManager.instance.nowExprience = GManager.instance.mostRecentExperience - (GManager.instance.nowMaxExprience - GManager.instance.beforeLevelupExperience);
        //Debug.Log("mostrecent" + GManager.instance.mostRecentExperience);
        //Debug.Log("max" + GManager.instance.nowMaxExprience);
        //Debug.Log("now" + GManager.instance.nowExprience);
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
