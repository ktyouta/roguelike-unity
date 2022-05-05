using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellow : MonoBehaviour
{
    private float moveTime = 0.075f;
    protected player playerObj;
    protected Vector2 playerPosition;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindWithTag("Player").GetComponent<player>();
        playerPosition = playerObj.transform.position;
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * NPCの行動
     */
    protected void fellowAction()
    {
        //移動
        if (playerObj.isMoving)
        {
            moveNpc();
            return;
        }
        //攻撃
        if (playerObj.isAttackEnemey)
        {
            attack();
            return;
        }
    }

    /**
     * 攻撃処理
     */
    protected void attack()
    {

    }

    /**
     * 移動処理
     */
    protected void moveNpc()
    {
        Move(playerPosition);
    }

    protected void Move(Vector2 next)
    {
        StartCoroutine(SmoothMovement(next));
    }

    //ユニットを今のスペースから次のスペースに移動するためのコルーチン。endを使用して移動先を指定します。
    protected IEnumerator SmoothMovement(Vector3 end)
    {
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
        playerPosition = playerObj.transform.position;
    }
}
