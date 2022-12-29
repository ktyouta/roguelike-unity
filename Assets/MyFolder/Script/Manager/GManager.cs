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
    [HideInInspector] public player playerObj;
    [HideInInspector] public StatusComponentPlayer statusComponentPlayer;

    //�p�l������n
    public GameObject levelText;
    public GameObject grayImage;
    public GameObject mapLoadingImage;
    public Text loadingText;
    public Text mapLoadingText;
    public Text nowStairs;

    //���X�g
    //�G���j�b�g�̃��X�g�B
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();
    //NPC�p�̃��X�g
    [HideInInspector] public List<NpcFellow> fellows = new List<NpcFellow>();
    //�󔠗p�̃A�C�e�����X�g
    [HideInInspector] public List<Item> treasureItemList = new List<Item>();
    //���I�p�A�C�e���̃��X�g(Treasure�N���X��lotteryId�ɂ��C���f�b�N�X��؂�ւ���)
    [HideInInspector] public List<List<GameObject>> lotteryitemList = new List<List<GameObject>>();
    //���O
    [HideInInspector] public List<string> logMessage = new List<string>();
    //�ړ��s�I�u�W�F�N�g�̍��W���i�[���郊�X�g
    [HideInInspector] public List<Vector2> unmovableList = new List<Vector2>();

    //�}�l�[�W���[
    private BoardManager boardScript;
    private EventManager eManager;
    private PanelManager pManager;

    //���̑�
    [Header("���x���A�b�v�ɂ��HP�̏㏸�l")] public int riseValueHp;
    //�G�������ۂɎ��̈ړ��_��ێ�����
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
    //���x�����J�n����O�ɑҋ@���鎞�ԁi�b�P�ʁj�B
    public float levelStartDelay = 2f;
    //�e�v���C���[�̃^�[���Ԃ̒x���B
    public float turnDelay = 0.05f;
    public static GManager instance = null;
    private bool enemiesMoving;

    //Start is called before the first frame update
    void Awake()
    {
        //�C���X�^���X�̊m�F
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //�C���X�^���X�����݂��邪�A����ł͂Ȃ��ꍇ
        else if (instance != this)
        {
            //GameManager�̃C���X�^���X��1�������݂ł��Ȃ�
            Destroy(gameObject);
        }

        //�Q�[���̊J�n����
        instance.InitGame();
    }

    //�e���x���̃Q�[�������������܂��B
    public void InitGame()
    {
        //���X�g�̃��Z�b�g
        unmovableList.Clear();
        enemies.Clear();
        doingSetup = true;
        StartCoroutine(settingMapAndEnemies());
    }

    /**
     * �����ݒ菈��
     */
    private IEnumerator settingMapAndEnemies()
    {
        //�p�l���̃C���X�^���X���擾
        mapLoadingImage = GameObject.Find("MapLoadingImage");
        if (mapLoadingImage != null)
        {
            //���݂̊K������ʂɕ\��
            mapLoadingText = GameObject.Find("MapLoadingText").GetComponent<Text>();
            mapLoadingText.text = instance.hierarchyLevel + " F";
        }

        nowStairs = GameObject.Find("NowStairs").GetComponent<Text>();

        //�}�l�[�W���[�̃C���X�^���X���擾
        //�}�b�v�̃����_������
        boardScript = GetComponent<BoardManager>();
        boardScript.Init();

        //�p�l���}�l�[�W���[
        pManager = GetComponent<PanelManager>();
        pManager.Init();

        //�C�x���g�}�l�[�W���[
        eManager = GetComponent<EventManager>();

        // �������ꂽ�G�I�u�W�F�N�g���擾
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        statusComponentPlayer = playerObj.GetComponent<StatusComponentPlayer>();
        //�쐬�����G�l�~�[��id������U��A���X�g�Ɋi�[����
        for (var i = 0; i < enemyObjects.Length; i++)
        {
            AddEnemyToList(enemyObjects[i], i);
        }
        yield return new WaitForSeconds(2.0f);
        //����������ɉ�ʕ\��
        hideLevelImage();
    }

    //�����\���̍����摜���\���ɂ���
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

    //�X�V�̓t���[�����ƂɌĂяo����܂��B
    void Update()
    {
        // �Z�b�g�A�b�v��
        if (doingSetup)
        {
            return;
        }
        // �v���C���[�̃^�[���܂��͓G�̍s����
        if (playersTurn || enemiesMoving)
        {
            //NPC����ѓG�̈ړ����J�n���Ȃ��B
            return;
        }
        //���Ԃ�NPC�����݂���ꍇ
        if (fellows.Count > 0)
        {
            Vector2 tmpRefPosition = playerObj.playerBeforePosition;
            for (int i = 0; i < fellows.Count; i++)
            {
                //�O���̃L�����̈ʒu����(�擪�̏ꍇ�̓v���C���[�̈ʒu)
                Vector2 refPosition = tmpRefPosition;
                fellows[i].fellowAction(refPosition);
                tmpRefPosition = fellows[i].npcBeforePosition;
            }

        }
        //�G���s��������
        StartCoroutine(moveEnemies());
    }


    //hp��0�ɂȂ����G�����X�g����폜
    public void removeEnemyToList(Enemy targetEnemy)
    {
        enemies.Remove(targetEnemy);
    }

    //������Ăяo���āA�n���ꂽ�G��G�I�u�W�F�N�g�̃��X�g�ɒǉ����܂��B
    public void AddEnemyToList(GameObject enemy,int enemyNumber)
    {
        Enemy enemyObj = enemy.GetComponent<Enemy>();
        //enemyObj.enemyNumber = enemyNumber;
        //string enemyName = enemy.GetComponent<StatusComponentBase>().charName.name;
        //if (string.IsNullOrEmpty(enemyName))
        //{
        //    enemyName = "�G�l�~�[";
        //}
        //enemy.GetComponent<StatusComponentBase>().charName.name = enemyName + (enemyNumber + 1);
        ////enemyObj.enemyName = "�G�l�~�[" + (enemyNumber + 1);
        ////List�ɃG�l�~�[��ǉ�����
        enemies.Add(enemyObj);
        latestEnemyNumber++;
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

    /**
     * �G�����Ԃɓ������R���[�`��
     */
    IEnumerator moveEnemies()
    {
        enemyActionEndCount = 0;
        //�v���C���[�͈ړ��ł��Ȃ�
        enemiesMoving = true;
        //�G�I�u�W�F�N�g�̃��X�g�����[�v
        for (int i = 0; i < enemies.Count; i++)
        {
            //isAction��true(���ɍs�����Ă���)�̏ꍇ�͍s�������Ȃ�
            if (enemies[i].isAction)
            {
                continue;
            }
            //yield return null;
            if (i > enemies.Count -1)
            {
                break;
            }
            //�G���X�g�̃C���f�b�N�Xi�ɂ���G��moveEnemy�֐����Ăяo��
            enemies[i].moveEnemy();
        }
        Invoke("forciblyAdvanceTurn", 1.5f);
        //�S�Ă̓G���s�����I����܂ő҂�
        yield return new WaitUntil(() => enemyActionEndCount >= enemies.Count);
        CancelInvoke();
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].isAction = false;
        }
        //�G�̈ړ�������������AplayersTurn��true�ɐݒ肵�āA�v���[���[���ړ��ł���悤�ɂ���
        playersTurn = true;
        //�v���C���[�̍U���t���O���I�t�ɂ���
        //playerObj.isAttack = false;
        //�G�̈ړ�������������AenemiesMoving��false�ɐݒ�
        enemiesMoving = false;
        //�ړ��_����ɂ���
        charsNextPosition.Clear();
        //�Q�[�����C�x���g����
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
     * ���炩�̖��œG�̃^�[�����I�����Ȃ������ꍇ�ɋ����I�Ƀ^�[����i�߂�
     */
    private void forciblyAdvanceTurn()
    {
        StartCoroutine(waitTurnFinish());
    }

    private IEnumerator waitTurnFinish()
    {
        grayImage.SetActive(true);
        loadingText.text = "�ҋ@��...";
        yield return new WaitForSeconds(1.0f);
        //�S�Ă̓G�̍s�����������Ă��Ȃ��ꍇ�͋����I�Ƀ^�[����i�߂�
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
