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

    //�s�v�c�̃_���W�����n�}�b�v�p�̕����G���A�̃N���X
    public class DivisionAreaClass
    {
        public int areaValue;
        public Vector2 startPoint;
        public Vector2 endPoint;
        public Vector2 innerStartPoint;
        public Vector2 innerEndPoint;
    }

    //���E���ɑ΂��ĐL�тĂ���ʘH�̃N���X
    public class AisleClass
    {
        //�ʘH�̍��W��ێ����郊�X�g
        public List<Vector2> aisleList;
        //�ʘH�̏I�_��ێ����郊�X�g
        public List<Vector2> aisleEndPointList;
    }

    //MAP�쐬�p�̃p�����[�^
    [Header("�}�b�v�̍s��")]public int rows;
    [Header("�}�b�v�̗�")]public int columns;
    [Header("�I�u�W�F�N�g�̖��W��̃p�����[�^")] public int denceParam = 4;
    [Header("�t���A��؂�p�����[�^")] public float borderParam = 0.25f;
    [Header("�}�b�v�̕�����")] public int borderDivision = 3;
    [Header("���E��")] public int secondBorder = 8;

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

    //�A�C�e��(�C�x���g�p)
    [Header("�e�X�g�C�x���g�A�C�e��")] public GameObject eventTestItem;

    //�v���C���[
    [Header("�v���C���[")] public GameObject player;

    //�����n�I�u�W�F�N�g
    [Header("��(�Œ�I�u�W�F�N�g)")] public GameObject castleObj;
    [Header("����1")] public GameObject buildingObj;

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
    [Header("���ԂɂȂ�NPC2")] public GameObject fellowTestNpc2;
    [Header("�C�x���g�pNPC(�[�����o��)")] public GameObject eventNpcAppearanceSkelton;
    [Header("�C�x���g�pNPC(�S�[�X�g���o��)")] public GameObject eventNpcAppearanceGhost;

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

    //�}�b�v�̐������[�h
    [HideInInspector] public int createMapMode = 2;

    //�s�v�c�̃_���W�����n�}�b�v�̍쐬�p�̃p�����[�^
    private int randomMapWidth = Define.MYSTERYMAP_WHITH;
    private int randomMapHeight = Define.MYSTERYMAP_HEIGHT;

    //�s�v�c�̃_���W�����n�}�b�v�p�t���A
    //�O��
    [Header("�O��(�s�v�c�̃_���W�����n)")] public GameObject labyrinthOuterWall;
    [Header("�O��2(�s�v�c�̃_���W�����n)")] public GameObject labyrinthOuterWall2;
    [Header("�O��3(�s�v�c�̃_���W�����n)")] public GameObject labyrinthOuterWall3;
    [Header("�O��5(�s�v�c�̃_���W�����n)")] public GameObject labyrinthOuterWall5;
    //�t���A
    //����
    [Header("�����t���A(�s�v�c�̃_���W�����n)")] public GameObject labyrinthGrassFloor;
    //��
    [Header("�΃t���A")] public GameObject labyrinthStoneFloor;
    //�K�i
    [Header("�V�[���؂�ւ��p�̊K�i")] public GameObject stairs;

    //�}�b�v�쐬�p
    enum Direction:int
    {
        Top,
        Right,
        Left,
        Bottom
    }

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
                GameObject instance = Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

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
                            //�ړ��s�n�_���X�g�ɍ��W��ǉ�
                            GManager.instance.unmovableList.Add(new Vector2(x, y));
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
                        //�ړ��s�n�_���X�g�ɍ��W��ǉ�
                        GManager.instance.unmovableList.Add(new Vector2(x, y));
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
                if (x == 1 && y == 1)
                {
                    continue;
                }
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
    private void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum, bool isUnmovable)
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
            if (gridPositons.Count < 1)
            {
                return;
            }
            //���݃I�u�W�F�N�g���u����Ă��Ȃ��A�����_���Ȉʒu���擾
            Vector3 randomPosition = RandomPosition();
            if (tile.name == "Stairs") Debug.Log("�K�i�̍��W" + randomPosition);
            //����
            Instantiate(tile, randomPosition, Quaternion.identity);
            //�ړ��s�̏ꍇ�͈ړ��s�n�_���X�g�ɍ��W��ǉ�
            if (isUnmovable)
            {
                GManager.instance.unmovableList.Add((Vector2)randomPosition);
            }
        }
    }

    /**
     * �v���C���[�����NPC��z�u����
     */
    private void LayoutPlayerAtRandom()
    {
        //randomIndex��錾���āAgridPositions�̐����琔�l�������_���œ����
        int randomIndex = Random.Range(0, gridPositons.Count);
        //randomPosition��錾���āAgridPositions��randomIndex�ɐݒ肷��
        Vector3 randomPosition = gridPositons[randomIndex];
        //�g�p����gridPositions�̗v�f���폜
        gridPositons.RemoveAt(randomIndex);
        Instantiate(player, randomPosition, Quaternion.identity);
        //NPC�p�̍��W���X�g
        List<Vector3> npcPointList = new List<Vector3>();
        //���Ԃ�NPC�����݂��Ȃ��ꍇ
        if (GManager.instance.fellows.Count < 1)
        {
            return;
        }
        //NPC�p�̍��W���X�g���쐬
        for (int i=0;i<8;i++)
        {
            float phase = (float)(2 * Mathf.PI * (i/8.0));
            float xValue = Mathf.Cos(phase);
            float yValue = Mathf.Sin(phase);
            if (i%2 != 0)
            {
                xValue = xValue == 0 ? 0 : xValue < 0 ? -1 : 1;
                yValue = yValue == 0 ? 0 : yValue < 0 ? -1 : 1;
            }
            npcPointList.Add(new Vector3(randomPosition.x + (int)xValue, randomPosition.y + (int)yValue, 0));
            Debug.Log("npcPointList"+ npcPointList[i]);
        }
        List<int> npcNextPositionList = new List<int>();
        //gridPositions�̃C���f�b�N�X���擾
        for (int i=0;i<npcPointList.Count;i++)
        {
            for (int j=0;j<gridPositons.Count;j++)
            {
                //NPC�̏����ʒu��₪�ݒu�\���W�̃��X�g�ɑ��݂���ꍇ
                if (npcPointList[i] == gridPositons[j])
                {
                    npcNextPositionList.Add(j);
                    Debug.Log("gridPositons[j]"+ gridPositons[j]+ ":"+j);
                    break;
                }
            }
        }
        //�~���\�[�g
        npcNextPositionList.Sort();
        npcNextPositionList.Reverse();
        //NPC�̏����ʒu�ݒ�
        for (int i=0;i<GManager.instance.fellows.Count;i++)
        {
            //NPC�̐����z�u�\���W�̐��𒴂��Ă���ꍇ�͎��̃V�[���Ɉ����p���Ȃ�
            if (i > npcNextPositionList.Count)
            {
                Destroy(GManager.instance.fellows[i]);
                continue;
            }
            //DontDestroyOnLoad����NPC�̈ʒu�ݒ�
            Debug.Log("npcNextPositionList[i]"+ npcNextPositionList[i]);
            GManager.instance.fellows[i].transform.position = gridPositons[npcNextPositionList[i]];
            gridPositons.RemoveAt(npcNextPositionList[i]);
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
        //���X�N���[���}�b�v���[�h
        if (createMapMode == 1)
        {
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
        //�s�v�c�̃_���W�����n�}�b�v���[�h
        else
        {
            //�V�[���ǂݍ��ݎ���grid�̒��g���N���A����
            gridPositons.Clear();
            //�}�b�v�I�u�W�F�N�g�ݒu�p�̔z��
            int[,] createMapArray = new int[randomMapHeight+1, randomMapWidth + 1];
            //�}�b�v�𕪊����ă��X�g�Ɋi�[����
            List<DivisionAreaClass> divAreaList = splitMap();
            Debug.Log("divarealistcount:" + divAreaList.Count);
            //�������ꂽ�����k������
            reduceDivisionArea(divAreaList);
            Debug.Log("----------------------------------------------------------------");
            for (int i=0;i<divAreaList.Count;i++)
            {
                Debug.Log("startpoint"+ divAreaList[i].startPoint);
                Debug.Log("endpoint" + divAreaList[i].endPoint);
                Debug.Log("innerstartpoint" + divAreaList[i].innerStartPoint);
                Debug.Log("innerendpoint" + divAreaList[i].innerEndPoint);
                Debug.Log("area" + divAreaList[i].areaValue);
            }
            //���E���ɑ΂��ĒʘH��L�΂�
            AisleClass aisleInfo = extendAisle(divAreaList);
            //�z��Ƀ}�b�v���(�ړ��\�G���A�A�O��)���i�[����
            pushMapInfo(divAreaList, createMapArray);
            //�z��Ƀ}�b�v���(�ʘH)���i�[����
            pushMapInfoAisle(aisleInfo.aisleList, createMapArray);
            //�ʘH�ƒʘH���Ȃ�
            List<Vector2> hookAisleInfo = hookAisle(divAreaList, aisleInfo.aisleEndPointList);
            //�z��Ƀ}�b�v���(�ڑ��p�̒ʘH)���i�[����
            pushMapInfoAisle(hookAisleInfo, createMapArray);
            //Instantiate(player, new Vector3(1, 1, 0f), Quaternion.identity);
            //�}�b�v�������ɃI�u�W�F�N�g���Z�b�g����
            createLabyrinthMap(createMapArray);
            //�Q�[���I�u�W�F�N�g���}�b�v�ɔz�u����
            layoutObject();
        }
    }

    /**
     * �}�b�v����
     */
    private List<DivisionAreaClass> splitMap()
    {
        int maxArea = randomMapWidth * randomMapHeight;
        int startXPoint = 0;
        int endXPoint = randomMapWidth;
        int startYPoint = 0;
        int endYPoint = randomMapHeight;
        List<DivisionAreaClass> divAreaList = new List<DivisionAreaClass>();
        bool splitAreaFlg = false;
        int splitResetCounter = 0;
        //�G���A���������擾
        int splitAreaNum = Random.Range(Define.AREA_MIN_NUM,Define.AREA_MAX_NUM + 1);
        while (divAreaList.Count < splitAreaNum)
        {
            //�ʐς��ő�̃G���A�����X�g����폜����
            if (splitAreaFlg)
            {
                divAreaList.RemoveAt(0);
            }
            int xDirectionPoint = endXPoint;
            int yDirectionPoint = endYPoint;
            int secondStartXPoint;
            int secondStartYPoint;
            int firstLength;
            int secondLength;
            //y�������ɕ���
            if (endXPoint > endYPoint)
            {
                xDirectionPoint = Random.Range(startXPoint,endXPoint);
                secondStartXPoint = xDirectionPoint + 1;
                secondStartYPoint = endYPoint - yDirectionPoint;
                firstLength = endXPoint - xDirectionPoint;
                secondLength = xDirectionPoint - startXPoint;
            }
            //x�������ɕ���
            else
            {
                yDirectionPoint = Random.Range(startYPoint,endYPoint);
                secondStartXPoint = endXPoint - xDirectionPoint;
                secondStartYPoint = yDirectionPoint + 1;
                firstLength = endYPoint - yDirectionPoint;
                secondLength = yDirectionPoint - startYPoint;
            }

            Debug.Log("xDirectionPoint:"+ xDirectionPoint);
            Debug.Log("yDirectionPoint:" + yDirectionPoint);

            //�G���A��������蒼��
            if (firstLength < 5 || secondLength < 5)
            {
                splitAreaFlg = false;
                splitResetCounter++;
                //�����̂�蒼�������񐔂𒴂����ꍇ�̓G���A�������Z�b�g���ĕ������ŏ������蒼��
                if (splitResetCounter > Define.AREA_SPLIT_MAXNUM)
                {
                    splitResetCounter = 0;
                    //���������G���A�����Z�b�g���ď����l��ݒ�
                    divAreaList.Clear();
                    maxArea = randomMapWidth * randomMapHeight;
                    startXPoint = 0;
                    endXPoint = randomMapWidth;
                    startYPoint = 0;
                    endYPoint = randomMapHeight;
                    //�����������炷
                    splitAreaNum--;
                    Debug.Log("resetsplit");
                }
                continue;
            }

            //���������G���A�̏����Z�b�g(���)
            int firstArea = (xDirectionPoint-startXPoint) * (yDirectionPoint-startYPoint);
            DivisionAreaClass firstDivArea = new DivisionAreaClass();
            //�ʐ�
            firstDivArea.areaValue = firstArea;
            //�[�_
            firstDivArea.startPoint = new Vector2(startXPoint,startYPoint);
            firstDivArea.endPoint = new Vector2(xDirectionPoint, yDirectionPoint);
            //���������G���A�̏����Z�b�g(���)
            DivisionAreaClass secondDivArea = new DivisionAreaClass();
            //�ʐ�
            secondDivArea.areaValue = maxArea - firstArea;
            //�[�_
            secondDivArea.startPoint = new Vector2(secondStartXPoint, secondStartYPoint);
            secondDivArea.endPoint = new Vector2(endXPoint,endYPoint);
            divAreaList.Add(firstDivArea);
            divAreaList.Add(secondDivArea);
            //�ʐςō~���ɕ��ёւ���
            divAreaList.Sort((a, b) => b.areaValue - a.areaValue);
            //�ʐς��ő�̃G���A�����Ɏ��̃G���A�������s��
            maxArea = divAreaList[0].areaValue;
            startXPoint = (int)divAreaList[0].startPoint.x;
            startYPoint = (int)divAreaList[0].startPoint.y;
            endXPoint = (int)divAreaList[0].endPoint.x;
            endYPoint = (int)divAreaList[0].endPoint.y;
            splitAreaFlg = true;
            //�ŏ��̖ʐς����l����������ꍇ�͕������I������
            if (divAreaList[divAreaList.Count - 1].areaValue < Define.MIN_AREA)
            {
                break;
            }
        }
        return divAreaList;
    }

    /**
     * �������������k������
     */
    private void reduceDivisionArea(List<DivisionAreaClass> divAreaList)
    {
        int randomInnerXPoint;
        int randomInnerYPoint;
        for (int i=0;i<divAreaList.Count;i++)
        {
            for (int j=0;j<2;j++)
            {
                randomInnerXPoint = Random.Range((int)(1 + divAreaList[i].endPoint.x - divAreaList[i].startPoint.x) / 4, 1 + (int)(divAreaList[i].endPoint.x - divAreaList[i].startPoint.x) / 3);
                randomInnerYPoint = Random.Range((int)(1 + divAreaList[i].endPoint.y - divAreaList[i].startPoint.y) / 4, 1 + (int)(divAreaList[i].endPoint.y - divAreaList[i].startPoint.y) / 3);
                //startpoint(�����̓_)�̐ݒ�
                if (j == 0)
                {
                    divAreaList[i].innerStartPoint = new Vector2(divAreaList[i].startPoint.x + randomInnerXPoint, divAreaList[i].startPoint.y + randomInnerYPoint);
                }
                //endpoint(�E���̓_)�̐ݒ�
                else
                {
                    divAreaList[i].innerEndPoint = new Vector2(divAreaList[i].endPoint.x - randomInnerXPoint, divAreaList[i].endPoint.y - randomInnerYPoint);
                }
            }
        }
    }

    /**
     * ���E���ɑ΂��ĒʘH��L�΂�
     */
    private AisleClass extendAisle(List<DivisionAreaClass> divAreaList)
    {
        AisleClass aisle = new AisleClass();
        List<Vector2> aisleList = new List<Vector2>();
        List<Vector2> aisleEndList = new List<Vector2>();
        int randomPoint;
        int extendNum;
        List<int> randomPointList = new List<int>();
        int xLineLength;
        int yLineLength;
        int extendCount = 0;
        for (int i=0;i<divAreaList.Count;i++)
        {
            xLineLength = (int)(divAreaList[i].innerEndPoint.x - divAreaList[i].innerStartPoint.x);
            yLineLength = (int)(divAreaList[i].innerEndPoint.y - divAreaList[i].innerStartPoint.y);
            //������
            if (divAreaList[i].startPoint.x != 0)
            {
                extendNum = yLineLength < 10 ? 1 : Random.Range(1, 3);
                while (extendCount < extendNum)
                {
                    extendCount++;
                    randomPoint = (int)Random.Range(divAreaList[i].innerStartPoint.y, divAreaList[i].innerEndPoint.y + 1);
                    if (randomPointList.Contains(randomPoint))
                    {
                        continue;
                    }
                    for (int k = (int)divAreaList[i].innerStartPoint.x - 1; k >= divAreaList[i].startPoint.x; k--)
                    {
                        aisleList.Add(new Vector2(k, randomPoint));
                        //�ʘH�̏I�_�����X�g�ɒǉ�
                        if (k == divAreaList[i].startPoint.x)
                        {
                            aisleEndList.Add(new Vector2(k, randomPoint));
                        }
                    }
                    randomPointList.Add(randomPoint);
                }
                randomPointList.Clear();
                extendCount = 0;
            }

            //������
            if (divAreaList[i].startPoint.y != 0)
            {
                extendNum = xLineLength < 10 ? 1 : Random.Range(1, 3);
                while (extendCount < extendNum)
                {
                    extendCount++;
                    randomPoint = (int)Random.Range(divAreaList[i].innerStartPoint.x, divAreaList[i].innerEndPoint.x + 1);
                    if (randomPointList.Contains(randomPoint))
                    {
                        continue;
                    }
                    for (int k = (int)divAreaList[i].innerStartPoint.y - 1; k >= divAreaList[i].startPoint.y; k--)
                    {
                        aisleList.Add(new Vector2(randomPoint, k));
                        //�ʘH�̏I�_�����X�g�ɒǉ�
                        if (k == divAreaList[i].startPoint.y)
                        {
                            aisleEndList.Add(new Vector2(randomPoint, k));
                        }
                    }
                    randomPointList.Add(randomPoint);
                }
                randomPointList.Clear();
                extendCount = 0;
            }

            //�E����
            if (divAreaList[i].endPoint.x != randomMapWidth)
            {
                extendNum = yLineLength < 10 ? 1 : Random.Range(1, 3);
                while (extendCount < extendNum)
                {
                    extendCount++;
                    randomPoint = (int)Random.Range(divAreaList[i].innerStartPoint.y, divAreaList[i].innerEndPoint.y + 1);
                    if (randomPointList.Contains(randomPoint))
                    {
                        continue;
                    }
                    for (int k = (int)divAreaList[i].innerEndPoint.x + 1; k <= divAreaList[i].endPoint.x; k++)
                    {
                        aisleList.Add(new Vector2(k, randomPoint));
                        //�ʘH�̏I�_�����X�g�ɒǉ�
                        if (k == divAreaList[i].endPoint.x)
                        {
                            aisleEndList.Add(new Vector2(k, randomPoint));
                        }
                    }
                    randomPointList.Add(randomPoint);
                }
                randomPointList.Clear();
                extendCount = 0;
            }

            //�����
            if (divAreaList[i].endPoint.y != randomMapHeight)
            {
                extendNum = xLineLength < 10 ? 1 : Random.Range(1, 3);
                while (extendCount < extendNum)
                {
                    extendCount++;
                    randomPoint = (int)Random.Range(divAreaList[i].innerStartPoint.x, divAreaList[i].innerEndPoint.x + 1);
                    if (randomPointList.Contains(randomPoint))
                    {
                        continue;
                    }
                    for (int k = (int)divAreaList[i].innerEndPoint.y + 1; k <= divAreaList[i].endPoint.y; k++)
                    {
                        aisleList.Add(new Vector2(randomPoint, k));
                        //�ʘH�̏I�_�����X�g�ɒǉ�
                        if (k == divAreaList[i].endPoint.y)
                        {
                            aisleEndList.Add(new Vector2(randomPoint, k));
                        }
                    }
                    randomPointList.Add(randomPoint);
                }
                randomPointList.Clear();
                extendCount = 0;
            }
        }
        aisle.aisleList = aisleList;
        aisle.aisleEndPointList = aisleEndList;
        Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        for (int i=0;i<aisleEndList.Count;i++)
        {
            Debug.Log("aisleEndlist"+aisleEndList[i]);
        }
        Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        return aisle;
    }

    /**
     * �z��Ƀ}�b�v���(�ړ��\�G���A�A�ړ��s�G���A)���i�[����
     */
    private void pushMapInfo(List<DivisionAreaClass> divAreaList,int[,] createMapArray)
    {
        for (int i=0;i<divAreaList.Count;i++)
        {
            for (int j=(int)divAreaList[i].startPoint.y ;j<=divAreaList[i].endPoint.y; j++)
            {
                for (int k=(int)divAreaList[i].startPoint.x; k<=divAreaList[i].endPoint.x; k++)
                {
                    //�ړ��\�G���A
                    if (j >= divAreaList[i].innerStartPoint.y && j <= divAreaList[i].innerEndPoint.y && k >= divAreaList[i].innerStartPoint.x && k <= divAreaList[i].innerEndPoint.x)
                    {
                        Debug.Log("j:" + j);
                        Debug.Log("k:" + k);
                        createMapArray[j,k] = Define.MOVABLE; 
                    }
                    //�ړ��s�G���A
                    else
                    {
                        Debug.Log("j:" + j);
                        Debug.Log("k:" + k);
                        createMapArray[j, k] = Define.WALL;
                    }
                }
            }
        }
    }

    /**
     * �z��Ƀ}�b�v���(�ʘH)���i�[����
     */
    private void pushMapInfoAisle(List<Vector2> aisleList, int[,] createMapArray)
    {
        for (int i=0;i<aisleList.Count;i++)
        {
            Debug.Log("aisleList[i].x"+ aisleList[i].x);
            Debug.Log("aisleList[i].y"+ aisleList[i].y);
            createMapArray[(int)aisleList[i].y, (int)aisleList[i].x] = Define.AISLE;
        }
    }

    /**
     * �ʘH�ƒʘH���Ȃ�
     */
    private List<Vector2> hookAisle(List<DivisionAreaClass> divAreaList, List<Vector2> aisleEndPointList)
    {
        List<Vector2> copyAisleEndPointList = new List<Vector2>(aisleEndPointList);
        List<Vector2> sameAxisList = new List<Vector2>();
        List<Vector2> hookAisleList = new List<Vector2>();
        int xLength;
        int yLength;
        for (int i=0;i<divAreaList.Count;i++)
        {
            xLength = (int)(divAreaList[i].endPoint.x - divAreaList[i].startPoint.x);
            yLength = (int)(divAreaList[i].endPoint.y - divAreaList[i].startPoint.y);
            //����
            if (divAreaList[i].startPoint.x != 0)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //x���̈�v����|�C���g�����X�g�ɒǉ�
                    if (copyAisleEndPointList[j].x == divAreaList[i].startPoint.x || copyAisleEndPointList[j].x == divAreaList[i].startPoint.x - 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //���X�g�̒��g��2�ȏ�̏ꍇ
                if (sameAxisList.Count > 1)
                {
                    //���X�g���\�[�g���čő�ƍŏ��Ԃ�ʘH�Ƃ���
                    sameAxisList.Sort((a, b) => (int)a.y - (int)b.y);
                    int minPoint = (int)sameAxisList[0].y;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].y;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //�ʘH�̍��W��ǉ�
                        hookAisleList.Add(new Vector2(divAreaList[i].startPoint.x, k));
                    }
                    //�ʘH�����яI�����_�̓��X�g����폜
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
            //����
            if (divAreaList[i].startPoint.y != 0)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //y���̈�v����|�C���g�����X�g�ɒǉ�
                    if (copyAisleEndPointList[j].y == divAreaList[i].startPoint.y || copyAisleEndPointList[j].y == divAreaList[i].startPoint.y - 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //���X�g�̒��g��2�ȏ�̏ꍇ
                if (sameAxisList.Count > 1)
                {
                    //���X�g���\�[�g���čő�ƍŏ��Ԃ�ʘH�Ƃ���
                    sameAxisList.Sort((a, b) => (int)a.x - (int)b.x);
                    int minPoint = (int)sameAxisList[0].x;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].x;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //�ʘH�̍��W��ǉ�
                        hookAisleList.Add(new Vector2(k, divAreaList[i].startPoint.y));
                    }
                    //�ʘH�����яI�����_�̓��X�g����폜
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
            //�E��
            if (divAreaList[i].endPoint.x != randomMapWidth)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //x���̈�v����|�C���g�����X�g�ɒǉ�
                    if (copyAisleEndPointList[j].x == divAreaList[i].endPoint.x || copyAisleEndPointList[j].x == divAreaList[i].endPoint.x + 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //���X�g�̒��g��2�ȏ�̏ꍇ
                if (sameAxisList.Count > 1)
                {
                    //���X�g���\�[�g���čő�ƍŏ��Ԃ�ʘH�Ƃ���
                    sameAxisList.Sort((a, b) => (int)a.y - (int)b.y);
                    int minPoint = (int)sameAxisList[0].y;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].y;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //�ʘH�̍��W��ǉ�
                        hookAisleList.Add(new Vector2(divAreaList[i].endPoint.x, k));
                    }
                    //�ʘH�����яI�����_�̓��X�g����폜
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
            //���
            if (divAreaList[i].endPoint.y != randomMapHeight)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //y���̈�v����|�C���g�����X�g�ɒǉ�
                    if (copyAisleEndPointList[j].y == divAreaList[i].endPoint.y || copyAisleEndPointList[j].y == divAreaList[i].endPoint.y + 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //���X�g�̒��g��2�ȏ�̏ꍇ
                if (sameAxisList.Count > 1)
                {
                    //���X�g���\�[�g���čő�ƍŏ��Ԃ�ʘH�Ƃ���
                    sameAxisList.Sort((a, b) => (int)a.x - (int)b.x);
                    int minPoint = (int)sameAxisList[0].x;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].x;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //�ʘH�̍��W��ǉ�
                        hookAisleList.Add(new Vector2(k, divAreaList[i].endPoint.y));
                    }
                    //�ʘH�����яI�����_�̓��X�g����폜
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
        }
        Debug.Log("copyAisleEndPointListcount"+ copyAisleEndPointList.Count);
        for (int i=0;i<copyAisleEndPointList.Count;i++)
        {
            Debug.Log("copyAisleEndPointList"+ copyAisleEndPointList[i]);
        }
        //�ڑ������ʘH�̍��W���X�g
        return hookAisleList;
    }

    /**
     * �z��̃}�b�v��񂩂�I�u�W�F�N�g���Z�b�g����
     */
    private void createLabyrinthMap(int[,] createMapArray)
    {
        //Debug.Log("width" + createMapArray.GetLength(0));
        //Debug.Log("height" + createMapArray.GetLength(1));
        for (int i=-1;i<=createMapArray.GetLength(0);i++)
        {
            for (int j=-1;j<=createMapArray.GetLength(1);j++)
            {
                Debug.Log("Vector2"+i+"  "+j);
                GameObject tile;
                //�}�b�v�[
                if (i == -1 || i == createMapArray.GetLength(0) || j == -1 || j == createMapArray.GetLength(1))
                {
                    Instantiate(labyrinthGrassFloor, new Vector3(j, i, 0), Quaternion.identity);
                    tile = labyrinthOuterWall5;
                    GameObject outerWallObj = Instantiate(tile, new Vector3(j, i, 0), Quaternion.identity) as GameObject;
                    //�}�b�v�[�̊O�ǂ͔j��s��
                    outerWallObj.GetComponent<OuterWallScript>().isIndestructible = true;
                    continue;
                }
                Debug.Log("createMapArray[i,j]:" + createMapArray[i, j]);
                switch (createMapArray[i, j])
                {
                    //�O��
                    case Define.WALL:
                        Instantiate(labyrinthGrassFloor, new Vector3(j, i, 0), Quaternion.identity);
                        tile = labyrinthOuterWall5;
                        break;
                    //�ړ��\�G���A
                    case Define.MOVABLE:
                        tile = labyrinthGrassFloor;
                        //�A�C�e���ƓG�̔z�u�p�ɍ��W��ۑ�����
                        gridPositons.Add(new Vector3(j, i, 0));
                        break;
                    //�ʘH
                    case Define.AISLE:
                        tile = labyrinthGrassFloor;
                        break;
                    default:
                        tile = labyrinthGrassFloor;
                        break;
                }
                Instantiate(tile, new Vector3(j,i,0), Quaternion.identity);
            }
        }
    }

    /**
     * �}�b�v��ɃI�u�W�F�N�g��z�u
     */
    private void layoutObject()
    {
        //�K�i���C���X�^���X��
        LayoutObjectAtRandom(stairs,1,1,false);
        //�v���C���[���C���X�^���X��
        //LayoutObjectAtRandom(player, 1, 1, false);
        LayoutPlayerAtRandom();
        //�H�ו����C���X�^���X���B
        LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum, false);
        //�G���C���X�^���X��
        //LayoutObjectAtRandom(enemy, 5, 5, false);
        //NPC(����)���C���X�^���X��
        //LayoutObjectAtRandom(fellowTestNpc, 2, 2, true);
        //NPC(����)���C���X�^���X��
        //LayoutObjectAtRandom(fellowTestNpc2, 2, 2, true);
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
        //�ΐ��i�s�Ɋ�Â��āA���݂̃��x�����Ɋ�Â��ēG�̐������肵�܂�
        int enemyCount = (int)Mathf.Log(2, 2f);
        ////Debug.Log(enemyCount);
        enemyCount = 2;

        //�I�u�W�F�N�g�ݒu���[������ɃI�u�W�F�N�g��ݒu
        //�����
        if (settingObjRule == Define.FIRST_SETTING)
        {
            //�H�ו����C���X�^���X���B
            LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum,false);
            //�|�[�V�������C���X�^���X��
            //LayoutObjectAtRandom(portion, portionCount.minmum, portionCount.maximum);
            //�󔠂��C���X�^���X��
            LayoutObjectAtRandom(treasure, portionCount.minmum, portionCount.maximum, true);
            //�J�o�����C���X�^���X���B
            LayoutObjectAtRandom(bag, 1, 1, false);
            //NPC���C���X�^���X��
            LayoutObjectAtRandom(testNpc, 1, 1, true);
            //NPC(�v���C���[��HP���񕜂���)���C���X�^���X��
            //LayoutObjectAtRandom(testRecoveryHpNpc, 1,1,true);
            //NPC(���򂠂�̃A�C�e���̈����n��)���C���X�^���X��
            LayoutObjectAtRandom(testGiveItemNpcBranchMessage,1,1, true);
            //NPC(��b����p)���C���X�^���X��
            LayoutObjectAtRandom(testBranchMessageNpc,1,1, true);
            //NPC(���)���C���X�^���X��
            LayoutObjectAtRandom(testSalesNpc,1,1, true);
            //NPC(����)���C���X�^���X��
            LayoutObjectAtRandom(fellowTestNpc, 2, 2, true);
            //NPC2(����)���C���X�^���X��
            LayoutObjectAtRandom(fellowTestNpc2, 1, 1, true);
            //�A�C�e����n���e�X�g�pNPC(���b�Z�[�W�\�����Ɏ����œn��)
            //LayoutObjectAtRandom(autoGiveItem,1,1);
            //�{���C���X�^���X��
            LayoutObjectAtRandom(bookDamageAllEnemy, 3,3, false);
            //�C�x���g�pNPC(�[�����o��)
            LayoutObjectAtRandom(eventNpcAppearanceSkelton, 1,1,true);
            //�C�x���g�pNPC(�S�[�X�g���o��)
            LayoutObjectAtRandom(eventNpcAppearanceGhost, 1, 1, true);
            //LayoutObjectAtRandom(enemy, enemyCount, enemyCount, false);
            //�C�x���g�p�A�C�e��
            //LayoutObjectAtRandom(eventTestItem, 1, 1, true);
        }
        //�����
        else if(settingObjRule == Define.SECOND_SETTING)
        {
            //�����C���X�^���X��
            LayoutObjectAtRandom(sword, swordCount.minmum, swordCount.maximum, false);
            //LayoutObjectAtRandom(enemy2, enemyCount - 1, enemyCount - 1, false);
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
        //int enemyCount = (int)Mathf.Log(2, 2f);
        ////Debug.Log(enemyCount);
        enemyCount = 2;
        //�����_�������ꂽ�ʒu�ŁA�ŏ��l�ƍő�l�Ɋ�Â��ă����_���Ȑ��̓G���C���X�^���X�����܂��B
        //LayoutObjectAtRandom(enemy, enemyCount, enemyCount, false);
        //LayoutObjectAtRandom(enemy2, enemyCount - 1, enemyCount - 1, false);

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