using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using static MoveActionComponentBase;
using static ComponentSettingManager;
using System;
using System.Linq;

public class Enemy : MovingObject
{
    [HideInInspector] public bool isAction = false;
    [HideInInspector] public SpriteRenderer sr = null;
    protected StatusComponentEnemy statusObj;
    //次の移動点
    List<NextMovePositionClass> nextMovePosition = new List<NextMovePositionClass>();

    //機能コンポーネント
    //攻撃アクション
    [HideInInspector] public AttackComponentBase attackComponentObj;
    //移動アクション
    [HideInInspector] public MoveActionComponentBase moveActionComponentObj;
    //センサー
    [HideInInspector] public SensorComponentBase sensorComponentObj;
    //ダメージアクション
    [HideInInspector] public DamageActionComponentBase damageActionComponentObj;

    //識別用ID(jsonから対応するデータ取得する用)
    [Header("識別用ID")] public int enemyId;


    //Startは、基本クラスの仮想Start関数をオーバーライドします。
    protected override void Start()
    {
        //スタート関数を抽象クラスから呼ぶ
        base.Start();
        sr = GetComponent<SpriteRenderer>();

        //機能コンポーネントを取得
        // IDの一致する敵のデータを取得
        RoguelikeEnemyClass enemyInfo = ComponentSettingManager.roguelikeEnemyInfoList
                                            .Where(e => e.id == enemyId).FirstOrDefault();

        // データの取得に失敗した場合はオブジェクトを削除する
        if (enemyInfo == null)
        {
            Destroy(gameObject);
        }

        //ステータスコンポーネントをセット
        statusObj = GetComponent<StatusComponentEnemy>();
        if (statusObj == null)
        {
            gameObject.AddComponent<StatusComponentEnemy>();
            statusObj = GetComponent<StatusComponentEnemy>();
        }

        //攻撃アクションコンポーネント
        Type addComponentType = Type.GetType(enemyInfo.attackComponentName);
        attackComponentObj = GetComponent<AttackComponentBase>();
        if (attackComponentObj == null)
        {
            gameObject.AddComponent(addComponentType);
            attackComponentObj = GetComponent<AttackComponentBase>();
        }

        //移動点取得コンポーネント
        addComponentType = Type.GetType(enemyInfo.movePointGetComponentName);
        moveActionComponentObj = GetComponent<MoveActionComponentBase>();
        if (moveActionComponentObj == null)
        {
            gameObject.AddComponent(addComponentType);
            moveActionComponentObj = GetComponent<MoveActionComponentAstar>();
        }

        //センサーコンポーネント
        addComponentType = Type.GetType(enemyInfo.sensorComponentName);
        sensorComponentObj = GetComponent<SensorComponentBase>();
        if (sensorComponentObj == null)
        {
            gameObject.AddComponent(addComponentType);
            sensorComponentObj = GetComponent<SensorComponentBase>();
        }

        //ダメージアクションコンポーネント
        addComponentType = Type.GetType(enemyInfo.damageActionComponentName);
        damageActionComponentObj = GetComponent<DamageActionComponentBase>();
        if (damageActionComponentObj == null)
        {
            gameObject.AddComponent(addComponentType);
            damageActionComponentObj = GetComponent<DamageActionComponentBase>();
        }

        //外部アクセス用コンポーネントをセット
        if (GetComponent<OutAccessComponentBase>() == null)
        {
            gameObject.AddComponent<OutAccessComponentBase>();
        }

        //リスポーン時に下方向を向く
        nextHorizontalKey = 0;
        nextVerticalkey = -1;
    }

    private void Update()
    {
        if (!isAttack)
        {
            if (nextHorizontalKey > 0)
            {
                animator?.Play("EnemyRightWalk");
            }
            else if (nextHorizontalKey < 0)
            {
                animator?.Play("EnemyLeftWalk");
            }
            else if (nextVerticalkey > 0)
            {
                animator?.Play("EnemyUpWalk");
            }
            else if (nextVerticalkey < 0)
            {
                animator?.Play("EnemyDownWalk");
            }
        }
    }

    //moveEnemyは毎ターンGameMangerによって呼び出され、各敵にプレイヤーに向かって移動するように指示します。
    public void moveEnemy()
    {
        //倒されていないまたは画面内にいる場合のみ移動
        if (statusObj.charHp.hp <= 0 || !sr.isVisible)
        {
            GManager.instance.enemyActionEndCount++;
            return;
        }

        isAction = true;
        int xDir;
        int yDir;
        //キャッシュした移動先が存在しない場合は新たに移動先を取得
        if (GManager.instance.charsNextPosition.Count > 0 && nextMovePosition.Count < 1)
        {
            // 次の移動点取得
            nextMovePosition = moveActionComponentObj.retNextPosition(GManager.instance.charsNextPosition[0]);
            // 移動点の取得に失敗
            if (nextMovePosition.Count < 1)
            {
                GManager.instance.enemyActionEndCount++;
                return;
            }
        }
        xDir = (int)(nextMovePosition[0].xDir);
        yDir = (int)(nextMovePosition[0].yDir);
        nextHorizontalKey = xDir;
        nextVerticalkey = yDir;
        Vector2 start = transform.position;
        Vector2 next = start + new Vector2(xDir, yDir);
        //移動先がプレイヤーの移動先と被った場合は攻撃
        if (sensorComponentObj != null && sensorComponentObj.searchTarget(next, GManager.instance.charsNextPosition[0]))
        {
            StartCoroutine(attackComponentObj?.attack(xDir, yDir));
            //attackComponentObj?.attack(xDir, yDir);
            return;
        }
        //移動点が他の敵と被れば移動できない
        if (GManager.instance.charsNextPosition.Contains(next))
        {
            GManager.instance.enemyActionEndCount++;
            return;
        }
        AttemptMove(xDir, yDir);
    }

    /**
     * キャラクターの移動
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        //boxColliderを無効にして、ラインキャストがこのオブジェクト自身のコライダーに当たらないようにする。
        boxCollider.enabled = false;

        //始点から終点までラインをキャストして、blockingLayerの衝突をチェックします。(ここで自分のオブジェクトとの接触判定が出ないようにfalseしている)
        hit = Physics2D.Linecast(transform.position, end, LayerUtil.enemyLayer);

        //ラインキャスト後にboxColliderを再度有効にする
        boxCollider.enabled = true;
        //ヒットしなかった場合は行動開始
        if (hit.transform == null)
        {
            afterComfirmMove(end);
            return;
        }
        Enemy otherEnemy = hit.collider.GetComponent<Enemy>();
        //Enemyコンポーネントの取得に失敗
        if (otherEnemy == null)
        {
            Debug.Log("failed hitname:" + hit.transform.name);
            nextMovePosition.Clear();
            return;
        }
        bool isAbleToMove = false;
        //ヒットした敵が既に行動を終えている場合
        if (otherEnemy.isAction)
        {
            //移動している場合は自身も移動可能
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        else
        {
            //ヒットした敵が行動していない場合は自身より先に行動させる
            otherEnemy.moveEnemy();
            //行動させた敵が移動した場合は自身も移動可能
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        //行動不可
        if (!isAbleToMove)
        {
            GManager.instance.enemyActionEndCount++;
            nextMovePosition.Clear();
            return;
        }
        //移動開始
        afterComfirmMove(end);
    }

    /**
     * 移動確定後の処理
     */
    private  void afterComfirmMove(Vector2 end)
    {
        nextMovePosition.RemoveAt(0);
        GManager.instance.charsNextPosition.Add(end);
        StartCoroutine(enemySmoothMovement(end));
    }

    protected IEnumerator enemySmoothMovement(Vector3 end)
    {
        yield return StartCoroutine(SmoothMovement(end));
        GManager.instance.enemyActionEndCount++;
    }
}