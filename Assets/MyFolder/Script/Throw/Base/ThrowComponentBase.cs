using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowComponentBase : MonoBehaviour
{
    protected Animator animator;
    [HideInInspector] public float itemXSpeed = 10.0f;
    [HideInInspector] public float itemYSpeed = 10.0f;
    protected BoxCollider2D boxCollider;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    /**
     * オブジェクトを投げる
     */
    public IEnumerator throwObject(int verticalDirection,int horizontalDirection, GameObject item)
    {
        if (item.GetComponent<ThrowObject>() == null)
        {
            item.AddComponent<ThrowObject>();
        }
        if (item.GetComponent<Rigidbody2D>() == null)
        {
            item.AddComponent<Rigidbody2D>();
        }
        //キャラクター自身にヒットしないように設定
        boxCollider.enabled = false;
        //キャラクタの向きから速度用ベクトルを取得
        float xSpeed = 0.0f;
        float ySpeed = 0.0f;
        if (horizontalDirection > 0)
        {
            xSpeed = itemXSpeed;
        }
        else if (horizontalDirection < 0)
        {
            xSpeed = (-1) * itemXSpeed;
        }
        if (verticalDirection > 0)
        {
            ySpeed = itemYSpeed;
        }
        else if (verticalDirection < 0)
        {
            ySpeed = (-1) * itemYSpeed;
        }
        //アイテムの速度をセット
        Vector2 velocityVector = new Vector2(xSpeed, ySpeed);
        Item tempItem = item.GetComponent<Item>();
        //投擲用のアイテムを生成
        GameObject newThrownItemObj = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newThrownItemObj.SetActive(true);
        newThrownItemObj.GetComponent<Item>().isEnter = true;
        ThrowObject throwObj = newThrownItemObj.GetComponent<ThrowObject>();
        //ベクトルをセット
        throwObj.rb.velocity = velocityVector;
        //ThrowObjectのupdateが走る(アイテムが移動する)
        throwObj.isThrownObj = true;
        Item throwItem = newThrownItemObj.GetComponent<Item>();
        yield return StartCoroutine(throwAction(tempItem, throwObj));
        boxCollider.enabled = true;
    }

    /**
     * アイテムを投げた際の処理(キャラごとの固有処理)
     */
    public abstract IEnumerator throwAction(Item item, ThrowObject throwObj);
}
