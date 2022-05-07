using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

//Map������script���L�q����
public class BoardManager : MonoBehaviour
{
    //Map��Ƀ����_����������A�C�e���ŏ��l�A�ő�l�����߂�class
    public class Count
    {
        public int minmum;
        public int maximum;

        public Count(int min, int max)
        {
            minmum = min;
            maximum = max;
        }
    }

    //�W���̃I�u�W�F�N�g���\������v�f��
    public class RandomCreatePoint
    { 
        public int minmum;
        public int maximum;
        public RandomCreatePoint(int min,int max)
        {
            minmum = min;
            maximum = max;
        }
    }

    //�Œ�I�u�W�F�N�g�̃T�C�Y(�����A��)�Ɛ������@
    public class BuildingObject
    {
        //�㉺��
        public int height;
        //���E��
        public int width;
        public int minmum;
        public int maximum;
        public BuildingObject(int x,int y, int min, int max)
        {
            height = y;
            width = x;
            minmum = min;
            maximum = max;
        }
    }

    //�t���A�̑���
    [System.Serializable]
    public class floorAttribute
    {
        public GameObject floorObj;
        public int settingMapObjRule;
        public int difficultyID;
        public floorAttribute(GameObject floor,int map,int difID)
        {
            floorObj = floor;
            settingMapObjRule = map;
            difficultyID = difID;
        }
    }

    //Map�̏c��
    [Header("�}�b�v�̍s��")]public int rows;
    [Header("�}�b�v�̗�")]public int columns;

    //��������A�C�e���̌�
    public Count Wallcount = new Count(3, 9);
    public Count foodcount = new Count(3, 5);
    public Count portionCount = new Count(2,3);
    public Count swordCount = new Count(3, 3);
    public Count shieldCount = new Count(3, 3);

    //���E���̗����͈�
    public Count borderRandom = new Count(-2,2);

    //�W���̐������̍\���v�f��
    public RandomCreatePoint singleMountain = new RandomCreatePoint(5,8);
    public RandomCreatePoint singleForest = new RandomCreatePoint(6, 9);

    //MAP�̍ޗ�
    //�t���A
    [Header("�o��")] public GameObject exit;
    [Header("�����t���A")] public GameObject grassFloor;
    [Header("�y�t���A")] public GameObject soilFloor;
    [Header("�΃t���A")] public GameObject stoneFloor;
    [Header("�����t���A")] public GameObject desertFloor;

    //�}�b�v�p�I�u�W�F�N�g(�X�e�[�^�X���Ɍ��ʂ���)
    [Header("�y�R")] public GameObject soilMountain;
    [Header("�y�R2")] public GameObject soilMountain2;
    [Header("��R")] public GameObject StoneMountain;
    [Header("�L�t��")] public GameObject HardWood;
    [Header("�j�t��")] public GameObject Conifer;
    [Header("����")] public GameObject WaterPlace;
    [Header("�O��")] public GameObject OuterWall;

    //�}�b�v�p�I�u�W�F�N�g(�����p)
    [Header("�΃^�C��")] public GameObject stoneTile;

    //�Œ�I�u�W�F�N�g
    public BuildingObject castle = new BuildingObject(9, 4, 1, 1);
    public BuildingObject building = new BuildingObject(4, 2, 1, 1);

    //�G�l�~�[
    [Header("�G")] public GameObject enemy;
    [Header("�G2")] public GameObject enemy2;

    //�A�C�e��(��)
    [Header("��")] public GameObject treasure;

    //�A�C�e��(����n)
    [Header("�H�ו�")] public GameObject food;
    [Header("�|�[�V����")] public GameObject portion;
    [Header("�{(�U���͏㏸)")] public GameObject bookAttackUp;
    [Header("�{(�h��͏㏸)")] public GameObject bookDefenceUp;
    [Header("�{(�S�̍U��)")] public GameObject bookDamageAllEnemy;

    //�A�C�e��(�����n)
    [Header("����")] public GameObject sword;
    [Header("��")] public GameObject shield;

    //�A�C�e��(�o�b�O)
    [Header("�o�b�O(�������𑝂₷)")] public GameObject bag;

    //�v���C���[
    [Header("�v���C���[")] public GameObject player;

    //�����n�I�u�W�F�N�g
    [Header("��(�Œ�I�u�W�F�N�g)")] public GameObject castleObj;
    [Header("����1")] public GameObject buildingObj;

    //MAp�쐬�p�̃p�����[�^
    [Header("�I�u�W�F�N�g�̖��W��̃p�����[�^")] public int denceParam = 4;
    [Header("�t���A��؂�p�����[�^")] public float borderParam = 0.25f;
    [Header("�}�b�v�̕�����")] public int borderDivision = 3;
    [Header("���E��")] public int secondBorder = 8;

    //��悲�Ƃ̑���
    [Header("�������̑���")] public floorAttribute grassDivisionAttribute;
    [Header("�y���̑���")] public floorAttribute soilDivisionAttribute;
    [Header("����̑���")] public floorAttribute stoneDivisionAttribute;
    [Header("�������̑���")] public floorAttribute desertDivisionAttribute;

    //NPC
    [Header("�e�X�g�pNPC")] public GameObject testNpc;
    [Header("�v���C���[��HP���񕜂���e�X�g�pNPC")] public GameObject testRecoveryHpNpc;
    [Header("�A�C�e����n���e�X�g�pNPC(���򂠂�)")] public GameObject testGiveItemNpcBranchMessage;
    [Header("��b����e�X�g�pNPC")] public GameObject testBranchMessageNpc;
    [Header("����e�X�g�pNPC")] public GameObject testSalesNpc;
    [Header("�A�C�e����n���e�X�g�pNPC(���b�Z�[�W�\�����Ɏ����œn��)")] public GameObject autoGiveItem;
    [Header("���ԂɂȂ�NPC")] public GameObject fellowTestNpc;

    //�ϊ��p
    private Transform boardHolder;

    //�I�u�W�F�N�g(�W����)�ݒu���̗����擾�Ď��s�̋��e��
    private int permitMaxSettingNum = 5;
    //�I�u�W�F�N�g(�P��)�ݒu���̗����擾�Ď��s�̋��e��
    private int permitMaxSettingSingleNum;
    private int isTop = 1;
    private int isBottom = -1;
    private int isRight = 1;
    private int isLeft = -1;
    private bool verticalPlus = false;
    private int thirdBorder;
    private int nextLineValue;
    private int randomSettingObjectNum;
    //���敪�̉���
    private int singleDivisionColumns;
   
    //���X�g
    //object���Ȃ��ꏊ���Ǘ�����
    private List<Vector3> gridPositons = new List<Vector3>();
    //�}�b�v�����̍ۂɎ擾�������X�g�̃C���f�b�N�X���i�[���郊�X�g
    private List<int> mapListIndex = new List<int>();
    //�Œ�I�u�W�F�N�g�ݒu���Ɉꎞ�I�ɃC���f�b�N�X���i�[���郊�X�g
    private List<int> createObjIndexList = new List<int>();
    //�쐬�����}�b�v�I�u�W�F�N�g���i�[���郊�X�g
    private List<floorAttribute> createMapTileList = new List<floorAttribute>();
    //�}�b�v�I�u�W�F�N�g���i�[���郊�X�g
    private List<floorAttribute> mapTileList = new List<floorAttribute>();
    //���E�����i�[���郊�X�g
    private List<int> mapBorderList = new List<int>();
    //���I�p�̃A�C�e�����X�g
    List<GameObject> lotteryList = new List<GameObject>();
    List<GameObject> lotteryList2 = new List<GameObject>();

    //�t�B�[���h����
    /**
     * y������ɃI�u�W�F�N�g��ݒu����
     */
    void BoardSetup()
    {
        secondBorder = (int)(columns * borderParam) + Random.Range(1, 2);
        thirdBorder = secondBorder * 2 + Random.Range(1, 2);
        //Board���C���X�^���X������boardHolder�ɐݒ�
        boardHolder = new GameObject("Board").transform;
        int border;
        int border2;
        for (int y = -1; y < rows + 1; y++)
        {
            border = secondBorder + Random.Range(borderRandom.minmum, borderRandom.maximum);
            border2 = thirdBorder + Random.Range(borderRandom.minmum, borderRandom.maximum);
            //thirdBorder += Random.Range(borderRandom.minmum, borderRandom.maximum);
            for (int x = -1; x < columns + 1; x++)
            {
                //����ݒu���ăC���X�^���X���̏���
                GameObject toInsutantiate = grassFloor;

                if (x >= border)
                {
                    toInsutantiate = soilFloor;
                }
                if (x >= border2)
                {
                    toInsutantiate = stoneFloor;
                }

                //8�~8�}�X�O�͊O�ǂ�ݒu���ăC���X�^���X���̏���
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInsutantiate = OuterWall;
                }

                //toInsutantiate�ɐݒ肳�ꂽ���̂��C���X�^���X��
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                //�C���X�^���X�����ꂽ��or�O�ǂ̐e�v�f��boardHolder�ɐݒ�
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /**
     * y������ɃI�u�W�F�N�g��ݒu����(���E���̔�������[�v�ōs��)
     */
    void BoardSetupRemodel()
    {
        GameObject toInsutantiate = null;
        for (int y = -1; y < rows + 1; y++)
        {
            for (int x = -1; x < columns + 1; x++)
            {
                //8�~8�}�X�O�͊O�ǂ�ݒu���ăC���X�^���X���̏���
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInsutantiate = OuterWall;
                }
                else
                {
                    int randomBorder = x!=0? Random.Range(borderRandom.minmum, borderRandom.maximum) : 0;
                    for (int i = 0; i < borderDivision; i++)
                    {
                        if (x >= mapBorderList[i] + randomBorder)
                        {
                            toInsutantiate = mapTileList[i].floorObj;
                        }
                    }
                }

                //toInsutantiate�ɐݒ肳�ꂽ���̂��C���X�^���X��
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                //�C���X�^���X�����ꂽ��or�O�ǂ̐e�v�f��boardHolder�ɐݒ�
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /**
     * ������(x������)�Ƀ}�b�v�𐶐�����
     */
    private void boardSetUp2()
    {
        GameObject toInsutantiate = null;
        GameObject tempToInsutantiate = null;
        bool isRandomSet = false;
        //Board���C���X�^���X������boardHolder�ɐݒ�
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            //��悲�ƂɃt���A�[��ݒu
            for (int i=0;i< borderDivision; i++)
            {
                //���E����̏ꍇ�͓��ꏈ��
                if (x == mapBorderList[i])
                {
                    isRandomSet = true;
                    tempToInsutantiate = mapTileList[i].floorObj;
                    break;
                }
                else if (x > mapBorderList[i])
                {
                    tempToInsutantiate = mapTileList[i].floorObj;
                }
            }
            for (int y = -1; y < rows + 1; y++)
            {
                toInsutantiate = tempToInsutantiate;
                //8�~8�}�X�O�͊O�ǂ�ݒu���ăC���X�^���X���̏���
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInsutantiate = OuterWall;
                }
                //toInsutantiate�ɐݒ肳�ꂽ���̂��C���X�^���X��
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                //�C���X�^���X�����ꂽ��or�O�ǂ̐e�v�f��boardHolder�ɐݒ�
                instance.transform.SetParent(boardHolder);
                if (isRandomSet)
                {
                    int reSettingRange = Random.Range(1, 6);
                    for (int k = 1; k < reSettingRange; k++)
                    {
                        //toInsutantiate�ɐݒ肳�ꂽ���̂��C���X�^���X��
                        instance =
                            Instantiate(toInsutantiate, new Vector3(x - k, y, -1), Quaternion.identity) as GameObject;
                        //�C���X�^���X�����ꂽ��or�O�ǂ̐e�v�f��boardHolder�ɐݒ�
                        instance.transform.SetParent(boardHolder);
                    }
                }
            }
            isRandomSet = false;
        }
    }

    /**
     * ����ł����ׂ����Ȃ��H
     * �ʏ펞��x�������Ƀ}�b�v�𐶐����A����̃G���A��(���E������)�̂�y�������Ƀ}�b�v�𐶐�����
     */
    private void boardSetUp2Remodel()
    {
        GameObject toInsutantiate = null;
        GameObject tempToInsutantiate = null;
        GameObject tempNextToInsutantiate = null;
        bool isRandomSettingLoop = false;
        int randomAreaBorder = Random.Range(-5, -6);
        //Board���C���X�^���X������boardHolder�ɐݒ�
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            //x���X�V�����^�C�~���O�Őݒu����I�u�W�F�N�g�𔻒肷��
            for (int i = 0; i < borderDivision; i++)
            {
                //x�������_���ɐݒu����G���A�ɓ������ꍇ
                if (mapBorderList[i]+randomAreaBorder > 0 && x > mapBorderList[i] + randomAreaBorder && x < mapBorderList[i])
                {
                    isRandomSettingLoop = true;
                    tempNextToInsutantiate = mapTileList[i].floorObj;
                    tempToInsutantiate = mapTileList[i-1].floorObj;
                    break;
                }
                else if (x >= mapBorderList[i])
                {
                    //Debug.Log("nomalarea");
                    tempToInsutantiate = mapTileList[i].floorObj;
                }
            }
            //�����_���ɔz�u����G���A(y�����x�������Ƀ��[�v)
            if (isRandomSettingLoop)
            {
                //y�����Œ�
                for (int y=-1; y < rows + 1; y ++)
                {
                    //�s���ƂɃt���A�̐؂�ւ��������_���ɂ���
                    int randomXBorder = Random.Range(1, (-1) * randomAreaBorder);
                    for (int randomAreaX=x; randomAreaX < x - randomAreaBorder; randomAreaX++)
                    {
                        toInsutantiate = tempToInsutantiate;
                        if (randomAreaX >= x + randomXBorder)
                        {
                            toInsutantiate = tempNextToInsutantiate;
                        }
                        if (y == -1 || y == rows)
                        {
                            toInsutantiate = OuterWall;
                        }
                        //Debug.Log("randomsettingobj" + toInsutantiate);
                        //toInsutantiate�ɐݒ肳�ꂽ���̂��C���X�^���X��
                        GameObject instance =
                            Instantiate(toInsutantiate, new Vector3(randomAreaX, y, 0), Quaternion.identity) as GameObject;
                        //�C���X�^���X�����ꂽ��or�O�ǂ̐e�v�f��boardHolder�ɐݒ�
                        instance.transform.SetParent(boardHolder);
                    }
                }
                isRandomSettingLoop = false;
                x += (-1)*randomAreaBorder -1;
            }
            //�ʏ�̃t���A�z�u
            else
            {
                for (int y = -1; y < rows + 1; y++)
                {
                    toInsutantiate = tempToInsutantiate;
                    //8�~8�}�X�O�͊O�ǂ�ݒu���ăC���X�^���X���̏���
                    if (x == -1 || x == columns || y == -1 || y == rows)
                    {
                        toInsutantiate = OuterWall;
                    }
                    //toInsutantiate�ɐݒ肳�ꂽ���̂��C���X�^���X��
                    GameObject instance =
                        Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    //�C���X�^���X�����ꂽ��or�O�ǂ̐e�v�f��boardHolder�ɐݒ�
                    instance.transform.SetParent(boardHolder);
                }
            }
        }
    }

    /**
     * �t���A�I�u�W�F�N�g�����X�g�Ɋi�[����(�V�K�ŃI�u�W�F�N�g���쐬�����ꍇ�ɒǉ�����)
     */
    private void inputFloorObj()
    {
        //�t���A�I�u�W�F�N�g�����X�g�Ɋi�[����
        createMapTileList.Add(grassDivisionAttribute);
        createMapTileList.Add(soilDivisionAttribute);
        //createMapTileList.Add(stoneDivisionAttribute);
        createMapTileList.Add(desertDivisionAttribute);
    }

    /**
     * �t���A�[�I�u�W�F�N�g�����X�g�Ɋi�[����
     */
    private void createFloorTileList()
    {
        //�t���A�I�u�W�F�N�g�����X�g�Ɋi�[����
        inputFloorObj();
        for (int i=0;i< borderDivision;i++)
        {
            mapTileList.Add(createMapTileList[i% createMapTileList.Count]);
        }
        //mapTileList.Add(floor);
        //mapTileList.Add(floor2);
        //mapTileList.Add(floor3);
        //���X�g���V���b�t��
        for (int i = mapTileList.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1); // �����_���ŗv�f�ԍ����P�I�ԁi�����_���v�f�j
            floorAttribute temp = mapTileList[i]; // ��ԍŌ�̗v�f�����m�ہitemp�j�ɂ����
            mapTileList[i] = mapTileList[j]; // �����_���v�f����ԍŌ�ɂ����
            mapTileList[j] = temp; // ���m�ۂ��������_���v�f�ɏ㏑��
        }
        //�t���A�̓�Փx���Ƀ\�[�g����
        for (int i=0;i<mapTileList.Count ;i++)
        {
            for (int j=i+1;j<mapTileList.Count ;j++)
            {
                if (mapTileList[i].difficultyID > mapTileList[j].difficultyID)
                {
                    floorAttribute temp = mapTileList[i];
                    mapTileList[i] = mapTileList[j];
                    mapTileList[j] = temp;
                }
            }
        }
    }

    /**
     * �t���A�[�ݒu�p�̋��E�������X�g�Ɋi�[����
     */
    private void createBorderLineList()
    {
        singleDivisionColumns = columns / borderDivision;
        for (int i=0;i< borderDivision + 1; i++)
        {
            mapBorderList.Add(singleDivisionColumns * i);
        }
    }

    //gridPositions���N���A����
    void InitialiseList(int startLine,int endLine)
    {
        //���X�g���N���A����
        gridPositons.Clear();
        for (int y = 1;y < rows -1; y++)
        {
            for (int x = startLine;x< endLine -1; x++)
            {
                //gridPositions��x,y�̒l�������
                gridPositons.Add(new Vector3(x, y, 0));
            }
        }
        Debug.Log("startline"+startLine);
        Debug.Log("endline"+endLine);
        Debug.Log("gridcountinit"+gridPositons.Count);
    }

    //gridPositions���烉���_���Ȉʒu���擾����
    Vector3 RandomPosition()
    {
        //randomIndex��錾���āAgridPositions�̐����琔�l�������_���œ����
        int randomIndex = Random.Range(0, gridPositons.Count);

        //randomPosition��錾���āAgridPositions��randomIndex�ɐݒ肷��
        Vector3 randomPosition = gridPositons[randomIndex];
        Debug.Log("gridpositioncount"+gridPositons.Count);
        Debug.Log("removeatindex"+randomIndex);
        //�g�p����gridPositions�̗v�f���폜
        gridPositons.RemoveAt(randomIndex);
        Debug.Log("gridpositioncount" + gridPositons.Count);
        return randomPosition;
    }

    //Map�Ƀ����_���ň����̂��̂�z�u����(�G�A�ǁA�A�C�e��)
    void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        //tile���Z�b�g����Ă��Ȃ��ꍇ��return
        if (tile == null)
        {
            return;
        }
        //��������A�C�e���̌����ŏ��ő�l���烉���_���Ɍ��߁AobjectCount�ɐݒ肷��
        int objectCount = Random.Range(minimum, maximum);
        //���蓖�ėp�̍��W���]���Ă���ꍇ�̂ݔz�u�\
        //�ݒu����I�u�W�F�N�g�̐������[�v�ŉ�
        for (int i = 0; i < objectCount; i++)
        {
            if (gridPositons.Count > 0)
            {
                //���݃I�u�W�F�N�g���u����Ă��Ȃ��A�����_���Ȉʒu���擾
                Vector3 randomPosition = RandomPosition();

                //����
                Instantiate(tile, randomPosition, Quaternion.identity);
            }
        }
    }

    /**
     * ��悲�ƂɃI�u�W�F�N�g(�A�C�e���A�G�Ȃ�)��ݒu����
     */
    private void settingGameObjectToDivisionArea()
    {
        for (int i = 0; i < borderDivision; i++)
        {
            if (i < borderDivision)
            {
                InitialiseList(mapBorderList[i] + 1, mapBorderList[i + 1]);
                nextLineValue = mapBorderList[i + 1];
                settingObjectBySection(mapTileList[i],i);
            }
        }
    }

    /**
     * �󔠂���o������A�C�e���̃��X�g���쐬
     */
    private void addTreasureItemList()
    {
        GManager.instance.treasureItemList.Add(food.GetComponent<Item>());
    }

    /**
     * �󔠂���o������A�C�e���̃��X�g���쐬
     */
    private void createTreasureItemList()
    {
        lotteryList.Add(portion);
        lotteryList2.Add(food);
        GManager.instance.lotteryitemList.Add(lotteryList);
        GManager.instance.lotteryitemList.Add(lotteryList2);
    }

    //�Q�[���{�[�h�����C�A�E�g����
    public void SetupScene(int level)
    {
        createTreasureItemList();
        createFloorTileList();
        createBorderLineList();
        //�O�ǂƏ����쐬���܂�
        boardSetUp2Remodel();
        ////�v���C���[��z�u
        ////gridPositons.RemoveAt(0);
        //mapListIndex.Add(0);
        Instantiate(player, new Vector3(1, 1, 0f), Quaternion.identity);
        //��悲�ƂɃI�u�W�F�N�g(�A�C�e���A�G�Ȃ�)��ݒu����
        settingGameObjectToDivisionArea();
    }

    /**
     * �}�b�v�̊e���ɃI�u�W�F�N�g��ݒu����
     */
    private void settingObjectBySection(floorAttribute floorDivisionObj,int settingObjRule)
    {
        mapListIndex.Clear();
        //�����t���A
        if (floorDivisionObj.settingMapObjRule == Define.FOREST_DIVISION)
        {
            //�؃I�u�W�F�N�g(�W����)��ݒu
            createMapRandomObjects(HardWood);
            //�΃^�C��(�����p)��ݒu
            //createMapRandomObjects(stoneTile);
        }
        //�y�t���A
        else if (floorDivisionObj.settingMapObjRule == Define.SOIL_DIVISION)
        {
            //�y�R�̃I�u�W�F�N�g(�W����)��ݒu
            //createMapRandomObjects(soilMountain, 1);
        }
        //��G���A
        else if (floorDivisionObj.settingMapObjRule == Define.STONE_DIVISION)
        {
            //��R�̃I�u�W�F�N�g(�W����)��ݒu
            //createMapRandomObjects(StoneMountain, 1);
        }
        //�����G���A
        else if (floorDivisionObj.settingMapObjRule == Define.DESERT_DIVISION)
        {
            //�y�R�̃I�u�W�F�N�g(�W����)��ݒu
            //createMapRandomObjects(WaterPlace, 1);
        }

        //�g�p�������X�g���폜����
        deleteRandomPoinst();

        //�I�u�W�F�N�g�ݒu���[������ɃI�u�W�F�N�g��ݒu
        //�����
        if (settingObjRule == Define.FIRST_SETTING)
        {
            //�H�ו����C���X�^���X���B
            LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum);
            //�|�[�V�������C���X�^���X��
            //LayoutObjectAtRandom(portion, portionCount.minmum, portionCount.maximum);
            //�󔠂��C���X�^���X��
            LayoutObjectAtRandom(treasure, portionCount.minmum, portionCount.maximum);
            //�J�o�����C���X�^���X���B
            LayoutObjectAtRandom(bag, 1, 1);
            //NPC���C���X�^���X��
            LayoutObjectAtRandom(testNpc, 1, 1);
            //NPC(�v���C���[��HP���񕜂���)���C���X�^���X��
            //LayoutObjectAtRandom(testRecoveryHpNpc, 1,1);
            //NPC(���򂠂�̃A�C�e���̈����n��)���C���X�^���X��
            LayoutObjectAtRandom(testGiveItemNpcBranchMessage,1,1);
            //NPC(��b����p)���C���X�^���X��
            //LayoutObjectAtRandom(testBranchMessageNpc,1,1);
            //NPC(���)���C���X�^���X��
            LayoutObjectAtRandom(testSalesNpc,1,1);
            //NPC(����)���C���X�^���X��
            LayoutObjectAtRandom(fellowTestNpc, 1, 1);
            //�A�C�e����n���e�X�g�pNPC(���b�Z�[�W�\�����Ɏ����œn��)
            //LayoutObjectAtRandom(autoGiveItem,1,1);
            //�{���C���X�^���X��
            LayoutObjectAtRandom(bookDamageAllEnemy, 3,3);
        }
        //�����
        else if(settingObjRule == Define.SECOND_SETTING)
        {
            //�����C���X�^���X��
            LayoutObjectAtRandom(sword, swordCount.minmum, swordCount.maximum);
        }

        //�����_�������ꂽ�ʒu�ŁA�ŏ��l�ƍő�l�Ɋ�Â��ă����_���Ȑ��̕ǃ^�C�����C���X�^���X�����܂��B
        //LayoutObjectAtRandom(Wall, Wallcount.minmum, Wallcount.maximum);

        ////�����_�������ꂽ�ʒu�ŁA�ŏ��l�ƍő�l�Ɋ�Â��ă����_���Ȑ��̐H�i�^�C�����C���X�^���X�����܂��B
        //LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum);

        ////�|�[�V�������C���X�^���X��
        //LayoutObjectAtRandom(portion, portionCount.minmum, portionCount.maximum);

        ////�����C���X�^���X��
        //LayoutObjectAtRandom(sword, swordCount.minmum, swordCount.maximum);

        //�����C���X�^���X��
        //LayoutObjectAtRandom(shield, swordCount.minmum, swordCount.maximum);

        //�ΐ��i�s�Ɋ�Â��āA���݂̃��x�����Ɋ�Â��ēG�̐������肵�܂�
        int enemyCount = (int)Mathf.Log(2, 2f);
        ////Debug.Log(enemyCount);
        enemyCount = 2;
        //�����_�������ꂽ�ʒu�ŁA�ŏ��l�ƍő�l�Ɋ�Â��ă����_���Ȑ��̓G���C���X�^���X�����܂��B
        LayoutObjectAtRandom(enemy, enemyCount, enemyCount);
        LayoutObjectAtRandom(enemy2, enemyCount - 1, enemyCount - 1);

        //�Q�[���{�[�h�̉E����ɏo���^�C�����C���X�^���X�����܂�
        //Instantiate(exit, new Vector3(rows - 1, columns - 1, 0f), Quaternion.identity);
        //Instantiate(exit, new Vector3(0, 3, 0f), Quaternion.identity);
    }

    public void destroyBoard()
    {
        Destroy(boardHolder);
    }

    public void LayoutEnemyAtRandom(GameObject tile, int minimum, int maximum)
    {
        //��������A�C�e���̌����ŏ��ő�l���烉���_���Ɍ��߁AobjectCount�ɐݒ肷��
        int objectCount = Random.Range(minimum, maximum);

        //�ݒu����I�u�W�F�N�g�̐������[�v�ŉ�
        for (int i = 0; i < objectCount; i++)
        {
            //���݃I�u�W�F�N�g���u����Ă��Ȃ��A�����_���Ȉʒu���擾
            Vector3 randomPosition = RandomPosition();

            //����
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }

    /**
     * ������p���ăI�u�W�F�N�g(�W����)�𐶐�����
     * �ŏ��̈�_���擾�����ɃI�u�W�F�N�g��ݒu����
     * @param tile�@�ݒu����I�u�W�F�N�g
     */
    public void createMapRandomObjects(GameObject tile)
    {
        TileBase tileObj;
        //�ݒu����I�u�W�F�N�g��TileBase���A�^�b�`����Ă��Ȃ��ꍇ�͐ݒu�s��
        if ((tileObj = tile.GetComponent<TileBase>()) == null)
        {
            return;
        }
        //�ݒu���[���������_���̏ꍇ�́A���[��0�`2����
        int settingAlgorithmNum = tileObj.settingFuncRule == Define.RANDOM_SETTING ? Random.Range(Define.NORMAL_SETTING, Define.RANDOM_SETTING) : tileObj.settingFuncRule;
        //�I�u�W�F�N�g(�W����)�̐ݒu�����v�Z
        int mapSingleDivisionSpace = rows * singleDivisionColumns;
        int rectangleSpace = 1;
        if (settingAlgorithmNum == Define.NORMAL_SETTING || settingAlgorithmNum == Define.DENCE_SETTING)
        {
           
        }
        else
        {
            rectangleSpace = (int)((singleDivisionColumns * tileObj.horizontalParam) * (rows*tileObj.verticalParam));
        }
        Debug.Log("singleDivisionColumns * tileObj.horizontalParam"+ singleDivisionColumns * tileObj.horizontalParam);
        Debug.Log("rows*tileObj.verticalParam"+ rows * tileObj.verticalParam);
        Debug.Log("mapSingleDivisionSpace"+ mapSingleDivisionSpace);
        Debug.Log("rectangleSpace"+ rectangleSpace);
        Debug.Log("mapSingleDivisionSpace/rectangleSpace"+ mapSingleDivisionSpace / rectangleSpace) ;
        int settingNum = (int)((mapSingleDivisionSpace / rectangleSpace) * 0.2);
        Debug.Log("settingNum"+settingNum);
        //�����̐������I�u�W�F�N�g(�I�u�W�F�N�g�̏W����)���쐬����
        for (int i=0;i< settingNum; i++)
        {
            var permitCounter = 0;
            int getIndex;
            isTop = 1;
            isBottom = -1;
            isRight = 1;
            isLeft = -1;
            verticalPlus = false;
            do
            {
                getIndex = getRandomPositionForMap();
                permitCounter++;
            }
            while (!checkRandomListIndex(getIndex) && permitCounter < permitMaxSettingNum);
            //�C���f�b�N�X�����������ꍇ�ɃI�u�W�F�N�g��ݒu����
            if (permitCounter < permitMaxSettingNum)
            {
                if (getIndex% nextLineValue == 0)
                {
                    isLeft = 0;
                }
                else if (getIndex% nextLineValue == nextLineValue - 1)
                {
                    isRight = 0;
                }
                if (getIndex >= 0 && getIndex <= nextLineValue)
                {
                    isTop = 0;
                }
                else if (getIndex >= gridPositons.Count- nextLineValue && getIndex <= gridPositons.Count-1)
                {
                    isBottom = 0;
                }
                Debug.Log("getindex" + getIndex);
                mapListIndex.Add(getIndex);
                Vector3 randomPosition = gridPositons[getIndex];
                Debug.Log("firstPosition"+randomPosition);
                Instantiate(tile, gridPositons[getIndex], Quaternion.identity);
                //TileBase�Œ�`���ꂽ�I�u�W�F�N�g�̐ݒu���[���ɏ]���֐����Ăяo��
                if (settingAlgorithmNum == Define.NORMAL_SETTING)
                {
                    createTile(tile, getIndex, tileObj.minSettingNum,tileObj.maxSettingNum);
                }
                else if (settingAlgorithmNum == Define.DENCE_SETTING)
                {
                    createDenceTile(tile, getIndex, tileObj.minSettingNum, tileObj.maxSettingNum);
                }
                else if (settingAlgorithmNum == Define.STRETCHING_SETTING)
                {
                    createDenceTileVertical(tile, getIndex,tileObj.verticalParam);
                }
                else
                {
                    createDenceTileVertical(tile, getIndex,tileObj.verticalParam);
                }
                //if (isDence)
                //{
                //    //createDenceTile(tile, getIndex, createNum);
                //    createDenceTileVertical(tile, getIndex);
                //}
                //else
                //{
                //    createTile(tile, getIndex, createNum);
                //}
            }
        }
    }

    /**
     * �����Ŏ擾�����_�̎���ɃI�u�W�F�N�g(�P��)��ݒu����
     * @param tile �ݒu����Q�[���I�u�W�F�N�g
     * @param position �ŏ��Ɏ擾����n�_
     */
    public void createTile(GameObject tile, int getIndex, int minNum,int maxNum)
    {
        //�����Œl�擾(�ݒu����I�u�W�F�N�g�̐�)
        int randomNum = Random.Range(minNum, maxNum);
        Debug.Log("randomnum" + randomNum);
        int minXRange = -1;
        int maxXRange = 1;
        int minYRange = -1;
        int maxYRange = 1;
        //�����̐��������[�v
        for (int i=0;i<randomNum;i++)
        {
            //�ŏ��Ɏ擾�����_�t�߂Ń����_���ȓ_���擾
            int permitCount = 0 ;
            int listIndex;
            //�I�u�W�F�N�g�ݒu�_�T���̏����
            permitMaxSettingSingleNum = (maxXRange-minXRange+1) * (maxYRange-minYRange+1) - 1;
            do
            {
                //�ŏ��Ɏ擾�����_�t�߂Ń����_���ȓ_���擾
                float settingPointX = Random.Range(minXRange, maxXRange);
                float settingPointY = Random.Range(minYRange, maxYRange);
                listIndex = getMapIndex(settingPointX, settingPointY, getIndex);
                permitCount++;
            }
            while (!checkRandomListIndex(listIndex) && permitCount < permitMaxSettingSingleNum);
            //�ݒu�͈͊O�̃C���f�b�N�X���擾�����ꍇ
            if (listIndex == -1)
            {
                return;
            }
            if (permitCount < permitMaxSettingSingleNum)
            {
                mapListIndex.Add(listIndex);
                Instantiate(tile, gridPositons[listIndex], Quaternion.identity);
            }
            minXRange += -1;
            maxXRange += 1;
            minYRange += -1;
            maxYRange += 1;
        }
    }

    /**
     * �����Ŏ擾�����_�̎���ɃI�u�W�F�N�g(�P��)��ݒu����
     * createTile()��薧�W������Ԃɐݒu
     * @param tile �ݒu����Q�[���I�u�W�F�N�g
     * 
     */
    public void createDenceTile(GameObject tile, int getIndex, int minNum, int maxNum)
    {
        //�����Œl�擾(�ݒu����I�u�W�F�N�g�̐�)
        int randomNum = Random.Range(minNum, maxNum);
        Debug.Log("randomnum" + randomNum);
        int minXRange = isLeft;
        int maxXRange = isRight;
        int minYRange = isBottom;
        int maxYRange = isTop;
        //�����̐��������[�v
        for (int i = 0; i < randomNum; i++)
        {
            if (denceParam < 0)
            {
                denceParam = 1;
            }
            Debug.Log("denceparam"+denceParam);
            int permitCount = 0;
            int listIndex;
            //�I�u�W�F�N�g�ݒu�_�T���̏����
            permitMaxSettingSingleNum = (maxXRange - minXRange + 1) * (maxYRange - minYRange + 1) - 1;
            for (int j=0;j< denceParam; j++)
            {
                do
                {
                    //�ŏ��Ɏ擾�����_�t�߂Ń����_���ȓ_���擾
                    float settingPointX = Random.Range(minXRange, maxXRange);
                    float settingPointY = Random.Range(minYRange, maxYRange);
                    listIndex = getMapIndex(settingPointX, settingPointY, getIndex);
                    permitCount++;
                }
                while (!checkRandomListIndex(listIndex) && permitCount < permitMaxSettingSingleNum);
                //�ݒu�͈͊O�̃C���f�b�N�X���擾�����ꍇ
                if (listIndex == -1)
                {
                    return;
                }
                //�T�����e�ő�񐔂𒴂����ɃC���f�b�N�X���擾�ł����ꍇ
                if (permitCount < permitMaxSettingSingleNum)
                {
                    mapListIndex.Add(listIndex);
                    Debug.Log("settingposition"+gridPositons[listIndex]);
                    Instantiate(tile, gridPositons[listIndex], Quaternion.identity);
                }
                //�T�����e�ő�񐔂𒴂����ꍇ�͔͈͂��L���ĒT������
                else
                {
                    break;
                }
            }
            //���S�_���痣���قǂ΂炯�Đݒu�����
            denceParam--;
            //minXRange += Random.Range(-1,-1);
            //maxXRange += Random.Range(0,0);
            //minYRange += Random.Range(-1,0);
            //maxYRange += Random.Range(0,1);
            //int xParameter = Random.Range(-1, 0);
            //minXRange += xParameter;
            //if (xParameter == 0)
            //{
            //    maxXRange += 1;
            //}
            //int yParameter = Random.Range(-1, 0);
            //minYRange += yParameter;
            //if (yParameter == 0)
            //{
            //    maxYRange += Random.Range(0, 1);
            //}
          
            //�����n�_���}�b�v�̉E�[
            if (isRight == 0)
            {
                minXRange += -1;
            }
            //�����n�_���}�b�v�̍��[
            else if (isLeft == 0)
            {
                maxXRange += 1;
            }
            else
            {
                int xParameter = Random.Range(-1, 0);
                minXRange += xParameter;
                if (xParameter == 0)
                {
                    maxXRange += 1;
                }
            }
            //�����n�_���}�b�v�̏�
            if (isTop == 0)
            {
                minYRange += Random.Range(-1, 0);
            }
            //�����n�_���}�b�v�̉�
            else if (isBottom == 0)
            {
                maxYRange += Random.Range(0, 1);
            }
            else
            {
                int yParameter = Random.Range(-1, 0);
                minYRange += yParameter;
                if (yParameter == 0)
                {
                    maxYRange += Random.Range(0, 1);
                }
            }
        }
    }

    //gridPositions���烉���_���Ȉʒu���擾����
    int getRandomPositionForMap()
    {
        Debug.Log("gridcount" + gridPositons.Count);
        int randomIndex = Random.Range(0, gridPositons.Count);
        return randomIndex;
    }

    /**
     * �����Ŏ擾�������X�g�̃C���f�b�N�X�����Ɏg�p����Ă��Ȃ����`�F�b�N����
     */
    private bool checkRandomListIndex(int index)
    {
        for (int i=0;i< mapListIndex.Count;i++)
        {
            if (mapListIndex[i] == index)
            {
                return false;
            }
        }
        return true;
    }

    /**
     * colum��row���烊�X�g�̃C���f�b�N�X���擾
     */
    private int getMapIndex(float xPos,float Ypos,float randomIndex)
    {
        int returnIndex;
        returnIndex = Mathf.FloorToInt(randomIndex + xPos + Ypos*(nextLineValue));
        //Debug.Log("row" + columns);
        Debug.Log("argument" + randomIndex);
        Debug.Log("xpos" + xPos);
        Debug.Log("ypos" + Ypos);
        Debug.Log("returnindex" + returnIndex);
        //Debug.Log("gridcount"+gridPositons.Count);
        if (returnIndex < 0 || returnIndex > gridPositons.Count)
        {
            returnIndex = -1;
            Debug.Log("index not found");
        }
        return returnIndex;
    }

    /**
     * ���X�g����Y������C���f�b�N�X�̗v�f���폜����
     */
    private void deleteRandomPoinst()
    {
        mapListIndex.Sort();
        mapListIndex.Reverse();
        for (int i=0;i<mapListIndex.Count;i++)
        {
            gridPositons.RemoveAt(mapListIndex[i]);
        }
    }

    /**
     * �����擾�_����c�����ɃI�u�W�F�N�g��ݒu���������ɍL����
     */
    private void createDenceTileVertical(GameObject tile, int getIndex,float verticalSettingParam)
    {
        //������Ɋg��
        if (getIndex < (gridPositons.Count/2))
        {
            verticalPlus = true;
        }
        //int verticalParam = getIndex;
        //int verticalParam = Random.Range(2,rows-2);
        int verticalParam = (int)(rows * verticalSettingParam);
        Debug.Log("count" + verticalParam);
        for (int i=0;i< verticalParam; i++)
        {
            if (checkMapIndex(getIndex) == -1)
            {
                break;
            }
            if (checkRandomListIndex(getIndex))
            {
                mapListIndex.Add(getIndex);
                Debug.Log("grid" + gridPositons[getIndex]);
                Instantiate(tile, gridPositons[getIndex], Quaternion.identity);
            }
            createDenceTileHorizontal(tile, getIndex);
            //if (verticalPlus)
            //{
            //    verticalParam += (nextLineValue);
            //}
            //else
            //{
            //    verticalParam -= (nextLineValue);
            //}
            getIndex += verticalPlus ? nextLineValue:(-1)*nextLineValue;
            //verticalParam += columns;
            //createDenceTileHorizontal(tile,verticalParam);
            //if (checkMapIndex(verticalParam) == -1)
            //{
            //    break;
            //}
            //if (checkRandomListIndex(verticalParam))
            //{
            //    mapListIndex.Add(verticalParam);
            //    Debug.Log("grid"+gridPositons[verticalParam]);
            //    Instantiate(tile, gridPositons[verticalParam], Quaternion.identity);
            //}
        }
    }

    //verticalParam���獶�E�Ɋg������
    private void createDenceTileHorizontal(GameObject tile,int verticalParam)
    {
        //int rightParam = Random.Range(1,5);
        //int leftParam = Random.Range(1,5);
        int rightParam = Random.Range(1, (int)(singleDivisionColumns*tile.GetComponent<TileBase>().horizontalParam));
        int leftParam = Random.Range(1, (int)(singleDivisionColumns * tile.GetComponent<TileBase>().horizontalParam));
        int horizontalParam = verticalParam;
        Debug.Log("right"+rightParam);
        Debug.Log("left" + leftParam);
        //�E�����Ɋg��
        for (int i=0;i<rightParam;i++)
        {
            horizontalParam += 1;
            //Debug.Log("horizon"+horizontalParam);
            if (checkMapIndex(horizontalParam) == -1)
            {
                break;
            }
            if (checkRandomListIndex(horizontalParam))
            {
                Debug.Log("horizon" + horizontalParam);
                mapListIndex.Add(horizontalParam);
                Instantiate(tile, gridPositons[horizontalParam], Quaternion.identity);
            }
        }
        horizontalParam = verticalParam;
        //�������Ɋg��
        for(int j = 0; j < leftParam; j++)
        {
            horizontalParam -= 1;
            //Debug.Log("horizon" + horizontalParam);
            if (checkMapIndex(horizontalParam) == -1)
            {
                break;
            }
            if (checkRandomListIndex(horizontalParam))
            {
                Debug.Log("horizon" + horizontalParam);
                mapListIndex.Add(horizontalParam);
                Instantiate(tile, gridPositons[horizontalParam], Quaternion.identity);
            }
        }
    }

    /**
     * colum��row���烊�X�g�̃C���f�b�N�X���擾
     */
    private int checkMapIndex(int index)
    {
        Debug.Log("checkmapindex");
        Debug.Log("index"+index);
        Debug.Log("gridcount"+gridPositons.Count);
        if (index < 0 || index >= gridPositons.Count)
        {
            index = -1;
            Debug.Log("index not found");
        }
        return index;
    }

    /**
     * �Œ�I�u�W�F�N�g�������_���ɔz�u����
     */
    private void randomSettingBuilding(GameObject tile, int minimum, int maximum, BuildingObject buildingObj)
    {
        //�����̐������I�u�W�F�N�g(�I�u�W�F�N�g�̏W����)���쐬����
        for (int i = 0; i < Random.Range(minimum, maximum); i++)
        {
            var permitCounter = 0;
            int getIndex;
            do
            {
                createObjIndexList.Clear();
                getIndex = getRandomPositionForMap();
                permitCounter++;
            }
            while (!getRandomPositionBuilding(getIndex, buildingObj) && permitCounter < permitMaxSettingNum);
            //�C���f�b�N�X�����������ꍇ�ɃI�u�W�F�N�g��ݒu����
            if (permitCounter < permitMaxSettingNum)
            {
                addBuildingObject();
                Vector3 randomPosition = gridPositons[getIndex];
                Debug.Log("firstPosition" + randomPosition);
                Instantiate(tile, gridPositons[getIndex], Quaternion.identity);
            }
            createObjIndexList.Clear();
        }
    }

    /**
     * �����_���Ɏ擾�����_���g�p�\�����肷��
     */
    private bool getRandomPositionBuilding(int randomIndex,BuildingObject buildObj)
    {
        int remainderIndex = randomIndex%(nextLineValue);
        //Debug.Log("randomindex" + randomIndex);
        Debug.Log("startcheck");
        //�Œ�I�u�W�F�N�g��ݒu����X�y�[�X�����݂��邩�`�F�b�N
        //������
        if (remainderIndex - buildObj.width < 0)
        {
            //Debug.Log("remainder"+remainderIndex);
            //Debug.Log("buildobjwidth"+buildObj.width);
            //Debug.Log("rightcheckerror");
            return false;
        }
        //�E����
        if (remainderIndex + buildObj.width > (nextLineValue) - 1)
        {
            //Debug.Log("leftcheckerror");
            return false;
        }
        //������
        if (randomIndex + (buildObj.height * (rows - 2)) > gridPositons.Count)
        {
            //Debug.Log("bottomcheckerror");
            return false;
        }
        //�����
        if (randomIndex - (buildObj.height * (rows - 2)) < 0)
        {
            //Debug.Log("topcheckerror");
            return false;
        }
        Debug.Log("finishrangecheck");
        //�X�y�[�X���m�ۂł��Ă���ꍇ�A���̃I�u�W�F�N�g�����ɐݒu����Ă��Ȃ����`�F�b�N
        int rangeNum = (1 + buildObj.height * 2) * (1 + buildObj.width * 2);
        //�������n�_�ɂ���
        int startPosition = randomIndex - (nextLineValue) * buildObj.height - buildObj.width;
        Debug.Log("startPos"+startPosition);
        for (int i=0;i<rangeNum;i++)
        {
            int checkIndex = startPosition;
            checkIndex += i;
            Debug.Log("checkindex:"+checkIndex);
            if (checkMapIndex(checkIndex) == -1)
            {
                return false;
            }
            //���Ɏg�p����Ă���ꍇ�͒T���I��
            if (!checkRandomListIndex(checkIndex))
            {
                return false;
            }
            createObjIndexList.Add(checkIndex);
            //��s���̒T�����I�������玟�̍s����T���J�n
            if (i%(nextLineValue) == (buildObj.width*2)+1)
            {
                Debug.Log("turningpoint");
                startPosition += rows - 2;
            }
        }
        return true;
    }

    /**
     * �Œ�I�u�W�F�N�g���ݒu�����_��mapListIndex�Ɋi�[
     */
    private void addBuildingObject()
    {
        for (int i=0;i< createObjIndexList.Count;i++)
        {
            mapListIndex.Add(createObjIndexList[i]);
        }
    }
}