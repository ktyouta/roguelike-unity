using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract������ƁA���ۃN���X�̐錾�ɂȂ�
public abstract class MovingObject : MonoBehaviour
{
    [Header("�u���b�L���O���C���[(���L���C���[�ȊO�Ői�s�s�ɂ���������)")]public LayerMask blockingLayer;  //�Փ˂��`�F�b�N����郌�C���[
    [Header("�G���C���[")] public LayerMask enemyLayer;
    [Header("�v���C���[���C���[")] public LayerMask playerLayer;
    [Header("�`�F�X�g���C���[")] public LayerMask treasureLayer;
    [HideInInspector] public bool isMoving;                    //�����邩�ǂ���
    protected bool canMove;
    protected BoxCollider2D boxCollider;         //���̃I�u�W�F�N�g�ɃA�^�b�`���ꂽ�ABoxCollider2D�̓��ꕨ��p��
    protected Animator animator;
    private Rigidbody2D rb2D;                //���̃I�u�W�F�N�g�ɃA�^�b�`���ꂽ�ARigidbody2D�̓��ꕨ��p��
    private float inverseMoveTime;            //�������������I�ɂ��邽�߂Ɏg�p����܂�
    private float moveTime = 0.075f;            //�I�u�W�F�N�g�̈ړ��ɂ����鎞�ԁi�b�P�ʁj���ŏ��̐ݒ��0.1

    protected virtual void Start()
    {
        //���̃I�u�W�F�N�g��BoxCollider2D�ւ̃R���|�[�l���g�Q�Ƃ��擾���܂�
        boxCollider = GetComponent<BoxCollider2D>();

        //���̃I�u�W�F�N�g��Rigidbody2D�ւ̃R���|�[�l���g�Q�Ƃ��擾���܂�
        rb2D = GetComponent<Rigidbody2D>();

        //�ړ����Ԃ̋t����ۑ����邱�ƂŁA���Z�ł͂Ȃ���Z�Ŏg�p�ł��邽�߁A�������I�ł��B
        inverseMoveTime = 1f / moveTime;
        animator = GetComponent<Animator>();
    }

    /**
     * �L�����N�^�[�̈ړ�
     */
    protected abstract void moveChar(Vector2 end);

    //���j�b�g�����̃X�y�[�X���玟�̃X�y�[�X�Ɉړ����邽�߂̃R���[�`���Bend���g�p���Ĉړ�����w�肵�܂��B
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        isMoving = true;

        //���݂̈ʒu�ƏI���p�����[�^�[�̍���2��̑傫���Ɋ�Â��āA�ړ�����c��̋������v�Z���܂��B
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

        //�ړ������false�ɕύX����
        isMoving = false;
    }


    //���z�L�[���[�h�́A�I�[�o�[���C�h�L�[���[�h���g�p���ăN���X���p�����邱�Ƃ�AttemptMove���I�[�o�[���C�h�ł��邱�Ƃ��Ӗ����܂��B
    //AttemptMove�́A�W�F�l���b�N�p�����[�^�[T�����A�u���b�N���ꂽ�ꍇ(�ړ��ł��Ȃ�)�Ƀ��j�b�g�����삷��R���|�[�l���g�̃^�C�v���w�肵�܂��B
    protected virtual void AttemptMove(int xDir, int yDir)
    {
        //���݈ʒu
        Vector2 start = transform.position;
        //�ړ���̈ʒu
        Vector2 end = start + new Vector2(xDir, yDir);
        moveChar(end);
    }
}