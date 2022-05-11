using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstractをつけると、抽象クラスの宣言になる
public abstract class MovingObject : MonoBehaviour
{
    [Header("ブロッキングレイヤー(下記レイヤー以外で進行不可にしたいもの)")]public LayerMask blockingLayer;  //衝突がチェックされるレイヤー
    [Header("敵レイヤー")] public LayerMask enemyLayer;
    [Header("プレイヤーレイヤー")] public LayerMask playerLayer;
    [Header("チェストレイヤー")] public LayerMask treasureLayer;
    [HideInInspector] public bool isMoving;                    //動けるかどうか
    protected bool canMove;
    protected BoxCollider2D boxCollider;         //このオブジェクトにアタッチされた、BoxCollider2Dの入れ物を用意
    protected Animator animator;
    private Rigidbody2D rb2D;                //このオブジェクトにアタッチされた、Rigidbody2Dの入れ物を用意
    private float inverseMoveTime;            //動きをより効率的にするために使用されます
    private float moveTime = 0.075f;            //オブジェクトの移動にかかる時間（秒単位）※最初の設定は0.1

    protected virtual void Start()
    {
        //このオブジェクトのBoxCollider2Dへのコンポーネント参照を取得します
        boxCollider = GetComponent<BoxCollider2D>();

        //このオブジェクトのRigidbody2Dへのコンポーネント参照を取得します
        rb2D = GetComponent<Rigidbody2D>();

        //移動時間の逆数を保存することで、除算ではなく乗算で使用できるため、より効率的です。
        inverseMoveTime = 1f / moveTime;
        animator = GetComponent<Animator>();
    }

    /**
     * キャラクターの移動
     */
    protected abstract void moveChar(Vector2 end);

    //ユニットを今のスペースから次のスペースに移動するためのコルーチン。endを使用して移動先を指定します。
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        //現在の位置と終了パラメーターの差の2乗の大きさに基づいて、移動する残りの距離を計算します。
        //計算量が少ないため、等級の代わりに平方等級使用。(sqrMagnitudeは返り値をベクトルの二乗にする)
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //残りの移動距離がイプシロン(ほぼゼロ)より大きい間
        while (sqrRemainingDistance > float.Epsilon)
        {
            //newPostionに、移動途中の位置を設定(現在位置、目的位置、呼び出されるごと(1フレーム)に移動する距離)
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //アタッチされたRigidbody2DでMovePositionを呼び出し、それを計算された位置に移動します。
            rb2D.MovePosition(newPostion);

            //移動後の残り距離を再計算します
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //ループを終了するためにsqrRemainingDistanceがゼロに近づくまで戻り、ループする
            yield return null;
        }
        //移動距離がイプシロンより小さくなった時、終了地点まで移動する
        rb2D.MovePosition(end);

        //移動判定をfalseに変更する
        isMoving = false;
    }


    //仮想キーワードは、オーバーライドキーワードを使用してクラスを継承することでAttemptMoveをオーバーライドできることを意味します。
    //AttemptMoveは、ジェネリックパラメーターTを取り、ブロックされた場合(移動できない)にユニットが操作するコンポーネントのタイプを指定します。
    protected virtual void AttemptMove(int xDir, int yDir)
    {
        //現在位置
        Vector2 start = transform.position;
        //移動後の位置
        Vector2 end = start + new Vector2(xDir, yDir);
        moveChar(end);
    }
}