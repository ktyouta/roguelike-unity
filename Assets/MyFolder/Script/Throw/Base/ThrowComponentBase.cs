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
     * �I�u�W�F�N�g�𓊂���
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
        //�L�����N�^�[���g�Ƀq�b�g���Ȃ��悤�ɐݒ�
        boxCollider.enabled = false;
        //�L�����N�^�̌������瑬�x�p�x�N�g�����擾
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
        //�A�C�e���̑��x���Z�b�g
        Vector2 velocityVector = new Vector2(xSpeed, ySpeed);
        Item tempItem = item.GetComponent<Item>();
        //�����p�̃A�C�e���𐶐�
        GameObject newThrownItemObj = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newThrownItemObj.SetActive(true);
        newThrownItemObj.GetComponent<Item>().isEnter = true;
        ThrowObject throwObj = newThrownItemObj.GetComponent<ThrowObject>();
        //�x�N�g�����Z�b�g
        throwObj.rb.velocity = velocityVector;
        //ThrowObject��update������(�A�C�e�����ړ�����)
        throwObj.isThrownObj = true;
        Item throwItem = newThrownItemObj.GetComponent<Item>();
        yield return StartCoroutine(throwAction(tempItem, throwObj));
        boxCollider.enabled = true;
    }

    /**
     * �A�C�e���𓊂����ۂ̏���(�L�������Ƃ̌ŗL����)
     */
    public abstract IEnumerator throwAction(Item item, ThrowObject throwObj);
}
