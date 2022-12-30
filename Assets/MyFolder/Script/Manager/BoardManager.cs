using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common;

//Map生成のscriptを記述する
public class BoardManager : MonoBehaviour
{
    //Map上にランダム生成するアイテム最小値、最大値を決めるclass
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

    //集合体オブジェクトを構成する要素数
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

    //固定オブジェクトのサイズ(高さ、幅)と生成方法
    public class BuildingObject
    {
        //上下幅
        public int height;
        //左右幅
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

    //フロアの属性
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

    //不思議のダンジョン系マップ用の分割エリアのクラス
    public class DivisionAreaClass
    {
        public int areaValue;
        public Vector2 startPoint;
        public Vector2 endPoint;
        public Vector2 innerStartPoint;
        public Vector2 innerEndPoint;
    }

    //境界線に対して伸びている通路のクラス
    public class AisleClass
    {
        //通路の座標を保持するリスト
        public List<Vector2> aisleList;
        //通路の終点を保持するリスト
        public List<Vector2> aisleEndPointList;
    }

    //MAP作成用のパラメータ
    [Header("マップの行数")]public int rows;
    [Header("マップの列数")]public int columns;
    [Header("オブジェクトの密集具合のパラメータ")] public int denceParam = 4;
    [Header("フロア区切りパラメータ")] public float borderParam = 0.25f;
    [Header("マップの分割数")] public int borderDivision = 3;
    [Header("境界線")] public int secondBorder = 8;

    //生成するアイテムの個数
    public Count Wallcount = new Count(3, 9);
    public Count foodcount = new Count(3, 5);
    public Count portionCount = new Count(2,3);
    public Count swordCount = new Count(3, 3);
    public Count shieldCount = new Count(3, 3);

    //境界線の乱数範囲
    public Count borderRandom = new Count(-2,2);

    //集合体生成時の構成要素数
    public RandomCreatePoint singleMountain = new RandomCreatePoint(5,8);
    public RandomCreatePoint singleForest = new RandomCreatePoint(6, 9);

    //MAPの材料
    //フロア
    [Header("出口")] public GameObject exit;
    [Header("草原フロア")] public GameObject grassFloor;
    [Header("土フロア")] public GameObject soilFloor;
    [Header("石フロア")] public GameObject stoneFloor;
    [Header("砂漠フロア")] public GameObject desertFloor;

    //マップ用オブジェクト(ステータス等に効果あり)
    [Header("土山")] public GameObject soilMountain;
    [Header("土山2")] public GameObject soilMountain2;
    [Header("岩山")] public GameObject StoneMountain;
    [Header("広葉樹")] public GameObject HardWood;
    [Header("針葉樹")] public GameObject Conifer;
    [Header("水場")] public GameObject WaterPlace;
    [Header("外壁")] public GameObject OuterWall;

    //マップ用オブジェクト(装飾用)
    [Header("石タイル")] public GameObject stoneTile;

    //固定オブジェクト
    public BuildingObject castle = new BuildingObject(9, 4, 1, 1);
    public BuildingObject building = new BuildingObject(4, 2, 1, 1);

    //エネミー
    [Header("敵")] public GameObject enemy;
    [Header("敵2")] public GameObject enemy2;

    //アイテム(宝箱)
    [Header("宝箱")] public GameObject treasure;

    //アイテム(消費系)
    [Header("食べ物")] public GameObject food;
    [Header("ポーション")] public GameObject portion;
    [Header("本(攻撃力上昇)")] public GameObject bookAttackUp;
    [Header("本(防御力上昇)")] public GameObject bookDefenceUp;
    [Header("本(全体攻撃)")] public GameObject bookDamageAllEnemy;

    //アイテム(装備系)
    [Header("武器")] public GameObject sword;
    [Header("盾")] public GameObject shield;

    //アイテム(バッグ)
    [Header("バッグ(所持数を増やす)")] public GameObject bag;

    //アイテム(イベント用)
    [Header("テストイベントアイテム")] public GameObject eventTestItem;

    //プレイヤー
    [Header("プレイヤー")] public GameObject player;

    //建物系オブジェクト
    [Header("城(固定オブジェクト)")] public GameObject castleObj;
    [Header("建物1")] public GameObject buildingObj;

    //区画ごとの属性
    [Header("草原区画の属性")] public floorAttribute grassDivisionAttribute;
    [Header("土区画の属性")] public floorAttribute soilDivisionAttribute;
    [Header("岩区画の属性")] public floorAttribute stoneDivisionAttribute;
    [Header("砂漠区画の属性")] public floorAttribute desertDivisionAttribute;

    //NPC
    [Header("テスト用NPC")] public GameObject testNpc;
    [Header("プレイヤーのHPを回復するテスト用NPC")] public GameObject testRecoveryHpNpc;
    [Header("アイテムを渡すテスト用NPC(分岐あり)")] public GameObject testGiveItemNpcBranchMessage;
    [Header("会話分岐テスト用NPC")] public GameObject testBranchMessageNpc;
    [Header("道具屋テスト用NPC")] public GameObject testSalesNpc;
    [Header("アイテムを渡すテスト用NPC(メッセージ表示中に自動で渡す)")] public GameObject autoGiveItem;
    [Header("仲間になるNPC")] public GameObject fellowTestNpc;
    [Header("仲間になるNPC2")] public GameObject fellowTestNpc2;
    [Header("イベント用NPC(骸骨が出現)")] public GameObject eventNpcAppearanceSkelton;
    [Header("イベント用NPC(ゴーストが出現)")] public GameObject eventNpcAppearanceGhost;

    //変換用
    private Transform boardHolder;

    //オブジェクト(集合体)設置時の乱数取得再試行の許容数
    private int permitMaxSettingNum = 5;
    //オブジェクト(単体)設置時の乱数取得再試行の許容数
    private int permitMaxSettingSingleNum;
    private int isTop = 1;
    private int isBottom = -1;
    private int isRight = 1;
    private int isLeft = -1;
    private bool verticalPlus = false;
    private int thirdBorder;
    private int nextLineValue;
    private int randomSettingObjectNum;
    //一区画分の横幅
    private int singleDivisionColumns;
   
    //リスト
    //objectがない場所を管理する
    private List<Vector3> gridPositons = new List<Vector3>();
    //マップ生成の際に取得したリストのインデックスを格納するリスト
    private List<int> mapListIndex = new List<int>();
    //固定オブジェクト設置時に一時的にインデックスを格納するリスト
    private List<int> createObjIndexList = new List<int>();
    //作成したマップオブジェクトを格納するリスト
    private List<floorAttribute> createMapTileList = new List<floorAttribute>();
    //マップオブジェクトを格納するリスト
    private List<floorAttribute> mapTileList = new List<floorAttribute>();
    //境界線を格納するリスト
    private List<int> mapBorderList = new List<int>();
    //抽選用のアイテムリスト
    List<GameObject> lotteryList = new List<GameObject>();
    List<GameObject> lotteryList2 = new List<GameObject>();

    //マップの生成モード
    [HideInInspector] public int createMapMode = 2;

    //不思議のダンジョン系マップの通常マップ作成用のパラメータ
    private int randomMapWidth = Define.MYSTERYMAP_WHITH;
    private int randomMapHeight = Define.MYSTERYMAP_HEIGHT;

    //不思議のダンジョン系マップのボス戦マップ作成用のパラメータ
    private int bossMapWidth = Define.MYSTERYBOSSMAP_WHITH;
    private int bossMapHeight = Define.MYSTERYBOSSMAP_HEIGHT;

    //不思議のダンジョン系マップ用フロア
    //外壁
    [Header("外壁(不思議のダンジョン系)")] public GameObject labyrinthOuterWall;
    [Header("外壁2(不思議のダンジョン系)")] public GameObject labyrinthOuterWall2;
    [Header("外壁3(不思議のダンジョン系)")] public GameObject labyrinthOuterWall3;
    [Header("外壁5(不思議のダンジョン系)")] public GameObject labyrinthOuterWall5;
    //フロア
    //草原
    [Header("草原フロア(不思議のダンジョン系)")] public GameObject labyrinthGrassFloor;
    //岩
    [Header("石フロア")] public GameObject labyrinthStoneFloor;
    //階段
    [Header("シーン切り替え用の階段")] public GameObject stairs;
    //タイル設置用のリスト
    [Header("タイル設置用のリスト(不思議のダンジョン系)")] public LabyrinthMapCreateMapClass labyrinthMapCreateMap;

    //不思議のダンジョン系マップ用フラグ
    [HideInInspector] public bool isBossMode;

    //不思議のダンジョン系マップ用gridPositons ※Dictionary型で保持
    Dictionary<int, List<Vector3>> gridPositionsDictionary;

    //プレイヤーのオブジェクト
    private GameObject playerObj;

    //アイテム等の複数設置するオブジェクト用のクラス
    [System.Serializable]
    public class MultipleSettingObjectClass
    {
        [Header("trueの場合オブジェクトを設置しない")] public bool noSettingFlg;
        [Header("trueの場合通常マップにオブジェクトを設置しない")] public bool noSettingNomalMap;
        [Header("設置するオブジェクト")] public GameObject multipleSettingObj;
        [Header("最小設置個数")] public int minSettingNumNomalMap;
        [Header("最大設置個数")] public int maxSettingNumNomalMap;
        [Header("trueの場合ボスマップにオブジェクトを配置しない")] public bool noSettingBossMap;
        [Header("最小設置個数(ボスマップ)")] public int minSettingNumBossMap;
        [Header("最大設置個数(ボスマップ)")] public int maxSettingNumBossMap;
    }

    //不思議のダンジョン系マップのタイル設置用クラス(階層ごとに設定するリスト形式)
    [System.Serializable]
    public class LabyrinthMapCreateMapListClass
    {
        [Header("フロアオブジェクト")] public GameObject floreObj;
        [Header("フロアオブジェクト(アクセント用)リスト")] public List<GameObject> subFloreObjList;
        [Header("効果付きフロアオブジェクト用リスト")] public List<GameObject> effectFloreObjList;
        [Header("外壁オブジェクト")] public GameObject wallObj;
        [Header("外壁オブジェクト(アクセント用)リスト")] public List<GameObject> subWallObjList;
        [Header("アイテムリスト")] public List<MultipleSettingObjectClass> itemObjList;
        [Header("敵リスト")] public List<MultipleSettingObjectClass> enemyObjList;
        [Header("トラップリスト")] public List<MultipleSettingObjectClass> trapObjList;
        [Header("Trueの場合は最後の階層でボス戦あり")] public bool isCreateBossMap;
        [Header("ボスモンスター")] public GameObject bossMonsterObj;
    }

    //LabyrinthMapCreateMapListClassのリストクラス(不思議のダンジョン系マップ)
    [System.Serializable]
    public class LabyrinthMapCreateMapClass
    {
        [Header("階層ごとのフロア、外壁等のオブジェクトを格納するリスト")] public List<LabyrinthMapCreateMapListClass> objList = new List<LabyrinthMapCreateMapListClass>();
        [Header("階層に関係なく設置するオブジェクトのリスト")] public List<MultipleSettingObjectClass> allFloorSettingObjList = new List<MultipleSettingObjectClass>();
    }

    //タイル設置およびモンスターハウス作成用クラス
    public class createMapArrayClass
    {
        public int? areaDivNum;
        public int tileType;
    }

    //プレイヤー設置時に使用するクラス
    public class SettingPlayerPositionClass
    {
        public int? nowPlyerPositionKey;
        public Vector3 randomPosition;
    }

    //アイテム等の設置時に使用するクラス
    public class SettingGameObjClass
    {
        public GameObject settingObj;
        public int minSetNum;
        public int maxSetNum;
    }


    //フィールド生成
    /**
     * y軸を基準にオブジェクトを設置する
     */
    void BoardSetup()
    {
        secondBorder = (int)(columns * borderParam) + Random.Range(1, 2);
        thirdBorder = secondBorder * 2 + Random.Range(1, 2);
        //Boardをインスタンス化してboardHolderに設定
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
                //床を設置してインスタンス化の準備
                GameObject toInsutantiate = grassFloor;

                if (x >= border)
                {
                    toInsutantiate = soilFloor;
                }
                if (x >= border2)
                {
                    toInsutantiate = stoneFloor;
                }

                //8×8マス外は外壁を設置してインスタンス化の準備
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInsutantiate = OuterWall;
                }

                //toInsutantiateに設定されたものをインスタンス化
                GameObject instance = Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                //インスタンス化された床or外壁の親要素をboardHolderに設定
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /**
     * y軸を基準にオブジェクトを設置する(境界線の判定をループで行う)
     */
    void BoardSetupRemodel()
    {
        GameObject toInsutantiate = null;
        for (int y = -1; y < rows + 1; y++)
        {
            for (int x = -1; x < columns + 1; x++)
            {
                //8×8マス外は外壁を設置してインスタンス化の準備
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

                //toInsutantiateに設定されたものをインスタンス化
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                //インスタンス化された床or外壁の親要素をboardHolderに設定
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /**
     * 横方向(x軸向き)にマップを生成する
     */
    private void boardSetUp2()
    {
        GameObject toInsutantiate = null;
        GameObject tempToInsutantiate = null;
        bool isRandomSet = false;
        //Boardをインスタンス化してboardHolderに設定
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            //区画ごとにフロアーを設置
            for (int i=0;i< borderDivision; i++)
            {
                //境界線上の場合は特殊処理
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
                //8×8マス外は外壁を設置してインスタンス化の準備
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInsutantiate = OuterWall;
                }
                //toInsutantiateに設定されたものをインスタンス化
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                //インスタンス化された床or外壁の親要素をboardHolderに設定
                instance.transform.SetParent(boardHolder);
                if (isRandomSet)
                {
                    int reSettingRange = Random.Range(1, 6);
                    for (int k = 1; k < reSettingRange; k++)
                    {
                        //toInsutantiateに設定されたものをインスタンス化
                        instance =
                            Instantiate(toInsutantiate, new Vector3(x - k, y, -1), Quaternion.identity) as GameObject;
                        //インスタンス化された床or外壁の親要素をboardHolderに設定
                        instance.transform.SetParent(boardHolder);
                    }
                }
            }
            isRandomSet = false;
        }
    }

    /**
     * 現状最も負荷が少ない？
     * 通常時はx軸向きにマップを生成し、特定のエリア内(境界線周り)のみy軸向きにマップを生成する
     */
    private void boardSetUp2Remodel()
    {
        GameObject toInsutantiate = null;
        GameObject tempToInsutantiate = null;
        GameObject tempNextToInsutantiate = null;
        bool isRandomSettingLoop = false;
        int randomAreaBorder = Random.Range(-5, -6);
        //Boardをインスタンス化してboardHolderに設定
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            //xが更新されるタイミングで設置するオブジェクトを判定する
            for (int i = 0; i < borderDivision; i++)
            {
                //xがランダムに設置するエリアに入った場合
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
            //ランダムに配置するエリア(y軸基準でx軸方向にループ)
            if (isRandomSettingLoop)
            {
                //y軸を固定
                for (int y=-1; y < rows + 1; y ++)
                {
                    //行ごとにフロアの切り替わりをランダムにする
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
                            //移動不可地点リストに座標を追加
                            GManager.instance.unmovableList.Add(new Vector2(x, y));
                            toInsutantiate = OuterWall;
                        }
                        //Debug.Log("randomsettingobj" + toInsutantiate);
                        //toInsutantiateに設定されたものをインスタンス化
                        GameObject instance =
                            Instantiate(toInsutantiate, new Vector3(randomAreaX, y, 0), Quaternion.identity) as GameObject;
                        //インスタンス化された床or外壁の親要素をboardHolderに設定
                        instance.transform.SetParent(boardHolder);
                    }
                }
                isRandomSettingLoop = false;
                x += (-1)*randomAreaBorder -1;
            }
            //通常のフロア配置
            else
            {
                for (int y = -1; y < rows + 1; y++)
                {
                    toInsutantiate = tempToInsutantiate;
                    //8×8マス外は外壁を設置してインスタンス化の準備
                    if (x == -1 || x == columns || y == -1 || y == rows)
                    {
                        //移動不可地点リストに座標を追加
                        GManager.instance.unmovableList.Add(new Vector2(x, y));
                        toInsutantiate = OuterWall;
                    }
                    //toInsutantiateに設定されたものをインスタンス化
                    GameObject instance =
                        Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    //インスタンス化された床or外壁の親要素をboardHolderに設定
                    instance.transform.SetParent(boardHolder);
                }
            }
        }
    }

    /**
     * フロアオブジェクトをリストに格納する(新規でオブジェクトを作成した場合に追加する)
     */
    private void inputFloorObj()
    {
        //フロアオブジェクトをリストに格納する
        createMapTileList.Add(grassDivisionAttribute);
        createMapTileList.Add(soilDivisionAttribute);
        //createMapTileList.Add(stoneDivisionAttribute);
        createMapTileList.Add(desertDivisionAttribute);
    }

    /**
     * フロアーオブジェクトをリストに格納する
     */
    private void createFloorTileList()
    {
        //フロアオブジェクトをリストに格納する
        inputFloorObj();
        for (int i=0;i< borderDivision;i++)
        {
            mapTileList.Add(createMapTileList[i% createMapTileList.Count]);
        }
        //mapTileList.Add(floor);
        //mapTileList.Add(floor2);
        //mapTileList.Add(floor3);
        //リストをシャッフル
        for (int i = mapTileList.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1); // ランダムで要素番号を１つ選ぶ（ランダム要素）
            floorAttribute temp = mapTileList[i]; // 一番最後の要素を仮確保（temp）にいれる
            mapTileList[i] = mapTileList[j]; // ランダム要素を一番最後にいれる
            mapTileList[j] = temp; // 仮確保を元ランダム要素に上書き
        }
        //フロアの難易度順にソートする
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
     * フロアー設置用の境界線をリストに格納する
     */
    private void createBorderLineList()
    {
        singleDivisionColumns = columns / borderDivision;
        for (int i=0;i< borderDivision + 1; i++)
        {
            mapBorderList.Add(singleDivisionColumns * i);
        }
    }

    //gridPositionsをクリアする
    void InitialiseList(int startLine,int endLine)
    {
        //リストをクリアする
        gridPositons.Clear();
        for (int y = 1;y < rows -1; y++)
        {
            for (int x = startLine;x< endLine -1; x++)
            {
                if (x == 1 && y == 1)
                {
                    continue;
                }
                //gridPositionsにx,yの値をいれる
                gridPositons.Add(new Vector3(x, y, 0));
            }
        }
        Debug.Log("startline"+startLine);
        Debug.Log("endline"+endLine);
        Debug.Log("gridcountinit"+gridPositons.Count);
    }

    //gridPositionsからランダムな位置を取得する
    Vector3 RandomPosition()
    {
        //randomIndexを宣言して、gridPositionsの数から数値をランダムで入れる
        int randomIndex = Random.Range(0, gridPositons.Count);

        //randomPositionを宣言して、gridPositionsのrandomIndexに設定する
        Vector3 randomPosition = gridPositons[randomIndex];
        Debug.Log("gridpositioncount"+gridPositons.Count);
        Debug.Log("removeatindex"+randomIndex);
        //使用したgridPositionsの要素を削除
        gridPositons.RemoveAt(randomIndex);
        Debug.Log("gridpositioncount" + gridPositons.Count);
        return randomPosition;
    }

    //Mapにランダムで引数のものを配置する(敵、壁、アイテム)
    private void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum, bool isUnmovable)
    {
        //tileがセットされていない場合はreturn
        if (tile == null)
        {
            return;
        }
        //生成するアイテムの個数を最小最大値からランダムに決め、objectCountに設定する
        int objectCount = Random.Range(minimum, maximum + 1);
        //割り当て用の座標が余っている場合のみ配置可能
        //設置するオブジェクトの数分ループで回す
        for (int i = 0; i < objectCount; i++)
        {
            if (gridPositons.Count < 1)
            {
                return;
            }
            //現在オブジェクトが置かれていない、ランダムな位置を取得
            Vector3 randomPosition = RandomPosition();
            if (tile.name == "Stairs") Debug.Log("階段の座標" + randomPosition);
            //生成
            Instantiate(tile, randomPosition, Quaternion.identity);
            //移動不可の場合は移動不可地点リストに座標を追加
            if (isUnmovable)
            {
                GManager.instance.unmovableList.Add((Vector2)randomPosition);
            }
        }
    }


    /**
     * 不思議のダンジョン系マップにオブジェクトを設置する
     */
    private void LayoutObjectAtRandomMysterymap(GameObject tile, int minimum, int maximum, bool isUnmovable,int? argDictionaryKey)
    {
        //オブジェクトがセットされていない場合
        if (tile == null)
        {
            return;
        }
        int? dictionaryKey;
        //生成するアイテムの個数を最小最大値からランダムに決め、objectCountに設定する
        int objectCount = Random.Range(minimum, maximum + 1);
        //割り当て用の座標が余っている場合のみ配置可能
        //設置するオブジェクトの数分ループで回す
        for (int i = 0; i < objectCount; i++)
        {
            //オブジェクトを配置する座標がなくなった場合
            if (gridPositionsDictionary.Count < 1)
            {
                return;
            }
            //引数でキーを渡されている場合はそれをセット
            //渡されていない場合はランダムに取得する
            dictionaryKey = argDictionaryKey == null ? getRandomDictionaryKey() : argDictionaryKey;
            //キーが存在しない場合
            if (dictionaryKey == null)
            {
                continue;
            }
            //現在オブジェクトが置かれていない、ランダムな位置を取得
            Vector3? randomPosition = getRandomPositionFromMysterymap((int)dictionaryKey);
            if (randomPosition == null)
            {
                continue;
            }
            if (tile.name == "Stairs") Debug.Log("階段の座標" + randomPosition);
            //生成
            Instantiate(tile, (Vector3)randomPosition, Quaternion.identity);
            //移動不可の場合は移動不可地点リストに座標を追加
            if (isUnmovable)
            {
                GManager.instance.unmovableList.Add((Vector2)randomPosition);
            }
        }
    }

    //gridPositionsDictionaryからランダムな位置を取得する
    Vector3? getRandomPositionFromMysterymap(int dictionaryKey)
    {
        //キーに対応したリストからランダムに座標を取得する
        Vector3 randomPosition = getPositionFromDictionary((int)dictionaryKey,out int randomPositionIndex);

        //使用した座標を削除する
        deleteUsedPosition((int)dictionaryKey, randomPositionIndex);
        return randomPosition;
    }

    /**
     * gridPositionsDictionaryからランダムにキーを取得する
     */
    private int? getRandomDictionaryKey()
    {
        List<int> keyList = gridPositionsDictionary.Keys.ToList();
        int randomDictionaryKey = Random.Range(0, keyList.Count);
        int dictionaryKey = keyList[randomDictionaryKey];
        //キーが存在しない場合
        if (!gridPositionsDictionary.ContainsKey(dictionaryKey))
        {
            return null;
        }
        return dictionaryKey;
    }

    /**
     * Dictionaryのキーから座標を返却
     */
    private Vector3 getPositionFromDictionary(int dictionaryKey,out int randomPositionIndex)
    {
        //キーに対応したリスト
        List<Vector3> pickedAreaPositionList = gridPositionsDictionary[dictionaryKey];
        //リストからランダムに座標を取得する
        randomPositionIndex = Random.Range(0, pickedAreaPositionList.Count);
        return pickedAreaPositionList[randomPositionIndex];
    }

    /**
     * オブジェクトを配置した座標をリストから削除する
     */
    private void deleteUsedPosition(int dictionaryKey,int randomPositionIndex)
    {
        gridPositionsDictionary[(int)dictionaryKey].RemoveAt(randomPositionIndex);
        //座標リストが空になった場合はDictionaryから削除する
        if (gridPositionsDictionary[(int)dictionaryKey].Count < 1)
        {
            gridPositionsDictionary.Remove((int)dictionaryKey);
        }
    }

    /**
     * プレイヤーを設置
     */
    private void LayoutPlayerAtRandom(SettingPlayerPositionClass settingPlayerInfo)
    {
        //gridPositionsDictionaryからランダムにキーを取得する
        settingPlayerInfo.nowPlyerPositionKey = getRandomDictionaryKey();
        //キーが存在しない場合
        if (settingPlayerInfo.nowPlyerPositionKey == null)
        {
            return;
        }
        //座標を取得
        settingPlayerInfo.randomPosition = getPositionFromDictionary((int)settingPlayerInfo.nowPlyerPositionKey, out int randomPositionIndex);
        playerObj = Instantiate(player, settingPlayerInfo.randomPosition, Quaternion.identity) as GameObject;
        deleteUsedPosition((int)settingPlayerInfo.nowPlyerPositionKey, randomPositionIndex);
    }

    /**
     * モンスターハウスの作成
     */
    private void createMonsterHouse(int nowPlyerPositionKey,int hierarchyIndex)
    {
        //敵の出現数
        int appearEnemyNum = Random.Range(gridPositionsDictionary[nowPlyerPositionKey].Count / 6, (gridPositionsDictionary[nowPlyerPositionKey].Count / 5)+1);
        int startAppearNum = 1;
        int randomAppearNum;
        //敵のリスト
        List<MultipleSettingObjectClass> multiEnemyObjList = labyrinthMapCreateMap.objList[hierarchyIndex].enemyObjList.FindAll(obj => obj.multipleSettingObj != null);
        for (int i=0;i< multiEnemyObjList.Count;i++)
        {
            randomAppearNum = Random.Range(startAppearNum, appearEnemyNum + 1);
            if (i == multiEnemyObjList.Count -1)
            {
                randomAppearNum = appearEnemyNum + 1 - startAppearNum;
            }
            LayoutObjectAtRandomMysterymap(multiEnemyObjList[i].multipleSettingObj, randomAppearNum, randomAppearNum, false, nowPlyerPositionKey);
            startAppearNum = randomAppearNum + 1;
            if (startAppearNum == appearEnemyNum + 1)
            {
                break;
            }
        }
        LogMessageManager.wrightLog(MessageManager.createMessage("8"));
    }

    /**
     * NPCをプレイヤーの周囲に配置
     */
    private void LayoutNpcAtRandom(Vector3 randomPosition,int nowPlyerPositionKey)
    {
        //Debug.Log("(Vector3)randomPosition" + (Vector3)randomPosition);
        //NPC用の座標リスト
        List<Vector3> npcPointList = new List<Vector3>();
        //NPC用の座標リストを作成(プレイヤーの周囲8マス)
        for (int i = 0; i < 8; i++)
        {
            float phase = (float)(2 * Mathf.PI * (i / 8.0));
            float xValue = Mathf.Cos(phase);
            float yValue = Mathf.Sin(phase);
            if (i % 2 != 0)
            {
                xValue = xValue == 0 ? 0 : xValue < 0 ? -1 : 1;
                yValue = yValue == 0 ? 0 : yValue < 0 ? -1 : 1;
            }
            //Debug.Log("((Vector3)randomPosition).x + (int)xValue" + ((randomPosition).x + (int)xValue));
            //Debug.Log("((Vector3)randomPosition).y + (int)yValue" + ((randomPosition).y + (int)yValue));
            npcPointList.Add(new Vector3((int)randomPosition.x + (int)xValue, (int)randomPosition.y + (int)yValue, 0));
        }
        //int keyInteger = nowPlyerPositionKey;
        List<Vector3> keyGridPositionList = gridPositionsDictionary[nowPlyerPositionKey];
        //gridPositionsDictionary[(int)nowPlyerPositionKey]のインデックスを取得
        List<int> npcNextPositionList = new List<int>();
        for (int i = 0; i < npcPointList.Count; i++)
        {
            for (int j = 0; j < keyGridPositionList.Count; j++)
            {
                //NPCの初期位置候補が設置可能座標のリストに存在する場合
                if (npcPointList[i] == keyGridPositionList[j])
                {
                    npcNextPositionList.Add(j);
                    break;
                }
            }
        }
        //降順ソート
        npcNextPositionList.Sort();
        npcNextPositionList.Reverse();
        //NPCの初期位置設定
        for (int i = 0; i < GManager.instance.fellows.Count; i++)
        {
            //NPCの数が配置可能座標の数を超えている場合は次のシーンに引き継げない
            if (i > npcNextPositionList.Count)
            {
                Destroy(GManager.instance.fellows[i]);
                continue;
            }
            //DontDestroyOnLoad内のNPCの位置設定
            //Debug.Log("npcNextPositionList[i]" + npcNextPositionList[i]);
            //Debug.Log("gridPositionsDictionary[(int)nowPlyerPositionKey][npcNextPositionList[i]]" + gridPositionsDictionary[nowPlyerPositionKey][npcNextPositionList[i]]);
            GManager.instance.fellows[i].transform.position = keyGridPositionList[npcNextPositionList[i]];
            //Debug.Log("GManager.instance.fellows[i].transform.position" + GManager.instance.fellows[i].transform.position);
            gridPositionsDictionary[nowPlyerPositionKey].RemoveAt(npcNextPositionList[i]);
        }

        //NPC用の座標リスト
        //List<Vector3> npcPointList = new List<Vector3>();
        ////仲間のNPCが存在しない場合
        //if (GManager.instance.fellows.Count < 1)
        //{
        //    return;
        //}
        ////NPC用の座標リストを作成
        //for (int i=0;i<8;i++)
        //{
        //    float phase = (float)(2 * Mathf.PI * (i/8.0));
        //    float xValue = Mathf.Cos(phase);
        //    float yValue = Mathf.Sin(phase);
        //    if (i%2 != 0)
        //    {
        //        xValue = xValue == 0 ? 0 : xValue < 0 ? -1 : 1;
        //        yValue = yValue == 0 ? 0 : yValue < 0 ? -1 : 1;
        //    }
        //    npcPointList.Add(new Vector3(((Vector3)randomPosition).x + (int)xValue, ((Vector3)randomPosition).y + (int)yValue, 0));
        //    Debug.Log("npcPointList"+ npcPointList[i]);
        //}

        //List<int> npcNextPositionList = new List<int>();
        ////gridPositionsのインデックスを取得
        //for (int i=0;i<npcPointList.Count;i++)
        //{
        //    for (int j=0;j<gridPositons.Count;j++)
        //    {
        //        //NPCの初期位置候補が設置可能座標のリストに存在する場合
        //        if (npcPointList[i] == gridPositons[j])
        //        {
        //            npcNextPositionList.Add(j);
        //            break;
        //        }
        //    }
        //}
        ////降順ソート
        //npcNextPositionList.Sort();
        //npcNextPositionList.Reverse();
        ////NPCの初期位置設定
        //for (int i=0;i<GManager.instance.fellows.Count;i++)
        //{
        //    //NPCの数が配置可能座標の数を超えている場合は次のシーンに引き継げない
        //    if (i > npcNextPositionList.Count)
        //    {
        //        Destroy(GManager.instance.fellows[i]);
        //        continue;
        //    }
        //    //DontDestroyOnLoad内のNPCの位置設定
        //    Debug.Log("npcNextPositionList[i]"+ npcNextPositionList[i]);
        //    GManager.instance.fellows[i].transform.position = gridPositons[npcNextPositionList[i]];
        //    gridPositons.RemoveAt(npcNextPositionList[i]);
        //}
    }

    /**
     * 座標からDictionaryのキーを返却
     */
    private int? getGridPositionsDictionaryKey(Vector3 point)
    {
        foreach (KeyValuePair<int, List<Vector3>> element in gridPositionsDictionary)
        {
            foreach (Vector3 vector in element.Value)
            {
                //Debug.Log("vector"+ vector);
                //Debug.Log("point" + point);
                //座標が一致した場合キーを返却
                if (vector == point)
                {
                    return element.Key;
                }
            }
        }
        return null;
    }

    /**
     * ボスのHPが0以下になった際に階段を配置する
     */
    public void LayoutStairsAtRandom(Vector3 bossPosition)
    {
        Vector3 randomStairsPosition;
        do
        {
            int randomStairsIndex = Random.Range(0, gridPositionsDictionary[0].Count);
            randomStairsPosition = gridPositionsDictionary[0][randomStairsIndex];
            gridPositionsDictionary[0].RemoveAt(randomStairsIndex);
            //プレイヤーまたはボスの現在位置と一致した場合再度位置を取得し直す
        } while (randomStairsPosition == playerObj.transform.position || bossPosition == randomStairsPosition);
        Instantiate(stairs, randomStairsPosition, Quaternion.identity);
    }

    /**
     * 区画ごとにオブジェクト(アイテム、敵など)を設置する
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
     * 宝箱から出現するアイテムのリストを作成
     */
    private void addTreasureItemList()
    {
        GManager.instance.treasureItemList.Add(food.GetComponent<Item>());
    }

    /**
     * 宝箱から出現するアイテムのリストを作成
     */
    private void createTreasureItemList()
    {
        lotteryList.Add(portion);
        lotteryList2.Add(food);
        GManager.instance.lotteryitemList.Add(lotteryList);
        GManager.instance.lotteryitemList.Add(lotteryList2);
    }

    //ゲームボードをレイアウトする
    public void Init()
    {
        createTreasureItemList();
        //横スクロールマップモード
        if (createMapMode == 1)
        {
            createFloorTileList();
            createBorderLineList();
            //外壁と床を作成します
            boardSetUp2Remodel();
            ////プレイヤーを配置
            ////gridPositons.RemoveAt(0);
            //mapListIndex.Add(0);
            Instantiate(player, new Vector3(1, 1, 0f), Quaternion.identity);
            //区画ごとにオブジェクト(アイテム、敵など)を設置する
            settingGameObjectToDivisionArea();
        }
        //不思議のダンジョン系マップモード
        else
        {
            //シーン読み込み時にgridの中身をクリアする
            gridPositons.Clear();
            //シーン読み込み時にgridPositionsDictionaryの中身をクリアする
            gridPositionsDictionary = new Dictionary<int, List<Vector3>>();
            int quotientNowHierarchyLevel = (GManager.instance.hierarchyLevel - 1) / Define.MAPTILE_CHANGE_STAIRS;
            //リストの要素数を超えているかチェック
            bool isLessListCount = quotientNowHierarchyLevel < labyrinthMapCreateMap.objList.Count;
            //リストのインデックスを取得
            int hierarchyIndex = isLessListCount ? quotientNowHierarchyLevel : labyrinthMapCreateMap.objList.Count - 1;
            //マップレイアウト用の配列
            createMapArrayClass[,] createMapArray;
            //ボス戦用マップを作成するかの判定
            isBossMode = GManager.instance.hierarchyLevel % Define.MAPTILE_CHANGE_STAIRS == 0 &&
                         isLessListCount &&
                         labyrinthMapCreateMap.objList[quotientNowHierarchyLevel].isCreateBossMap &&
                         labyrinthMapCreateMap.objList[quotientNowHierarchyLevel].bossMonsterObj &&
                         labyrinthMapCreateMap.objList[quotientNowHierarchyLevel].bossMonsterObj?.GetComponent<Enemy>()?.enabled != null;
            //ボス戦マップ作成
            if (isBossMode)
            {
                //マップオブジェクト設置用の配列
                createMapArray = new createMapArrayClass[bossMapHeight + 1, bossMapWidth + 1];
                pushBossMapInfo(createMapArray);
            }
            //通常マップ作成
            else
            {
                //マップオブジェクト設置用の配列
                createMapArray = new createMapArrayClass[randomMapHeight + 1, randomMapWidth + 1];
                //マップを分割してリストに格納する
                List<DivisionAreaClass> divAreaList = splitMap();
                Debug.Log("divarealistcount:" + divAreaList.Count);
                //分割された区画を縮小する
                reduceDivisionArea(divAreaList);
                Debug.Log("----------------------------------------------------------------");
                for (int i = 0; i < divAreaList.Count; i++)
                {
                    Debug.Log("startpoint" + divAreaList[i].startPoint);
                    Debug.Log("endpoint" + divAreaList[i].endPoint);
                    Debug.Log("innerstartpoint" + divAreaList[i].innerStartPoint);
                    Debug.Log("innerendpoint" + divAreaList[i].innerEndPoint);
                    Debug.Log("area" + divAreaList[i].areaValue);
                }
                //境界線に対して通路を伸ばす
                AisleClass aisleInfo = extendAisle(divAreaList);
                //配列にマップ情報(移動可能エリア、外壁)を格納する
                pushMapInfo(divAreaList, createMapArray);
                //配列にマップ情報(通路)を格納する
                pushMapInfoAisle(aisleInfo.aisleList, createMapArray);
                //通路と通路をつなぐ
                List<Vector2> hookAisleInfo = hookAisle(divAreaList, aisleInfo.aisleEndPointList);
                //配列にマップ情報(接続用の通路)を格納する
                pushMapInfoAisle(hookAisleInfo, createMapArray);
            }
            //マップ情報を元にオブジェクト(タイル)をセットする
            createLabyrinthMap(createMapArray, hierarchyIndex);
            //ゲームオブジェクトをマップに配置する
            layoutObjectInLabyrinth(hierarchyIndex, isBossMode);
        }
    }

    /**
     * マップ分割
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
        //エリア分割数を取得
        int splitAreaNum = Random.Range(Define.AREA_MIN_NUM,Define.AREA_MAX_NUM + 1);
        while (divAreaList.Count < splitAreaNum)
        {
            //面積が最大のエリアをリストから削除する
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
            //y軸方向に分割
            if (endXPoint > endYPoint)
            {
                xDirectionPoint = Random.Range(startXPoint,endXPoint);
                secondStartXPoint = xDirectionPoint + 1;
                secondStartYPoint = endYPoint - yDirectionPoint;
                firstLength = endXPoint - xDirectionPoint;
                secondLength = xDirectionPoint - startXPoint;
            }
            //x軸方向に分割
            else
            {
                yDirectionPoint = Random.Range(startYPoint,endYPoint);
                secondStartXPoint = endXPoint - xDirectionPoint;
                secondStartYPoint = yDirectionPoint + 1;
                firstLength = endYPoint - yDirectionPoint;
                secondLength = yDirectionPoint - startYPoint;
            }

            //Debug.Log("xDirectionPoint:"+ xDirectionPoint);
            //Debug.Log("yDirectionPoint:" + yDirectionPoint);

            //エリア分割をやり直す
            if (firstLength <= 5 || secondLength <= 5)
            {
                splitAreaFlg = false;
                splitResetCounter++;
                //分割のやり直しが一定回数を超えた場合はエリア情報をリセットして分割を最初からやり直す
                if (splitResetCounter > Define.AREA_SPLIT_MAXNUM)
                {
                    splitResetCounter = 0;
                    //分割したエリアをリセットして初期値を設定
                    divAreaList.Clear();
                    maxArea = randomMapWidth * randomMapHeight;
                    startXPoint = 0;
                    endXPoint = randomMapWidth;
                    startYPoint = 0;
                    endYPoint = randomMapHeight;
                    //分割数を減らす
                    splitAreaNum--;
                    //Debug.Log("resetsplit");
                }
                continue;
            }

            //分割したエリアの情報をセット(一つ目)
            int firstArea = (xDirectionPoint-startXPoint) * (yDirectionPoint-startYPoint);
            DivisionAreaClass firstDivArea = new DivisionAreaClass();
            //面積
            firstDivArea.areaValue = firstArea;
            //端点
            firstDivArea.startPoint = new Vector2(startXPoint,startYPoint);
            firstDivArea.endPoint = new Vector2(xDirectionPoint, yDirectionPoint);
            //分割したエリアの情報をセット(二つ目)
            DivisionAreaClass secondDivArea = new DivisionAreaClass();
            //面積
            secondDivArea.areaValue = maxArea - firstArea;
            //端点
            secondDivArea.startPoint = new Vector2(secondStartXPoint, secondStartYPoint);
            secondDivArea.endPoint = new Vector2(endXPoint,endYPoint);
            divAreaList.Add(firstDivArea);
            divAreaList.Add(secondDivArea);
            //面積で降順に並び替える
            divAreaList.Sort((a, b) => b.areaValue - a.areaValue);
            //面積が最大のエリアを元に次のエリア分割を行う
            maxArea = divAreaList[0].areaValue;
            startXPoint = (int)divAreaList[0].startPoint.x;
            startYPoint = (int)divAreaList[0].startPoint.y;
            endXPoint = (int)divAreaList[0].endPoint.x;
            endYPoint = (int)divAreaList[0].endPoint.y;
            splitAreaFlg = true;
            //最小の面積が一定値を下回った場合は分割を終了する
            if (divAreaList[divAreaList.Count - 1].areaValue < Define.MIN_AREA)
            {
                break;
            }
        }
        return divAreaList;
    }

    /**
     * 分割した区画を縮小する
     */
    private void reduceDivisionArea(List<DivisionAreaClass> divAreaList)
    {
        int randomInnerXPoint;
        int randomInnerYPoint;
        for (int i=0;i<divAreaList.Count;i++)
        {
            for (int j=0;j<2;j++)
            {
                randomInnerXPoint = Random.Range((int)(1 + divAreaList[i].endPoint.x - divAreaList[i].startPoint.x) / 5, 1 + (int)(divAreaList[i].endPoint.x - divAreaList[i].startPoint.x) / 3);
                randomInnerYPoint = Random.Range((int)(1 + divAreaList[i].endPoint.y - divAreaList[i].startPoint.y) / 5, 1 + (int)(divAreaList[i].endPoint.y - divAreaList[i].startPoint.y) / 3);
                //startpoint(左下の点)の設定
                if (j == 0)
                {
                    divAreaList[i].innerStartPoint = new Vector2(divAreaList[i].startPoint.x + randomInnerXPoint, divAreaList[i].startPoint.y + randomInnerYPoint);
                }
                //endpoint(右下の点)の設定
                else
                {
                    divAreaList[i].innerEndPoint = new Vector2(divAreaList[i].endPoint.x - randomInnerXPoint, divAreaList[i].endPoint.y - randomInnerYPoint);
                }
            }
        }
    }

    /**
     * 境界線に対して通路を伸ばす
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
            //左方向
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
                    for (int k = (int)divAreaList[i].innerStartPoint.x ; k >= divAreaList[i].startPoint.x; k--)
                    {
                        aisleList.Add(new Vector2(k, randomPoint));
                        //通路の終点をリストに追加
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

            //下方向
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
                    for (int k = (int)divAreaList[i].innerStartPoint.y ; k >= divAreaList[i].startPoint.y; k--)
                    {
                        aisleList.Add(new Vector2(randomPoint, k));
                        //通路の終点をリストに追加
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

            //右方向
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
                    for (int k = (int)divAreaList[i].innerEndPoint.x ; k <= divAreaList[i].endPoint.x; k++)
                    {
                        aisleList.Add(new Vector2(k, randomPoint));
                        //通路の終点をリストに追加
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

            //上方向
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
                    for (int k = (int)divAreaList[i].innerEndPoint.y ; k <= divAreaList[i].endPoint.y; k++)
                    {
                        aisleList.Add(new Vector2(randomPoint, k));
                        //通路の終点をリストに追加
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
        //Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        //for (int i=0;i<aisleEndList.Count;i++)
        //{
        //    Debug.Log("aisleEndlist"+aisleEndList[i]);
        //}
        //Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
        return aisle;
    }

    /**
     * 配列にマップ情報(移動可能エリア、移動不可エリア)を格納する
     */
    private void pushMapInfo(List<DivisionAreaClass> divAreaList, createMapArrayClass[,] createMapArray)
    {
        for (int i=0;i<divAreaList.Count;i++)
        {
            for (int j=(int)divAreaList[i].startPoint.y ;j<=divAreaList[i].endPoint.y; j++)
            {
                for (int k=(int)divAreaList[i].startPoint.x; k<=divAreaList[i].endPoint.x; k++)
                {
                    //移動可能エリア
                    if (j >= divAreaList[i].innerStartPoint.y && j <= divAreaList[i].innerEndPoint.y && k >= divAreaList[i].innerStartPoint.x && k <= divAreaList[i].innerEndPoint.x)
                    {
                        //Debug.Log("j:" + j);
                        //Debug.Log("k:" + k);
                        createMapArray[j, k] = new createMapArrayClass {areaDivNum = i,tileType = Define.MOVABLE };
                    }
                    //移動不可エリア
                    else
                    {
                        //Debug.Log("j:" + j);
                        //Debug.Log("k:" + k);
                        createMapArray[j, k] = new createMapArrayClass { tileType = Define.WALL };
                    }
                }
            }
        }
    }

    /**
     * 配列にマップ情報(通路)を格納する
     */
    private void pushMapInfoAisle(List<Vector2> aisleList, createMapArrayClass[,] createMapArray)
    {
        for (int i=0;i<aisleList.Count;i++)
        {
            //Debug.Log("aisleList[i].x"+ aisleList[i].x);
            //Debug.Log("aisleList[i].y"+ aisleList[i].y);
            createMapArray[(int)aisleList[i].y, (int)aisleList[i].x] = new createMapArrayClass { areaDivNum = null, tileType = Define.AISLE };
        }
    }

    /**
     * 通路と通路をつなぐ
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
            //左辺
            if (divAreaList[i].startPoint.x != 0)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //x軸の一致するポイントをリストに追加
                    if (copyAisleEndPointList[j].x == divAreaList[i].startPoint.x || copyAisleEndPointList[j].x == divAreaList[i].startPoint.x - 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //リストの中身が2以上の場合
                if (sameAxisList.Count > 1)
                {
                    //リストをソートして最大と最小間を通路とする
                    sameAxisList.Sort((a, b) => (int)a.y - (int)b.y);
                    int minPoint = (int)sameAxisList[0].y;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].y;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //通路の座標を追加
                        hookAisleList.Add(new Vector2(divAreaList[i].startPoint.x, k));
                    }
                    //通路を結び終えた点はリストから削除
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
            //下辺
            if (divAreaList[i].startPoint.y != 0)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //y軸の一致するポイントをリストに追加
                    if (copyAisleEndPointList[j].y == divAreaList[i].startPoint.y || copyAisleEndPointList[j].y == divAreaList[i].startPoint.y - 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //リストの中身が2以上の場合
                if (sameAxisList.Count > 1)
                {
                    //リストをソートして最大と最小間を通路とする
                    sameAxisList.Sort((a, b) => (int)a.x - (int)b.x);
                    int minPoint = (int)sameAxisList[0].x;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].x;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //通路の座標を追加
                        hookAisleList.Add(new Vector2(k, divAreaList[i].startPoint.y));
                    }
                    //通路を結び終えた点はリストから削除
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
            //右辺
            if (divAreaList[i].endPoint.x != randomMapWidth)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //x軸の一致するポイントをリストに追加
                    if (copyAisleEndPointList[j].x == divAreaList[i].endPoint.x || copyAisleEndPointList[j].x == divAreaList[i].endPoint.x + 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //リストの中身が2以上の場合
                if (sameAxisList.Count > 1)
                {
                    //リストをソートして最大と最小間を通路とする
                    sameAxisList.Sort((a, b) => (int)a.y - (int)b.y);
                    int minPoint = (int)sameAxisList[0].y;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].y;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //通路の座標を追加
                        hookAisleList.Add(new Vector2(divAreaList[i].endPoint.x, k));
                    }
                    //通路を結び終えた点はリストから削除
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
            //上辺
            if (divAreaList[i].endPoint.y != randomMapHeight)
            {
                for (int j = 0; j < copyAisleEndPointList.Count; j++)
                {
                    //y軸の一致するポイントをリストに追加
                    if (copyAisleEndPointList[j].y == divAreaList[i].endPoint.y || copyAisleEndPointList[j].y == divAreaList[i].endPoint.y + 1)
                    {
                        sameAxisList.Add(copyAisleEndPointList[j]);
                    }
                }
                //リストの中身が2以上の場合
                if (sameAxisList.Count > 1)
                {
                    //リストをソートして最大と最小間を通路とする
                    sameAxisList.Sort((a, b) => (int)a.x - (int)b.x);
                    int minPoint = (int)sameAxisList[0].x;
                    int maxPoint = (int)sameAxisList[sameAxisList.Count - 1].x;
                    for (int k = minPoint; k <= maxPoint; k++)
                    {
                        //通路の座標を追加
                        hookAisleList.Add(new Vector2(k, divAreaList[i].endPoint.y));
                    }
                    //通路を結び終えた点はリストから削除
                    for (int l = 0; l < sameAxisList.Count; l++)
                    {
                        copyAisleEndPointList.Remove(sameAxisList[l]);
                    }
                }
            }
            sameAxisList.Clear();
        }
        //Debug.Log("copyAisleEndPointListcount"+ copyAisleEndPointList.Count);
        for (int i=0;i<copyAisleEndPointList.Count;i++)
        {
            //Debug.Log("copyAisleEndPointList"+ copyAisleEndPointList[i]);
        }
        //接続した通路の座標リスト
        return hookAisleList;
    }

    /**
     * 配列のマップ情報からオブジェクトをセットする
     */
    private void createLabyrinthMap(createMapArrayClass[,] createMapArray,int hierarchyIndex)
    {
        GameObject floreTile = labyrinthMapCreateMap.objList[hierarchyIndex].floreObj;
        GameObject wallTile = labyrinthMapCreateMap.objList[hierarchyIndex].wallObj;
        //フロア(アクセント用)がnull以外でフィルター
        List<GameObject> subFloreTileList = labyrinthMapCreateMap.objList[hierarchyIndex].subFloreObjList.FindAll(obj => obj != null);
        //フロア(効果付き)がnull以外でフィルター
        List<GameObject> effectFloreTileList = labyrinthMapCreateMap.objList[hierarchyIndex].effectFloreObjList.FindAll(obj => obj != null);
        //外壁(アクセント用)がnull以外でフィルター
        List <GameObject> subWallTileList = labyrinthMapCreateMap.objList[hierarchyIndex].subWallObjList.FindAll(obj => obj != null);
        GameObject tile;
        GameObject effectTile;
        for (int i=-1;i<=createMapArray.GetLength(0);i++)
        {
            for (int j=-1;j<=createMapArray.GetLength(1);j++)
            {
                //Debug.Log("Vector2"+i+"  "+j);
                effectTile = null;
                bool isMapEdge = i == -1 || i == createMapArray.GetLength(0) || j == -1 || j == createMapArray.GetLength(1);
                //マップ端または移動不可エリア
                if (isMapEdge || createMapArray[i, j].tileType == Define.WALL)
                {
                    //先に通常のフロアを設置
                    Instantiate(floreTile, new Vector3(j, i, 0), Quaternion.identity);
                    //アクセント用の外壁がセットされている場合は確率で設置
                    tile = subWallTileList.Count > 0 ? getRandomWallTile(wallTile, subWallTileList[Random.Range(0, subWallTileList.Count)]) : wallTile;
                    GameObject outerWallObj = Instantiate(tile, new Vector3(j, i, 0), Quaternion.identity) as GameObject;
                    //移動不可地点リストに座標を追加
                    GManager.instance.unmovableList.Add(new Vector2(j, i));
                    //マップ端の外壁を破壊不可にする
                    if (isMapEdge)
                    {
                        //outerWallObj.GetComponent<OuterWallScript>().isIndestructible = true;
                        DamageActionComponentOuterWall damageActionObj = outerWallObj?.GetComponent<DamageActionComponentOuterWall>();
                        if (damageActionObj != null)
                        {
                            damageActionObj.isIndestructible = true;
                        }
                    }
                    continue;
                }
                //Debug.Log("createMapArray[i,j]:" + createMapArray[i, j]);
                switch (createMapArray[i, j].tileType)
                {
                    //移動可能エリア(通常フロア)
                    case Define.MOVABLE:

                        if (createMapArray[i, j].areaDivNum == null)
                        {
                            continue;
                        }

                        //アクセント用のフロアがセットされている場合は確率で設置
                        tile = subFloreTileList.Count > 0 ? getRandomWallTile(floreTile, subFloreTileList[Random.Range(0, subFloreTileList.Count)]) : floreTile;

                        //効果付きタイルがセットされている場合は確率で設置
                        if (effectFloreTileList.Count > 0 && Random.Range(1, 101) <= Define.WALL_LOTTERY_PARAM)
                        {
                            effectTile = effectFloreTileList[Random.Range(0, effectFloreTileList.Count)];
                        }

                        //効果付きタイルの上にオブジェクトを設置しない
                        if (effectTile == null)
                        {
                            //アイテムと敵の配置用に座標を保存する
                            //キーが存在する場合
                            if (gridPositionsDictionary.ContainsKey((int)createMapArray[i, j].areaDivNum))
                            {
                                gridPositionsDictionary[(int)createMapArray[i, j].areaDivNum].Add(new Vector3(j, i, 0));
                            }
                            else
                            {
                                gridPositionsDictionary.Add((int)createMapArray[i, j].areaDivNum, new List<Vector3> { new Vector3(j, i, 0) });
                            }
                        }
                        break;
                    //通路
                    case Define.AISLE:
                        tile = floreTile;
                        break;
                    default:
                        tile = floreTile;
                        break;
                }
                Instantiate(tile, new Vector3(j,i,0), Quaternion.identity);
                //効果付きタイルを設置する場合
                if (effectTile != null)
                {
                    Instantiate(effectTile, new Vector3(j, i, 0), Quaternion.identity);
                }
            }
        }
    }

    /**
     * 設置する外壁をランダムで選択して返却
     */
    private GameObject getRandomWallTile(GameObject mainTile,GameObject subTile)
    {
        int randomNum = Random.Range(1,101);
        GameObject wall = mainTile;
        //一定の確率でアクセント用の外壁を返却
        if (Random.Range(1, 101) <= Define.WALL_LOTTERY_PARAM)
        {
            wall = subTile;
        }
        return wall;
    }

    /**
     * マップ上にオブジェクトを配置
     */
    private void layoutObjectInLabyrinth(int hierarchyIndex,bool isBossMode)
    {
        List<MultipleSettingObjectClass> compositeObjList = new List<MultipleSettingObjectClass>();
        List<SettingGameObjClass> settingGameObjList = new List<SettingGameObjClass>();
        //プレイヤーを設置
        SettingPlayerPositionClass settingPlayerInfo = new SettingPlayerPositionClass();
        LayoutPlayerAtRandom(settingPlayerInfo);
        //キーが存在しない場合
        if (settingPlayerInfo.nowPlyerPositionKey == null)
        {
            return;
        }

        //NPC(仲間)を設置
        if (GManager.instance.fellows.Count > 0)
        {
            LayoutNpcAtRandom(settingPlayerInfo.randomPosition, (int)settingPlayerInfo.nowPlyerPositionKey);
        }

        //ボス戦マップ
        if (isBossMode)
        {
            //ボスモンスターを設置
            LayoutObjectAtRandomMysterymap(labyrinthMapCreateMap.objList[hierarchyIndex].bossMonsterObj, 1, 1, false, null);
            //アイテム
            compositeObjList = labyrinthMapCreateMap.objList[hierarchyIndex].itemObjList.FindAll(obj => !obj.noSettingFlg && !obj.noSettingBossMap && obj.multipleSettingObj != null);
            //敵
            compositeObjList.AddRange(labyrinthMapCreateMap.objList[hierarchyIndex].enemyObjList.FindAll(obj => !obj.noSettingFlg && !obj.noSettingBossMap && obj.multipleSettingObj != null));
            foreach (MultipleSettingObjectClass item in compositeObjList)
            {
                settingGameObjList.Add(
                    new SettingGameObjClass
                    {
                        settingObj = item.multipleSettingObj,
                        minSetNum = item.minSettingNumBossMap,
                        maxSetNum = item.maxSettingNumBossMap
                    }
                );
            }
        }
        //通常マップ
        else
        {
            //階段を配置
            LayoutObjectAtRandomMysterymap(stairs, 1, 1, true, null);
            //モンスターハウスが出現する場合
            if (Random.Range(1, 101) <= Define.MONSTERHOUSE_PROBABILITY)
            {
                createMonsterHouse((int)settingPlayerInfo.nowPlyerPositionKey, hierarchyIndex);
            }
            //アイテム
            compositeObjList = labyrinthMapCreateMap.objList[hierarchyIndex].itemObjList.FindAll(obj => !obj.noSettingFlg && !obj.noSettingNomalMap && obj.multipleSettingObj != null);
            //敵
            compositeObjList.AddRange(labyrinthMapCreateMap.objList[hierarchyIndex].enemyObjList.FindAll(obj => !obj.noSettingFlg && !obj.noSettingNomalMap && obj.multipleSettingObj != null));
            //トラップ
            compositeObjList.AddRange(labyrinthMapCreateMap.objList[hierarchyIndex].trapObjList.FindAll(obj => !obj.noSettingFlg && !obj.noSettingNomalMap && obj.multipleSettingObj != null));
            // 階層に関係なく設置するオブジェクト
            compositeObjList.AddRange(labyrinthMapCreateMap.allFloorSettingObjList.FindAll(obj => !obj.noSettingFlg && !obj.noSettingNomalMap && obj.multipleSettingObj != null));
            foreach (MultipleSettingObjectClass item in compositeObjList)
            {
                settingGameObjList.Add(
                    new SettingGameObjClass
                    {
                        settingObj = item.multipleSettingObj,
                        minSetNum = item.minSettingNumNomalMap,
                        maxSetNum = item.maxSettingNumNomalMap
                    }
                );
            }
        }
        //複数設置用オブジェクトを設置
        for (int i = 0; i < settingGameObjList.Count; i++)
        {
            LayoutObjectAtRandomMysterymap(settingGameObjList[i].settingObj, settingGameObjList[i].minSetNum, settingGameObjList[i].maxSetNum, false, null);
        }
    }

    /**
     * ボスマップ用の情報を格納する
     */
    private void pushBossMapInfo(createMapArrayClass[,] createMapArray)
    {
        //Debug.Log("createMapArray.GetLength(0)"+ createMapArray.GetLength(0));
        //Debug.Log("createMapArray.GetLength(1)"+ createMapArray.GetLength(1));
        for (int i = 0; i < createMapArray.GetLength(0); i++)
        {
            for (int j = 0; j < createMapArray.GetLength(1); j++)
            {
                //移動不可エリア
                if (i == 0 || i == createMapArray.GetLength(0) -1 || i == createMapArray.GetLength(0) || 
                    j == 0 || j == createMapArray.GetLength(1)-1 || j == createMapArray.GetLength(1)
                    )
                {
                    createMapArray[i, j] = new createMapArrayClass { tileType = Define.WALL };
                }
                //移動可能エリア
                else
                {
                    createMapArray[i, j] = new createMapArrayClass { areaDivNum = 0, tileType = Define.MOVABLE };
                }
            }
        }
    }


    /**
     * マップの各区画にオブジェクトを設置する
     */
    private void settingObjectBySection(floorAttribute floorDivisionObj,int settingObjRule)
    {
        mapListIndex.Clear();
        //草原フロア
        if (floorDivisionObj.settingMapObjRule == Define.FOREST_DIVISION)
        {
            //木オブジェクト(集合体)を設置
            createMapRandomObjects(HardWood);
            //石タイル(装飾用)を設置
            //createMapRandomObjects(stoneTile);
        }
        //土フロア
        else if (floorDivisionObj.settingMapObjRule == Define.SOIL_DIVISION)
        {
            //土山のオブジェクト(集合体)を設置
            //createMapRandomObjects(soilMountain, 1);
        }
        //岩エリア
        else if (floorDivisionObj.settingMapObjRule == Define.STONE_DIVISION)
        {
            //岩山のオブジェクト(集合体)を設置
            //createMapRandomObjects(StoneMountain, 1);
        }
        //砂漠エリア
        else if (floorDivisionObj.settingMapObjRule == Define.DESERT_DIVISION)
        {
            //土山のオブジェクト(集合体)を設置
            //createMapRandomObjects(WaterPlace, 1);
        }

        //使用したリストを削除する
        deleteRandomPoinst();
        //対数進行に基づいて、現在のレベル数に基づいて敵の数を決定します
        int enemyCount = (int)Mathf.Log(2, 2f);
        ////Debug.Log(enemyCount);
        enemyCount = 2;

        //オブジェクト設置ルールを基にオブジェクトを設置
        //第一区画
        if (settingObjRule == Define.FIRST_SETTING)
        {
            //食べ物をインスタンス化。
            LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum,false);
            //ポーションをインスタンス化
            //LayoutObjectAtRandom(portion, portionCount.minmum, portionCount.maximum);
            //宝箱をインスタンス化
            LayoutObjectAtRandom(treasure, portionCount.minmum, portionCount.maximum, true);
            //カバンをインスタンス化。
            LayoutObjectAtRandom(bag, 1, 1, false);
            //NPCをインスタンス化
            LayoutObjectAtRandom(testNpc, 1, 1, true);
            //NPC(プレイヤーのHPを回復する)をインスタンス化
            //LayoutObjectAtRandom(testRecoveryHpNpc, 1,1,true);
            //NPC(分岐ありのアイテムの引き渡し)をインスタンス化
            LayoutObjectAtRandom(testGiveItemNpcBranchMessage,1,1, true);
            //NPC(会話分岐用)をインスタンス化
            LayoutObjectAtRandom(testBranchMessageNpc,1,1, true);
            //NPC(道具屋)をインスタンス化
            LayoutObjectAtRandom(testSalesNpc,1,1, true);
            //NPC(仲間)をインスタンス化
            LayoutObjectAtRandom(fellowTestNpc, 2, 2, true);
            //NPC2(仲間)をインスタンス化
            LayoutObjectAtRandom(fellowTestNpc2, 1, 1, true);
            //アイテムを渡すテスト用NPC(メッセージ表示中に自動で渡す)
            //LayoutObjectAtRandom(autoGiveItem,1,1);
            //本をインスタンス化
            LayoutObjectAtRandom(bookDamageAllEnemy, 3,3, false);
            //イベント用NPC(骸骨が出現)
            LayoutObjectAtRandom(eventNpcAppearanceSkelton, 1,1,true);
            //イベント用NPC(ゴーストが出現)
            LayoutObjectAtRandom(eventNpcAppearanceGhost, 1, 1, true);
            //LayoutObjectAtRandom(enemy, enemyCount, enemyCount, false);
            //イベント用アイテム
            //LayoutObjectAtRandom(eventTestItem, 1, 1, true);
        }
        //第二区画
        else if(settingObjRule == Define.SECOND_SETTING)
        {
            //剣をインスタンス化
            LayoutObjectAtRandom(sword, swordCount.minmum, swordCount.maximum, false);
            //LayoutObjectAtRandom(enemy2, enemyCount - 1, enemyCount - 1, false);
        }

        //ランダム化された位置で、最小値と最大値に基づいてランダムな数の壁タイルをインスタンス化します。
        //LayoutObjectAtRandom(Wall, Wallcount.minmum, Wallcount.maximum);

        ////ランダム化された位置で、最小値と最大値に基づいてランダムな数の食品タイルをインスタンス化します。
        //LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum);

        ////ポーションをインスタンス化
        //LayoutObjectAtRandom(portion, portionCount.minmum, portionCount.maximum);

        ////剣をインスタンス化
        //LayoutObjectAtRandom(sword, swordCount.minmum, swordCount.maximum);

        //盾をインスタンス化
        //LayoutObjectAtRandom(shield, swordCount.minmum, swordCount.maximum);

        //対数進行に基づいて、現在のレベル数に基づいて敵の数を決定します
        //int enemyCount = (int)Mathf.Log(2, 2f);
        ////Debug.Log(enemyCount);
        enemyCount = 2;
        //ランダム化された位置で、最小値と最大値に基づいてランダムな数の敵をインスタンス化します。
        //LayoutObjectAtRandom(enemy, enemyCount, enemyCount, false);
        //LayoutObjectAtRandom(enemy2, enemyCount - 1, enemyCount - 1, false);

        //ゲームボードの右上隅に出口タイルをインスタンス化します
        //Instantiate(exit, new Vector3(rows - 1, columns - 1, 0f), Quaternion.identity);
        //Instantiate(exit, new Vector3(0, 3, 0f), Quaternion.identity);
    }

    public void destroyBoard()
    {
        Destroy(boardHolder);
    }

    public void LayoutEnemyAtRandom(GameObject tile, int minimum, int maximum)
    {
        //生成するアイテムの個数を最小最大値からランダムに決め、objectCountに設定する
        int objectCount = Random.Range(minimum, maximum);

        //設置するオブジェクトの数分ループで回す
        for (int i = 0; i < objectCount; i++)
        {
            //現在オブジェクトが置かれていない、ランダムな位置を取得
            Vector3 randomPosition = RandomPosition();

            //生成
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }

    /**
     * 乱数を用いてオブジェクト(集合体)を生成する
     * 最初の一点を取得後周りにオブジェクトを設置する
     * @param tile　設置するオブジェクト
     */
    public void createMapRandomObjects(GameObject tile)
    {
        TileBase tileObj;
        //設置するオブジェクトにTileBaseがアタッチされていない場合は設置不可
        if ((tileObj = tile.GetComponent<TileBase>()) == null)
        {
            return;
        }
        //設置ルールがランダムの場合は、ルール0～2を代入
        int settingAlgorithmNum = tileObj.settingFuncRule == Define.RANDOM_SETTING ? Random.Range(Define.NORMAL_SETTING, Define.RANDOM_SETTING) : tileObj.settingFuncRule;
        //オブジェクト(集合体)の設置個数を計算
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
        //乱数の数だけオブジェクト(オブジェクトの集合体)を作成する
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
            //インデックスが見つかった場合にオブジェクトを設置する
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
                //TileBaseで定義されたオブジェクトの設置ルールに従い関数を呼び出す
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
     * 乱数で取得した点の周りにオブジェクト(単体)を設置する
     * @param tile 設置するゲームオブジェクト
     * @param position 最初に取得する地点
     */
    public void createTile(GameObject tile, int getIndex, int minNum,int maxNum)
    {
        //乱数で値取得(設置するオブジェクトの数)
        int randomNum = Random.Range(minNum, maxNum);
        Debug.Log("randomnum" + randomNum);
        int minXRange = -1;
        int maxXRange = 1;
        int minYRange = -1;
        int maxYRange = 1;
        //乱数の数だけループ
        for (int i=0;i<randomNum;i++)
        {
            //最初に取得した点付近でランダムな点を取得
            int permitCount = 0 ;
            int listIndex;
            //オブジェクト設置点探索の上限回数
            permitMaxSettingSingleNum = (maxXRange-minXRange+1) * (maxYRange-minYRange+1) - 1;
            do
            {
                //最初に取得した点付近でランダムな点を取得
                float settingPointX = Random.Range(minXRange, maxXRange);
                float settingPointY = Random.Range(minYRange, maxYRange);
                listIndex = getMapIndex(settingPointX, settingPointY, getIndex);
                permitCount++;
            }
            while (!checkRandomListIndex(listIndex) && permitCount < permitMaxSettingSingleNum);
            //設置範囲外のインデックスを取得した場合
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
     * 乱数で取得した点の周りにオブジェクト(単体)を設置する
     * createTile()より密集した状態に設置
     * @param tile 設置するゲームオブジェクト
     * 
     */
    public void createDenceTile(GameObject tile, int getIndex, int minNum, int maxNum)
    {
        //乱数で値取得(設置するオブジェクトの数)
        int randomNum = Random.Range(minNum, maxNum);
        Debug.Log("randomnum" + randomNum);
        int minXRange = isLeft;
        int maxXRange = isRight;
        int minYRange = isBottom;
        int maxYRange = isTop;
        //乱数の数だけループ
        for (int i = 0; i < randomNum; i++)
        {
            if (denceParam < 0)
            {
                denceParam = 1;
            }
            Debug.Log("denceparam"+denceParam);
            int permitCount = 0;
            int listIndex;
            //オブジェクト設置点探索の上限回数
            permitMaxSettingSingleNum = (maxXRange - minXRange + 1) * (maxYRange - minYRange + 1) - 1;
            for (int j=0;j< denceParam; j++)
            {
                do
                {
                    //最初に取得した点付近でランダムな点を取得
                    float settingPointX = Random.Range(minXRange, maxXRange);
                    float settingPointY = Random.Range(minYRange, maxYRange);
                    listIndex = getMapIndex(settingPointX, settingPointY, getIndex);
                    permitCount++;
                }
                while (!checkRandomListIndex(listIndex) && permitCount < permitMaxSettingSingleNum);
                //設置範囲外のインデックスを取得した場合
                if (listIndex == -1)
                {
                    return;
                }
                //探索許容最大回数を超えずにインデックスを取得できた場合
                if (permitCount < permitMaxSettingSingleNum)
                {
                    mapListIndex.Add(listIndex);
                    Debug.Log("settingposition"+gridPositons[listIndex]);
                    Instantiate(tile, gridPositons[listIndex], Quaternion.identity);
                }
                //探索許容最大回数を超えた場合は範囲を広げて探索する
                else
                {
                    break;
                }
            }
            //中心点から離れるほどばらけて設置される
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
          
            //初期地点がマップの右端
            if (isRight == 0)
            {
                minXRange += -1;
            }
            //初期地点がマップの左端
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
            //初期地点がマップの上
            if (isTop == 0)
            {
                minYRange += Random.Range(-1, 0);
            }
            //初期地点がマップの下
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

    //gridPositionsからランダムな位置を取得する
    int getRandomPositionForMap()
    {
        Debug.Log("gridcount" + gridPositons.Count);
        int randomIndex = Random.Range(0, gridPositons.Count);
        return randomIndex;
    }

    /**
     * 乱数で取得したリストのインデックスが既に使用されていないかチェックする
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
     * columとrowからリストのインデックスを取得
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
     * リストから該当するインデックスの要素を削除する
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
     * 初期取得点から縦方向にオブジェクトを設置し横方向に広げる
     */
    private void createDenceTileVertical(GameObject tile, int getIndex,float verticalSettingParam)
    {
        //上方向に拡張
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

    //verticalParamから左右に拡張する
    private void createDenceTileHorizontal(GameObject tile,int verticalParam)
    {
        //int rightParam = Random.Range(1,5);
        //int leftParam = Random.Range(1,5);
        int rightParam = Random.Range(1, (int)(singleDivisionColumns*tile.GetComponent<TileBase>().horizontalParam));
        int leftParam = Random.Range(1, (int)(singleDivisionColumns * tile.GetComponent<TileBase>().horizontalParam));
        int horizontalParam = verticalParam;
        Debug.Log("right"+rightParam);
        Debug.Log("left" + leftParam);
        //右方向に拡張
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
        //左方向に拡張
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
     * columとrowからリストのインデックスを取得
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
     * 固定オブジェクトをランダムに配置する
     */
    private void randomSettingBuilding(GameObject tile, int minimum, int maximum, BuildingObject buildingObj)
    {
        //乱数の数だけオブジェクト(オブジェクトの集合体)を作成する
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
            //インデックスが見つかった場合にオブジェクトを設置する
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
     * ランダムに取得した点が使用可能か判定する
     */
    private bool getRandomPositionBuilding(int randomIndex,BuildingObject buildObj)
    {
        int remainderIndex = randomIndex%(nextLineValue);
        //Debug.Log("randomindex" + randomIndex);
        Debug.Log("startcheck");
        //固定オブジェクトを設置するスペースが存在するかチェック
        //左方向
        if (remainderIndex - buildObj.width < 0)
        {
            //Debug.Log("remainder"+remainderIndex);
            //Debug.Log("buildobjwidth"+buildObj.width);
            //Debug.Log("rightcheckerror");
            return false;
        }
        //右方向
        if (remainderIndex + buildObj.width > (nextLineValue) - 1)
        {
            //Debug.Log("leftcheckerror");
            return false;
        }
        //下方向
        if (randomIndex + (buildObj.height * (rows - 2)) > gridPositons.Count)
        {
            //Debug.Log("bottomcheckerror");
            return false;
        }
        //上方向
        if (randomIndex - (buildObj.height * (rows - 2)) < 0)
        {
            //Debug.Log("topcheckerror");
            return false;
        }
        Debug.Log("finishrangecheck");
        //スペースが確保できている場合、他のオブジェクトが既に設置されていないかチェック
        int rangeNum = (1 + buildObj.height * 2) * (1 + buildObj.width * 2);
        //左下を始点にする
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
            //既に使用されている場合は探索終了
            if (!checkRandomListIndex(checkIndex))
            {
                return false;
            }
            createObjIndexList.Add(checkIndex);
            //一行分の探索が終了したら次の行から探索開始
            if (i%(nextLineValue) == (buildObj.width*2)+1)
            {
                Debug.Log("turningpoint");
                startPosition += rows - 2;
            }
        }
        return true;
    }

    /**
     * 固定オブジェクトが設置される点をmapListIndexに格納
     */
    private void addBuildingObject()
    {
        for (int i=0;i< createObjIndexList.Count;i++)
        {
            mapListIndex.Add(createObjIndexList[i]);
        }
    }
}