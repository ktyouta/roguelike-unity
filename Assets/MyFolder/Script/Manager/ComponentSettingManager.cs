using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ComponentSettingManager
{
    /// <summary>
    /// 共通のステータス
    /// </summary>
    [System.Serializable]
    public class CommonStatusInfoClass
    {
        public string name;
        public int hp;
        public int attack;
        public int defence;
    }


    /// <summary>
    /// 敵のデータリスト
    /// </summary>
    public class RoguelikeEnemyJsonArrayClass
    {
        public RoguelikeEnemyClass[] enemyInfos;
    }

    /// <summary>
    /// 敵のデータ
    /// </summary>
    [System.Serializable]
    public class RoguelikeEnemyClass
    {
        public int id;
        public EnemyStatusInfoClass status;
        public int enemyType;
        public int attackComponentType;
        public string attackComponentName;
        public int movePointGetComponentType;
        public string movePointGetComponentName;
        public int sensorComponentType;
        public string sensorComponentName;
        public int damageActionComponentType;
        public string damageActionComponentName;
    }

    /// <summary>
    /// 敵のステータス
    /// </summary>
    [System.Serializable]
    public class EnemyStatusInfoClass : CommonStatusInfoClass
    {
        public int wallet;
        public int experience;
    }

    /// <summary>
    /// 攻撃用コンポーネントのデータリスト
    /// </summary>
    public class AttackComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] attackComponentMaster;
    }

    /// <summary>
    /// 攻撃用コンポーネントのデータ
    /// </summary>
    [System.Serializable]
    public class AttackComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// 移動用コンポーネントのデータリスト
    /// </summary>
    public class MoveActionComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] moveActionComponentMaster;
    }

    /// <summary>
    /// 移動用コンポーネントのデータ
    /// </summary>
    [System.Serializable]
    public class MoveActionComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// センサー用コンポーネントのデータリスト
    /// </summary>
    public class SensorComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] sensorComponentMaster;
    }

    /// <summary>
    /// センサー用コンポーネントのデータ
    /// </summary>
    [System.Serializable]
    public class SensorComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// ダメージアクション用コンポーネントのデータリスト
    /// </summary>
    public class DamageActionComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] damageActionComponentMaster;
    }

    /// <summary>
    /// ダメージアクション用コンポーネントのデータ
    /// </summary>
    [System.Serializable]
    public class DamageActionComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// プレイヤーのデータリスト
    /// </summary>
    public class RoguelikePlayerJsonArrayClass
    {
        public RoguelikePlayerClass[] playerInfos;
    }

    /// <summary>
    /// プレイヤーのデータ
    /// </summary>
    [System.Serializable]
    public class RoguelikePlayerClass
    {
        public int id;
        public PlayerStatusInfoClass status;
    }

    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    [System.Serializable]
    public class PlayerStatusInfoClass : CommonStatusInfoClass
    {
        public int wallet;
        public int food;
        public int charm;
    }

    // 結合後の敵のデータリスト
    [HideInInspector] public static List<RoguelikeEnemyClass> roguelikeEnemyInfoList = new List<RoguelikeEnemyClass>();
    // プレイヤーのデータリスト
    [HideInInspector] public static List<RoguelikePlayerClass> roguelikePlayerInfoList = new List<RoguelikePlayerClass>();

    static ComponentSettingManager()
    {
        //敵のデータを取得
        string loadEnemyJson = Resources.Load<TextAsset>("json/Component/RoguelikeEnemy").ToString();
        RoguelikeEnemyJsonArrayClass enemyInfosJsonData = new RoguelikeEnemyJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, enemyInfosJsonData);

        //攻撃用コンポーネントのマスターを取得
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/AttackComponentMaster").ToString();
        AttackComponentMasterJsonArrayClass attackComponentJsonData = new AttackComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, attackComponentJsonData);

        //移動用コンポーネントのマスターを取得
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/MoveActionComponentMaster").ToString();
        MoveActionComponentMasterJsonArrayClass moveActionComponentJsonData = new MoveActionComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, moveActionComponentJsonData);

        //センサー用コンポーネントのマスターを取得
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/SensorComponentMaster").ToString();
        SensorComponentMasterJsonArrayClass sensorComponentJsonData = new SensorComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, sensorComponentJsonData);

        //ダメージアクション用コンポーネントのマスターを取得
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/DamageActionComponentMaster").ToString();
        DamageActionComponentMasterJsonArrayClass damageActionComponentJsonData = new DamageActionComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, damageActionComponentJsonData);

        //取得したデータを敵のデータリストと結合する
        roguelikeEnemyInfoList = enemyInfosJsonData.enemyInfos
                        //攻撃用コンポーネント
                        .Join(
                            attackComponentJsonData.attackComponentMaster,
                            A => A.attackComponentType,
                            B => B.id,
                            (A, B) => new
                            {
                                id = A.id,
                                status = A.status,
                                enemyType = A.enemyType,
                                attackComponentType = A.attackComponentType,
                                attackComponentName = B.name,
                                movePointGetComponentType = A.movePointGetComponentType,
                                movePointGetComponentName = A.movePointGetComponentName,
                                sensorComponentType = A.sensorComponentType,
                                sensorComponentName = A.sensorComponentName,
                                damageActionComponentType = A.damageActionComponentType,
                                damageActionComponentName = A.damageActionComponentName,
                            }
                        )
                        //移動用コンポーネント
                        .Join(
                            moveActionComponentJsonData.moveActionComponentMaster,
                            A => A.movePointGetComponentType,
                            C => C.id,
                            (A, C) => new
                            {
                                id = A.id,
                                status = A.status,
                                enemyType = A.enemyType,
                                attackComponentType = A.attackComponentType,
                                attackComponentName = A.attackComponentName,
                                movePointGetComponentType = A.movePointGetComponentType,
                                movePointGetComponentName = C.name,
                                sensorComponentType = A.sensorComponentType,
                                sensorComponentName = A.sensorComponentName,
                                damageActionComponentType = A.damageActionComponentType,
                                damageActionComponentName = A.damageActionComponentName,
                            }
                        )
                        //センサー用コンポーネント
                        .Join(
                            sensorComponentJsonData.sensorComponentMaster,
                            A => A.sensorComponentType,
                            D => D.id,
                            (A, D) => new
                            {
                                id = A.id,
                                status = A.status,
                                enemyType = A.enemyType,
                                attackComponentType = A.attackComponentType,
                                attackComponentName = A.attackComponentName,
                                movePointGetComponentType = A.movePointGetComponentType,
                                movePointGetComponentName = A.movePointGetComponentName,
                                sensorComponentType = A.sensorComponentType,
                                sensorComponentName = D.name,
                                damageActionComponentType = A.damageActionComponentType,
                                damageActionComponentName = A.damageActionComponentName,
                            }
                        )
                        //ダメージアクション用コンポーネント
                        .Join(
                            damageActionComponentJsonData.damageActionComponentMaster,
                            A => A.damageActionComponentType,
                            E => E.id,
                            (A, E) => new
                            {
                                id = A.id,
                                status = A.status,
                                enemyType = A.enemyType,
                                attackComponentType = A.attackComponentType,
                                attackComponentName = A.attackComponentName,
                                movePointGetComponentType = A.movePointGetComponentType,
                                movePointGetComponentName = A.movePointGetComponentName,
                                sensorComponentType = A.sensorComponentType,
                                sensorComponentName = A.sensorComponentName,
                                damageActionComponentType = A.damageActionComponentType,
                                damageActionComponentName = E.name,
                            }
                        )
                        .Select(table=> new RoguelikeEnemyClass
                        {
                            id = table.id,
                            status = table.status,
                            enemyType = table.enemyType,
                            attackComponentName = table.attackComponentName,
                            movePointGetComponentName = table.movePointGetComponentName,
                            sensorComponentName = table.sensorComponentName,
                            damageActionComponentName = table.damageActionComponentName,
                        }).ToList();

        //プレイヤーのデータを取得
        string loadPlayerJson = Resources.Load<TextAsset>("json/Component/RoguelikePlayer").ToString();
        RoguelikePlayerJsonArrayClass playerInfosJsonData = new RoguelikePlayerJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadPlayerJson, playerInfosJsonData);
        roguelikePlayerInfoList = playerInfosJsonData.playerInfos.ToList();
    }
}
