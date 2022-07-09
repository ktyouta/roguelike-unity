using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("NPCとの会話で出現する敵(骸骨)")] public GameObject appearanceEnemySkelton;
    [Header("NPCとの会話で出現する敵(ゴースト)")] public GameObject appearanceEnemyGhost;
    private BoardManager boardManager;
    //イベントフラグ
    [HideInInspector] public bool skeltonAppearanceFlg = false;
    [HideInInspector] public int skeltonAppearanceEventTurnNum = 0;
    [HideInInspector] public bool ghostAppearanceFlg = false;
    [HideInInspector] public int ghostAppearanceEventTurnNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //敵(骸骨)の出現イベント
        if (skeltonAppearanceFlg && skeltonAppearanceEventTurnNum >= 3)
        {
            appearanceSkelton();
        }
        //敵(ゴースト)の出現イベント
        if (ghostAppearanceFlg && ghostAppearanceEventTurnNum >= 3)
        {
            appearanceGhost();
        }
    }

    /**
     * マップに敵(骸骨)を出現させる
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
        //出現可能ポイントが一つもない場合
        if (enemyCandidatePositionList.Count < 1)
        {
            skeltonAppearanceFlg = true;
            return;
        }
        createEnemyOnMap(enemyCandidatePositionList, appearanceEnemyObj);
    }

    /**
     * マップに敵(ゴースト)を出現させる
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
        //出現可能ポイントが一つもない場合
        if (enemyCandidatePositionList.Count < 1)
        {
            ghostAppearanceFlg = true;
            return;
        }
        createEnemyOnMap(enemyCandidatePositionList, appearanceEnemyObj);
    }

    /**
     * 敵の出現候補のリストを作成する
     */
    private List<Vector2> createCandidateEnemyPosition()
    {
        List<Vector2> enemyCandidatePositionList = new List<Vector2>();
        Vector2 nowPlayerPosition = new Vector2((int)GManager.instance.playerObj.transform.position.x, (int)GManager.instance.playerObj.transform.position.y);
        Vector2 firstPosition = nowPlayerPosition + new Vector2(-2, 1);
        //5×3マスの範囲を敵の出現候補にする
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector2 candidatePosition = firstPosition + new Vector2(i, (-1) * j);
                //マップ外の領域
                if (candidatePosition.x < 0 || candidatePosition.x > boardManager.columns || candidatePosition.y < 0 || candidatePosition.y > boardManager.rows)
                {
                    continue;
                }
                //プレイヤーの位置と被らないようにする
                if (candidatePosition == nowPlayerPosition)
                {
                    continue;
                }
                //移動不可の座標と一致するかのチェック
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
     * 出現候補からランダムな位置に敵を生成する
     */
     private void createEnemyOnMap(List<Vector2> enemyCandidatePositionList, GameObject appearanceEnemyObj)
     {
        //ランダムで位置を取得
        int randomIndex = Random.Range(0, enemyCandidatePositionList.Count);
        GameObject enemyObj = Instantiate(appearanceEnemyObj, enemyCandidatePositionList[randomIndex], Quaternion.identity);
        //敵の生成とリストへの追加
        GManager.instance.AddEnemyToList(enemyObj, GManager.instance.latestEnemyNumber);
        GManager.instance.wrightLog(enemyObj.GetComponent<Enemy>().enemyName + "が出現した。");
     }

    /**
     * 移動不可な座標のチェック
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
}
