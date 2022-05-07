using System.Collections;
using System.Collections.Generic;
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

    //Mapの縦横
    [Header("マップの行数")]public int rows;
    [Header("マップの列数")]public int columns;

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

    //プレイヤー
    [Header("プレイヤー")] public GameObject player;

    //建物系オブジェクト
    [Header("城(固定オブジェクト)")] public GameObject castleObj;
    [Header("建物1")] public GameObject buildingObj;

    //MAp作成用のパラメータ
    [Header("オブジェクトの密集具合のパラメータ")] public int denceParam = 4;
    [Header("フロア区切りパラメータ")] public float borderParam = 0.25f;
    [Header("マップの分割数")] public int borderDivision = 3;
    [Header("境界線")] public int secondBorder = 8;

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
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

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
    void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        //tileがセットされていない場合はreturn
        if (tile == null)
        {
            return;
        }
        //生成するアイテムの個数を最小最大値からランダムに決め、objectCountに設定する
        int objectCount = Random.Range(minimum, maximum);
        //割り当て用の座標が余っている場合のみ配置可能
        //設置するオブジェクトの数分ループで回す
        for (int i = 0; i < objectCount; i++)
        {
            if (gridPositons.Count > 0)
            {
                //現在オブジェクトが置かれていない、ランダムな位置を取得
                Vector3 randomPosition = RandomPosition();

                //生成
                Instantiate(tile, randomPosition, Quaternion.identity);
            }
        }
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
    public void SetupScene(int level)
    {
        createTreasureItemList();
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

        //オブジェクト設置ルールを基にオブジェクトを設置
        //第一区画
        if (settingObjRule == Define.FIRST_SETTING)
        {
            //食べ物をインスタンス化。
            LayoutObjectAtRandom(food, foodcount.minmum, foodcount.maximum);
            //ポーションをインスタンス化
            //LayoutObjectAtRandom(portion, portionCount.minmum, portionCount.maximum);
            //宝箱をインスタンス化
            LayoutObjectAtRandom(treasure, portionCount.minmum, portionCount.maximum);
            //カバンをインスタンス化。
            LayoutObjectAtRandom(bag, 1, 1);
            //NPCをインスタンス化
            LayoutObjectAtRandom(testNpc, 1, 1);
            //NPC(プレイヤーのHPを回復する)をインスタンス化
            //LayoutObjectAtRandom(testRecoveryHpNpc, 1,1);
            //NPC(分岐ありのアイテムの引き渡し)をインスタンス化
            LayoutObjectAtRandom(testGiveItemNpcBranchMessage,1,1);
            //NPC(会話分岐用)をインスタンス化
            //LayoutObjectAtRandom(testBranchMessageNpc,1,1);
            //NPC(道具屋)をインスタンス化
            LayoutObjectAtRandom(testSalesNpc,1,1);
            //NPC(仲間)をインスタンス化
            LayoutObjectAtRandom(fellowTestNpc, 1, 1);
            //アイテムを渡すテスト用NPC(メッセージ表示中に自動で渡す)
            //LayoutObjectAtRandom(autoGiveItem,1,1);
            //本をインスタンス化
            LayoutObjectAtRandom(bookDamageAllEnemy, 3,3);
        }
        //第二区画
        else if(settingObjRule == Define.SECOND_SETTING)
        {
            //剣をインスタンス化
            LayoutObjectAtRandom(sword, swordCount.minmum, swordCount.maximum);
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
        int enemyCount = (int)Mathf.Log(2, 2f);
        ////Debug.Log(enemyCount);
        enemyCount = 2;
        //ランダム化された位置で、最小値と最大値に基づいてランダムな数の敵をインスタンス化します。
        LayoutObjectAtRandom(enemy, enemyCount, enemyCount);
        LayoutObjectAtRandom(enemy2, enemyCount - 1, enemyCount - 1);

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