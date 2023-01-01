using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ComponentSettingManager
{
    /// <summary>
    /// ���ʂ̃X�e�[�^�X
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
    /// �G�̃f�[�^���X�g
    /// </summary>
    public class RoguelikeEnemyJsonArrayClass
    {
        public RoguelikeEnemyClass[] enemyInfos;
    }

    /// <summary>
    /// �G�̃f�[�^
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
    /// �G�̃X�e�[�^�X
    /// </summary>
    [System.Serializable]
    public class EnemyStatusInfoClass : CommonStatusInfoClass
    {
        public int wallet;
        public int experience;
    }

    /// <summary>
    /// �U���p�R���|�[�l���g�̃f�[�^���X�g
    /// </summary>
    public class AttackComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] attackComponentMaster;
    }

    /// <summary>
    /// �U���p�R���|�[�l���g�̃f�[�^
    /// </summary>
    [System.Serializable]
    public class AttackComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// �ړ��p�R���|�[�l���g�̃f�[�^���X�g
    /// </summary>
    public class MoveActionComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] moveActionComponentMaster;
    }

    /// <summary>
    /// �ړ��p�R���|�[�l���g�̃f�[�^
    /// </summary>
    [System.Serializable]
    public class MoveActionComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// �Z���T�[�p�R���|�[�l���g�̃f�[�^���X�g
    /// </summary>
    public class SensorComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] sensorComponentMaster;
    }

    /// <summary>
    /// �Z���T�[�p�R���|�[�l���g�̃f�[�^
    /// </summary>
    [System.Serializable]
    public class SensorComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// �_���[�W�A�N�V�����p�R���|�[�l���g�̃f�[�^���X�g
    /// </summary>
    public class DamageActionComponentMasterJsonArrayClass
    {
        public AttackComponentMasterClass[] damageActionComponentMaster;
    }

    /// <summary>
    /// �_���[�W�A�N�V�����p�R���|�[�l���g�̃f�[�^
    /// </summary>
    [System.Serializable]
    public class DamageActionComponentMasterClass
    {
        public int id;
        public string name;
    }

    /// <summary>
    /// �v���C���[�̃f�[�^���X�g
    /// </summary>
    public class RoguelikePlayerJsonArrayClass
    {
        public RoguelikePlayerClass[] playerInfos;
    }

    /// <summary>
    /// �v���C���[�̃f�[�^
    /// </summary>
    [System.Serializable]
    public class RoguelikePlayerClass
    {
        public int id;
        public PlayerStatusInfoClass status;
    }

    /// <summary>
    /// �v���C���[�̃X�e�[�^�X
    /// </summary>
    [System.Serializable]
    public class PlayerStatusInfoClass : CommonStatusInfoClass
    {
        public int wallet;
        public int food;
        public int charm;
    }

    // ������̓G�̃f�[�^���X�g
    [HideInInspector] public static List<RoguelikeEnemyClass> roguelikeEnemyInfoList = new List<RoguelikeEnemyClass>();
    // �v���C���[�̃f�[�^���X�g
    [HideInInspector] public static List<RoguelikePlayerClass> roguelikePlayerInfoList = new List<RoguelikePlayerClass>();

    static ComponentSettingManager()
    {
        //�G�̃f�[�^���擾
        string loadEnemyJson = Resources.Load<TextAsset>("json/Component/RoguelikeEnemy").ToString();
        RoguelikeEnemyJsonArrayClass enemyInfosJsonData = new RoguelikeEnemyJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, enemyInfosJsonData);

        //�U���p�R���|�[�l���g�̃}�X�^�[���擾
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/AttackComponentMaster").ToString();
        AttackComponentMasterJsonArrayClass attackComponentJsonData = new AttackComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, attackComponentJsonData);

        //�ړ��p�R���|�[�l���g�̃}�X�^�[���擾
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/MoveActionComponentMaster").ToString();
        MoveActionComponentMasterJsonArrayClass moveActionComponentJsonData = new MoveActionComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, moveActionComponentJsonData);

        //�Z���T�[�p�R���|�[�l���g�̃}�X�^�[���擾
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/SensorComponentMaster").ToString();
        SensorComponentMasterJsonArrayClass sensorComponentJsonData = new SensorComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, sensorComponentJsonData);

        //�_���[�W�A�N�V�����p�R���|�[�l���g�̃}�X�^�[���擾
        loadEnemyJson = Resources.Load<TextAsset>("json/Component/DamageActionComponentMaster").ToString();
        DamageActionComponentMasterJsonArrayClass damageActionComponentJsonData = new DamageActionComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadEnemyJson, damageActionComponentJsonData);

        //�擾�����f�[�^��G�̃f�[�^���X�g�ƌ�������
        roguelikeEnemyInfoList = enemyInfosJsonData.enemyInfos
                        //�U���p�R���|�[�l���g
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
                        //�ړ��p�R���|�[�l���g
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
                        //�Z���T�[�p�R���|�[�l���g
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
                        //�_���[�W�A�N�V�����p�R���|�[�l���g
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

        //�v���C���[�̃f�[�^���擾
        string loadPlayerJson = Resources.Load<TextAsset>("json/Component/RoguelikePlayer").ToString();
        RoguelikePlayerJsonArrayClass playerInfosJsonData = new RoguelikePlayerJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadPlayerJson, playerInfosJsonData);
        roguelikePlayerInfoList = playerInfosJsonData.playerInfos.ToList();
    }
}
