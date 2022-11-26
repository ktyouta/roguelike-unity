using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using static MoveActionComponentBase;

public class Enemy : MovingObject
{
    [HideInInspector] public bool isAction = false;
    [HideInInspector] public SpriteRenderer sr = null;
    protected StatusComponentBase statusObj;
    //���̈ړ��_
    List<NextMovePositionClass> nextMovePosition = new List<NextMovePositionClass>();

    //�@�\�R���|�[�l���g
    //�U���A�N�V����
    [HideInInspector] public AttackComponentBase attackComponentObj;
    //�ړ��A�N�V����
    [HideInInspector] public MoveActionComponentBase moveActionComponentObj;
    //�Z���T�[
    [HideInInspector] public SensorComponentBase sensorComponentObj;


    //Start�́A��{�N���X�̉��zStart�֐����I�[�o�[���C�h���܂��B
    protected override void Start()
    {
        //�X�^�[�g�֐��𒊏ۃN���X����Ă�
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        this.statusObj = GetComponent<StatusComponentBase>();
        if (this.statusObj == null)
        {
            Destroy(gameObject);
            GManager.instance.removeEnemyToList(GetComponent<Enemy>());
        }
        //�@�\�R���|�[�l���g���擾
        //�U���A�N�V�����R���|�[�l���g
        attackComponentObj = GetComponent<AttackComponentBase>();
        if (attackComponentObj == null)
        {
            gameObject.AddComponent<AttackComponentEnemy>();
            attackComponentObj = GetComponent<AttackComponentBase>();
        }
        //�ړ��_�擾�R���|�[�l���g
        moveActionComponentObj = GetComponent<MoveActionComponentBase>();
        if (moveActionComponentObj == null)
        {
            gameObject.AddComponent<MoveActionComponentAstar>();
            moveActionComponentObj = GetComponent<MoveActionComponentAstar>();
        }
        //�Z���T�[�R���|�[�l���g
        sensorComponentObj = GetComponent<SensorComponentBase>();
        if (sensorComponentObj == null)
        {
            gameObject.AddComponent<SensorComponentNext>();
            sensorComponentObj = GetComponent<SensorComponentBase>();
        }
    }

    //moveEnemy�͖��^�[��GameManger�ɂ���ČĂяo����A�e�G�Ƀv���C���[�Ɍ������Ĉړ�����悤�Ɏw�����܂��B
    public void moveEnemy()
    {
        //�|����Ă��Ȃ��܂��͉�ʓ��ɂ���ꍇ�݈̂ړ�
        if (statusObj.charHp.hp <= 0 || !sr.isVisible)
        {
            GManager.instance.enemyActionEndCount++;
            return;
        }

        isAction = true;
        int xDir;
        int yDir;
        //�L���b�V�������ړ��悪���݂��Ȃ��ꍇ�͐V���Ɉړ�����擾
        if (GManager.instance.charsNextPosition.Count > 0 && nextMovePosition.Count < 1)
        {
            // ���̈ړ��_�擾
            nextMovePosition = moveActionComponentObj.retNextPosition(GManager.instance.charsNextPosition[0]);
            // �ړ��_�̎擾�Ɏ��s
            if (nextMovePosition.Count < 1)
            {
                GManager.instance.enemyActionEndCount++;
                return;
            }
        }
        xDir = (int)(nextMovePosition[0].xDir);
        yDir = (int)(nextMovePosition[0].yDir);
        Vector2 start = transform.position;
        Vector2 next = start + new Vector2(xDir, yDir);
        //�ړ��悪�v���C���[�̈ړ���Ɣ�����ꍇ�͍U��
        if (sensorComponentObj != null && sensorComponentObj.searchTarget(next, GManager.instance.charsNextPosition[0]))
        {
            attackComponentObj?.attack(xDir, yDir);
            return;
        }
        //�ړ��_�����̓G�Ɣ��Έړ��ł��Ȃ�
        if (GManager.instance.charsNextPosition.Contains(next))
        {
            GManager.instance.enemyActionEndCount++;
            return;
        }
        AttemptMove(xDir, yDir);
    }

    /**
     * �L�����N�^�[�̈ړ�
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        //boxCollider�𖳌��ɂ��āA���C���L���X�g�����̃I�u�W�F�N�g���g�̃R���C�_�[�ɓ�����Ȃ��悤�ɂ���B
        boxCollider.enabled = false;

        //�n�_����I�_�܂Ń��C�����L���X�g���āAblockingLayer�̏Փ˂��`�F�b�N���܂��B(�����Ŏ����̃I�u�W�F�N�g�Ƃ̐ڐG���肪�o�Ȃ��悤��false���Ă���)
        hit = Physics2D.Linecast(transform.position, end, LayerUtil.enemyLayer);

        //���C���L���X�g���boxCollider���ēx�L���ɂ���
        boxCollider.enabled = true;
        //�q�b�g���Ȃ������ꍇ�͍s���J�n
        if (hit.transform == null)
        {
            afterComfirmMove(end);
            return;
        }
        Enemy otherEnemy = hit.collider.GetComponent<Enemy>();
        //Enemy�R���|�[�l���g�̎擾�Ɏ��s
        if (otherEnemy == null)
        {
            Debug.Log("failed hitname:" + hit.transform.name);
            nextMovePosition.Clear();
            return;
        }
        bool isAbleToMove = false;
        //�q�b�g�����G�����ɍs�����I���Ă���ꍇ
        if (otherEnemy.isAction)
        {
            //�ړ����Ă���ꍇ�͎��g���ړ��\
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        else
        {
            //�q�b�g�����G���s�����Ă��Ȃ��ꍇ�͎��g����ɍs��������
            otherEnemy.moveEnemy();
            //�s���������G���ړ������ꍇ�͎��g���ړ��\
            if (otherEnemy.isMoving)
            {
                isAbleToMove = true;
            }
        }
        //�s���s��
        if (!isAbleToMove)
        {
            GManager.instance.enemyActionEndCount++;
            nextMovePosition.Clear();
            return;
        }
        //�ړ��J�n
        afterComfirmMove(end);
    }

    /**
     * �ړ��m���̏���
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