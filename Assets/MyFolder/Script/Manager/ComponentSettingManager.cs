using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentSettingManager : MonoBehaviour
{
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
    public class EnemyStatusInfoClass
    {
        public string name;
        public int hp;
        public int attack;
        public int defence;
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

    // ������̓G�̃f�[�^���X�g
    [HideInInspector] public List<RoguelikeEnemyClass> roguelikeEnemyInfoList = new List<RoguelikeEnemyClass>();

    // Start is called before the first frame update
    void Awake()
    {
        //��x�擾������͍ēx�擾���Ȃ�
        if (GManager.instance.componentSettingManager?.roguelikeEnemyInfoList?.Count > 0)
        {
            return;
        }

        //�G�̃f�[�^���擾
        string loadjson = Resources.Load<TextAsset>("json/Component/RoguelikeEnemy").ToString();
        RoguelikeEnemyJsonArrayClass enemyInfosJsonData = new RoguelikeEnemyJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, enemyInfosJsonData);        

        //�U���p�R���|�[�l���g�̃}�X�^�[���擾
        loadjson = Resources.Load<TextAsset>("json/Component/AttackComponentMaster").ToString();
        AttackComponentMasterJsonArrayClass attackComponentJsonData = new AttackComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, attackComponentJsonData);

        //�ړ��p�R���|�[�l���g�̃}�X�^�[���擾
        loadjson = Resources.Load<TextAsset>("json/Component/MoveActionComponentMaster").ToString();
        MoveActionComponentMasterJsonArrayClass moveActionComponentJsonData = new MoveActionComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, moveActionComponentJsonData);

        //�Z���T�[�p�R���|�[�l���g�̃}�X�^�[���擾
        loadjson = Resources.Load<TextAsset>("json/Component/SensorComponentMaster").ToString();
        SensorComponentMasterJsonArrayClass sensorComponentJsonData = new SensorComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, sensorComponentJsonData);

        //�_���[�W�A�N�V�����p�R���|�[�l���g�̃}�X�^�[���擾
        loadjson = Resources.Load<TextAsset>("json/Component/DamageActionComponentMaster").ToString();
        DamageActionComponentMasterJsonArrayClass damageActionComponentJsonData = new DamageActionComponentMasterJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, damageActionComponentJsonData);

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
    }
}
