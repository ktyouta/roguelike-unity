using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class EventManager : MonoBehaviour
{
    public class AroundPositionClass
    {
        public int x;
        public int y;
    }

    [Header("NPC�Ƃ̉�b�ŏo������G(�[��)")] public GameObject appearanceEnemySkelton;
    [Header("NPC�Ƃ̉�b�ŏo������G(�S�[�X�g)")] public GameObject appearanceEnemyGhost;
    [Header("�ʏ�̓G")] public GameObject appearanceEnemyNomal;
    private BoardManager boardManager;
    //�C�x���g�p
    [HideInInspector] public bool skeltonAppearanceFlg = false;
    [HideInInspector] public int skeltonAppearanceEventTurnNum = 0;
    [HideInInspector] public bool ghostAppearanceFlg = false;
    [HideInInspector] public int ghostAppearanceEventTurnNum = 0;
    [HideInInspector] public int nomalEnemyAppearanceTurnNum = 0;
    private int nomalEnemyAppearanceTurn = Define.NOMALENEMY_APPEARANCE_MINTURN;
    private List<AroundPositionClass> aroundPositionList = new List<AroundPositionClass>();

    // Start is called before the first frame update
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
        aroundPositionList.Add(new AroundPositionClass { x = 0, y = -1 }) ;
        aroundPositionList.Add(new AroundPositionClass { x = -1, y = 0 });
        aroundPositionList.Add(new AroundPositionClass { x = 1, y = 0 });
        aroundPositionList.Add(new AroundPositionClass { x = 0, y = 1 });
    }

    // Update is called once per frame
    void Update()
    {
        //�G(�[��)�̏o���C�x���g
        if (skeltonAppearanceFlg && skeltonAppearanceEventTurnNum >= 3)
        {
            appearanceSkelton();
        }
        //�G(�S�[�X�g)�̏o���C�x���g
        if (ghostAppearanceFlg && ghostAppearanceEventTurnNum >= 3)
        {
            appearanceGhost();
        }
        //�G(�ʏ�)�̏o��
        //if (nomalEnemyAppearanceTurnNum >= nomalEnemyAppearanceTurn)
        //{
        //    appearanceNomalEnemy();
        //}
    }

    /**
     * �}�b�v�ɓG(�[��)���o��������
     */
    private void appearanceSkelton()
    {
        skeltonAppearanceFlg = false;
        GameObject appearanceEnemyObj = appearanceEnemySkelton;
        if (appearanceEnemyObj == null)
        {
            return;
        }
        List<Vector2> enemyCandidatePositionList = createCandidateEnemyPosition();
        //�o���\�|�C���g������Ȃ��ꍇ
        if (enemyCandidatePositionList.Count < 1)
        {
            skeltonAppearanceFlg = true;
            return;
        }
        createEnemyOnMap(enemyCandidatePositionList, appearanceEnemyObj);
    }

    /**
     * �}�b�v�ɓG(�S�[�X�g)���o��������
     */
    private void appearanceGhost()
    {
        ghostAppearanceFlg = false;
        GameObject appearanceEnemyObj = appearanceEnemyGhost;
        if (appearanceEnemyObj == null)
        {
            return;
        }
        List<Vector2> enemyCandidatePositionList = createCandidateEnemyPosition();
        //�o���\�|�C���g������Ȃ��ꍇ
        if (enemyCandidatePositionList.Count < 1)
        {
            ghostAppearanceFlg = true;
            return;
        }
        createEnemyOnMap(enemyCandidatePositionList, appearanceEnemyObj);
    }

    /**
     * �}�b�v�ɓG(�ʏ�)���o��������
     */
    private void appearanceNomalEnemy()
    {
        GameObject appearanceEnemyObj = appearanceEnemyNomal;
        if (appearanceEnemyObj == null)
        {
            return;
        }
        List<Vector2> enemyCandidatePositionList = createCandidateEnemyPosition();
        //�o���\�|�C���g������Ȃ��ꍇ
        if (enemyCandidatePositionList.Count < 1)
        {
            return;
        }
        createEnemyOnMap(enemyCandidatePositionList, appearanceEnemyObj);
        nomalEnemyAppearanceTurnNum = 0;
        nomalEnemyAppearanceTurn = Random.Range(Define.NOMALENEMY_APPEARANCE_MINTURN, Define.NOMALENEMY_APPEARANCE_MAXTURN);
    }

    /**
     * �G�̏o�����̃��X�g���쐬����
     */
    private List<Vector2> createCandidateEnemyPosition()
    {
        List<Vector2> enemyCandidatePositionList = new List<Vector2>();
        Vector2 nowPlayerPosition = new Vector2((int)GManager.instance.playerObj.transform.position.x, (int)GManager.instance.playerObj.transform.position.y);
        Vector2 firstPosition = nowPlayerPosition + new Vector2(-2, 1);
        //5�~3�}�X�͈̔͂�G�̏o�����ɂ���
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector2 candidatePosition = firstPosition + new Vector2(i, (-1) * j);
                //�}�b�v�O�̗̈�
                if (candidatePosition.x < 0 || candidatePosition.x > boardManager.columns || candidatePosition.y < 0 || candidatePosition.y > boardManager.rows)
                {
                    continue;
                }
                //�v���C���[�̈ʒu�Ɣ��Ȃ��悤�ɂ���
                if (candidatePosition == nowPlayerPosition)
                {
                    continue;
                }
                //�v���C���[�̗אڃ}�X���`�F�b�N
                if (checkPlayerAroundPosition(nowPlayerPosition,candidatePosition))
                {
                    continue;
                }
                //�ړ��s�̍��W�ƈ�v���邩�̃`�F�b�N
                if (checkUnmovableList(candidatePosition))
                {
                    continue;
                }
                enemyCandidatePositionList.Add(candidatePosition);
            }
        }
        return enemyCandidatePositionList;
    }

    /**
     * �v���C���[�̗אڃ}�X���`�F�b�N
     */
    private bool checkPlayerAroundPosition(Vector2 nowPlyaerPosition,Vector2 candidatePosition)
    {
        for (int i=0;i< aroundPositionList.Count;i++)
        {
            Vector2 aroundPosition = nowPlyaerPosition + new Vector2(aroundPositionList[i].x, aroundPositionList[i].y);
            if (candidatePosition == aroundPosition)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * �o����₩�烉���_���Ȉʒu�ɓG�𐶐�����
     */
     private void createEnemyOnMap(List<Vector2> enemyCandidatePositionList, GameObject appearanceEnemyObj)
     {
        //�����_���ňʒu���擾
        int randomIndex = Random.Range(0, enemyCandidatePositionList.Count);
        GameObject enemyObj = Instantiate(appearanceEnemyObj, enemyCandidatePositionList[randomIndex], Quaternion.identity);
        //�G�̐����ƃ��X�g�ւ̒ǉ�
        GManager.instance.AddEnemyToList(enemyObj, GManager.instance.latestEnemyNumber);
        //GManager.instance.wrightLog(enemyObj.GetComponent<Enemy>().enemyName + "���o�������B");
     }

    /**
     * �ړ��s�ȍ��W�̃`�F�b�N
     */
    private bool checkUnmovableList(Vector2 position)
    {
        for (int i=0;i<GManager.instance.unmovableList.Count;i++)
        {
            if (position == GManager.instance.unmovableList[i])
            {
                return true;
            }
        }
        for (int i=0;i<GManager.instance.enemies.Count;i++)
        {
            if (position == (Vector2)GManager.instance.enemies[i].transform.position)
            {
                return true;
            }
        }
        for (int i=0;i<GManager.instance.fellows.Count;i++)
        {
            if (position == (Vector2)GManager.instance.fellows[i].transform.position)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * �p�����[�^�Ŏ󂯎�����A�C�e�����v���C���[�̎������ɑ��݂��邩�̃`�F�b�N
     */
    private bool checkPalyerPossessions(string itemName)
    {
        for (int i=0;i< ItemManager.itemList.Count;i++)
        {
            if (ItemManager.itemList[i].GetComponent<Item>().name == itemName)
            {
                return true;
            }
        }
        return false;
    }
}
