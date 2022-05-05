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
     * NPC�̍s��
     */
    protected void fellowAction()
    {
        //�ړ�
        if (playerObj.isMoving)
        {
            moveNpc();
            return;
        }
        //�U��
        if (playerObj.isAttackEnemey)
        {
            attack();
            return;
        }
    }

    /**
     * �U������
     */
    protected void attack()
    {

    }

    /**
     * �ړ�����
     */
    protected void moveNpc()
    {
        Move(playerPosition);
    }

    protected void Move(Vector2 next)
    {
        StartCoroutine(SmoothMovement(next));
    }

    //���j�b�g�����̃X�y�[�X���玟�̃X�y�[�X�Ɉړ����邽�߂̃R���[�`���Bend���g�p���Ĉړ�����w�肵�܂��B
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //�v�Z�ʂ����Ȃ����߁A�����̑���ɕ��������g�p�B(sqrMagnitude�͕Ԃ�l���x�N�g���̓��ɂ���)
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //�c��̈ړ��������C�v�V����(�قڃ[��)���傫����
        while (sqrRemainingDistance > float.Epsilon)
        {
            //newPostion�ɁA�ړ��r���̈ʒu��ݒ�(���݈ʒu�A�ړI�ʒu�A�Ăяo����邲��(1�t���[��)�Ɉړ����鋗��)
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //�A�^�b�`���ꂽRigidbody2D��MovePosition���Ăяo���A������v�Z���ꂽ�ʒu�Ɉړ����܂��B
            rb2D.MovePosition(newPostion);

            //�ړ���̎c�苗�����Čv�Z���܂�
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //���[�v���I�����邽�߂�sqrRemainingDistance���[���ɋ߂Â��܂Ŗ߂�A���[�v����
            yield return null;
        }
        //�ړ��������C�v�V������菬�����Ȃ������A�I���n�_�܂ňړ�����
        rb2D.MovePosition(end);
        playerPosition = playerObj.transform.position;
    }
}
