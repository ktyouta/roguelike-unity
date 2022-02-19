using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract������ƁA���ۃN���X�̐錾�ɂȂ�
public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.03f;            //�I�u�W�F�N�g�̈ړ��ɂ����鎞�ԁi�b�P�ʁj
    [Header("���C���[�ݒ�")]public LayerMask blockingLayer;            //�Փ˂��`�F�b�N����郌�C���[
    [Header("�G�I�u�W�F�N�g")] public LayerMask enemyLayer;
    [Header("�v���C���[�I�u�W�F�N�g")] public LayerMask playerLayer;

    protected bool canMove;
    protected BoxCollider2D boxCollider;         //���̃I�u�W�F�N�g�ɃA�^�b�`���ꂽ�ABoxCollider2D�̓��ꕨ��p��
    private Rigidbody2D rb2D;                //���̃I�u�W�F�N�g�ɃA�^�b�`���ꂽ�ARigidbody2D�̓��ꕨ��p��
    private float inverseMoveTime;            //�������������I�ɂ��邽�߂Ɏg�p����܂�
    private bool isMoving;                    //�����邩�ǂ���

    //�ی삳�ꂽ���z�֐��́A�N���X���p�����邱�ƂŃI�[�o�[���C�h�ł��܂��B
    protected virtual void Start()
    {
        //���̃I�u�W�F�N�g��BoxCollider2D�ւ̃R���|�[�l���g�Q�Ƃ��擾���܂�
        boxCollider = GetComponent<BoxCollider2D>();

        //���̃I�u�W�F�N�g��Rigidbody2D�ւ̃R���|�[�l���g�Q�Ƃ��擾���܂�
        rb2D = GetComponent<Rigidbody2D>();

        //�ړ����Ԃ̋t����ۑ����邱�ƂŁA���Z�ł͂Ȃ���Z�Ŏg�p�ł��邽�߁A�������I�ł��B
        inverseMoveTime = 1f / moveTime;
    }


    //�ړ��ł���ꍇ��true���A�ړ��ł��Ȃ��ꍇ��false��Ԃ��܂��B
    // Move��x�����Ay�����A�����RaycastHit2D�̃p�����[�^�[�����A�Փ˂��`�F�b�N���܂��B
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //�I�u�W�F�N�g�̊J�n�ʒu��ۑ����܂��B(���݈ʒu)
        Vector2 start = transform.position;

        //Move���Ăяo���Ƃ��ɓn���������p�����[�^�[�Ɋ�Â��ďI���ʒu���v�Z���܂��B�i�ړ���̈ʒu�j
        Vector2 end = start + new Vector2(xDir, yDir);

        //boxCollider�𖳌��ɂ��āA���C���L���X�g�����̃I�u�W�F�N�g���g�̃R���C�_�[�ɓ�����Ȃ��悤�ɂ��܂��B
        boxCollider.enabled = false;

        //�n�_����I�_�܂Ń��C�����L���X�g���āAblockingLayer�̏Փ˂��`�F�b�N���܂��B(�����Ŏ����̃I�u�W�F�N�g�Ƃ̐ڐG���肪�o�Ȃ��悤��false���Ă���)
        hit = Physics2D.Linecast(start, end, blockingLayer | enemyLayer | playerLayer);
        //Debug.Log("start" + start);
        //Debug.Log("end" + end);
        //Debug.Log("hit" + hit);
        if (hit.transform)
        {
            //Debug.Log("obj" + hit.transform.gameObject);
        }
        
        //���C���L���X�g���boxCollider���ēx�L���ɂ��܂�
        boxCollider.enabled = true;

        //�������q�b�g�������ǂ������m�F���܂�
        if (hit.transform == null && !isMoving)
        {
            //�����q�b�g���Ȃ������ꍇ�́AVector2�G���h������Ƃ��ēn����SmoothMovement�R���[�`�����J�n���܂��B
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        //�������q�b�g�����ꍇ�́Afalse��Ԃ��A�s���͂ł��Ȃ�
        return false;
    }


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
        //hit��錾
        RaycastHit2D hit;

        //�ړ������������ꍇ��canMove��true�ɐݒ肵�A���s�����ꍇ��false�ɐݒ肵�܂��B
        canMove = Move(xDir, yDir, out hit);

        //���C���L���X�g�̉e�����󂯂Ă��Ȃ����m�F����
        if (hit.transform == null)
        {
            //�����q�b�g���Ȃ������ꍇ�A����ȏ�AttemptMove�֐��̃R�[�h�����s���܂���B
            return;
        }


        //�q�b�g�����I�u�W�F�N�g�ɐڑ�����Ă���^�C�vT�̃R���|�[�l���g�ւ̃R���|�[�l���g�Q�Ƃ��擾���܂�
        //�G�ɂ̓v���C���[�ƕǁA�v���C���[�ɂ͕ǂƓG�j
        //T hitComponent = hit.transform.GetComponent<T>();

        //canMove��false�ŁAhitComponent��null�ɓ������Ȃ��ꍇ�A�܂�AMovingObject���u���b�N����A���ݍ�p�ł��鉽���Ƀq�b�g�������Ƃ��Ӗ����܂�
        //if (!canMove && hitComponent != null)
        //{
        //    //OnCantMove�֐����Ăяo���A�p�����[�^�Ƃ���hitComponent��n���܂��B
        //    OnCantMove(hitComponent);
        //}
    }
    //OnCantMove�́A�p������N���X�̊֐��ɂ���ăI�[�o�[���C�h����܂��B
    //��Q���ɂԂ���ړ��ł��Ȃ����ɌĂяo��
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}