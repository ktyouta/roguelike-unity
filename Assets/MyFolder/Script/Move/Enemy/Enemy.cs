using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    [Header("�G�l�~�[�̍U����")] public int enemyAttackValue;
    [Header("�G�l�~�[�̏�����")] public int enemyMoney;
    [Header("�G�l�~�[��|�����ۂ̌o���l")] public int experiencePoint;
    [Header("�G�l�~�[��HP")] public int enemyHp = 10;
    [Header("NPC���C���[")] public LayerMask npcLayer;
    [HideInInspector] public string enemyName;
    [HideInInspector] public int enemyNumber;            //�G�ɕt�^�����A��
    [HideInInspector] public bool isAction = false;
    private Animator animator;                            //�G��Animator�R���|�[�l���g�ւ̎Q�Ƃ��i�[����Animator�^�̕ϐ��B
    private Transform target;                            //�e�^�[���Ɉړ����悤�Ƃ���ړIobject
    private bool isDefeatEnemy = false;
    private SpriteRenderer sr = null;

    //Start�́A��{�N���X�̉��zStart�֐����I�[�o�[���C�h���܂��B
    protected override void Start()
    {
        // Enemy�I�u�W�F�N�g�̃��X�g�ɒǉ����āA���̓G��GameManager�̃C���X�^���X�ɓo�^���܂��B
        //����ɂ��AGameManager���ړ��R�}���h�𔭍s�ł���悤�ɂȂ�܂��B
        //GManager.instance.AddEnemyToList(this);

        //�Y�t���ꂽAnimator�R���|�[�l���g�ւ̎Q�Ƃ��擾���ĕۑ����܂��B
        animator = GetComponent<Animator>();

        //�^�O���g�p����Player GameObject�������Atransform��ۑ����܂��B
        target = GameObject.FindGameObjectWithTag("Player").transform;

        enemyName = "�G�l�~�[" + (enemyNumber + 1);
        sr = GetComponent<SpriteRenderer>();

        //�X�^�[�g�֐��𒊏ۃN���X����Ă�
        base.Start();
    }

    protected void Update()
    {
        if (enemyHp <= 0 && !isDefeatEnemy)
        {
            enemyDefeat();
        }
    }

    //MoveEnemy�͖��^�[��GameManger�ɂ���ČĂяo����A�e�G�Ƀv���C���[�Ɍ������Ĉړ�����悤�Ɏw�����܂��B
    public void MoveEnemy()
    {
        //��ʓ��ɂ���ꍇ�݈̂ړ�
        if (!sr.isVisible)
        {
            return;
        }
        isAction = true;
        // X����Y���̈ړ������̕ϐ���錾���܂��B�����͈̔͂�-1����1�ł��B
        //�����̒l�ɂ��A��{�I�ȕ����i��A���A���A�E�j��I���ł��܂��B
        int xDir = 0;
        int yDir = 0;

        //x�����C�v�V����(�ق�)�̕����傫���ꍇ
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            //�^�[�Q�b�g�i�v���[���[�j�̈ʒu��y���W�����̓G�̈ʒu��y���W���傫���ꍇ�́Ay����1�i��Ɉړ��j��ݒ肵�܂��B �����łȂ��ꍇ�́A-1�ɐݒ肵�܂��i���Ɉړ����܂��j�B
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        //y���������ꍇ
        else
        {
            //�^�[�Q�b�g��x�ʒu���G��x�ʒu���傫�����ǂ������m�F���܂��B�����ł���΁Ax������1�i�E�Ɉړ��j�ɐݒ肵�A�����łȂ����-1�i���Ɉړ��j�ɐݒ肵�܂��B
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }
        Vector2 start = transform.position;
        Vector2 next = start + new Vector2(xDir, yDir);
        //�ړ��悪�v���C���[�̈ړ���Ɣ�����ꍇ�͍U��
        if (next == GManager.instance.enemyNextPosition[0])
        {
            enemyAttack(xDir, yDir);
            return;
        }
        //�ړ��_�����̓G�Ɣ��Έړ��ł��Ȃ�
        if (checkNextPosition(next))
        {
            return;
        }
        GManager.instance.enemyNextPosition.Add(next);
        AttemptMove(xDir, yDir);
    }

    /**
     * �L�����N�^�[�̈ړ�
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        Enemy otherEnemy;
        //boxCollider�𖳌��ɂ��āA���C���L���X�g�����̃I�u�W�F�N�g���g�̃R���C�_�[�ɓ�����Ȃ��悤�ɂ���B
        boxCollider.enabled = false;

        //�n�_����I�_�܂Ń��C�����L���X�g���āAblockingLayer�̏Փ˂��`�F�b�N���܂��B(�����Ŏ����̃I�u�W�F�N�g�Ƃ̐ڐG���肪�o�Ȃ��悤��false���Ă���)
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | enemyLayer | playerLayer | treasureLayer | npcLayer);

        //���C���L���X�g���boxCollider���ēx�L���ɂ���
        boxCollider.enabled = true;

        //�������q�b�g�������ǂ������m�F���܂�
        if (hit.transform == null)
        {
            //�����q�b�g���Ȃ������ꍇ�́AVector2�G���h������Ƃ��ēn����SmoothMovement�R���[�`�����J�n���܂��B
            StartCoroutine(SmoothMovement(end));
            return;
        }
        //�����ȊO�̓G�Ƀq�b�g�����ꍇ
        if ((otherEnemy = hit.collider.GetComponent<Enemy>()) != null)
        {
            //�q�b�g������̓G���ړ����̏ꍇ
            if (otherEnemy.isMoving)
            {
                StartCoroutine(SmoothMovement(end));
            }
            else
            {
                //�q�b�g�����G�����ɍs�����Ă���ꍇ�͍s�������Ȃ�
                if (otherEnemy.isAction)
                {
                    return;
                }
                otherEnemy.MoveEnemy();
                boxCollider.enabled = false;
                hit = Physics2D.Linecast(transform.position, end,enemyLayer);
                boxCollider.enabled = true;
                otherEnemy = hit.collider.GetComponent<Enemy>();
                //�s���������G���ړ������ꍇ�͎��g���ړ�����
                if (hit.transform == null || (otherEnemy != null && otherEnemy.isAction && otherEnemy.isMoving))
                {
                    StartCoroutine(SmoothMovement(end));
                }
            }
        }
    }

    /**
     * �G�����̈ړ��_�Ɉړ��ł��邩����(���ɑ��̓G�̐�񂪂Ȃ����`�F�b�N)
     */
    protected bool checkNextPosition(Vector2 next)
    {
        for (int i=0;i<GManager.instance.enemyNextPosition.Count;i++)
        {
            if (GManager.instance.enemyNextPosition[i] == next)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * �G�̍U������
     */
    protected void enemyAttack(int x,int y)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(x, y);
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, playerLayer);
        boxCollider.enabled = true;
        if (hit.transform)
        {
            animator.Play("EnemyAttack");
            GManager.instance.playerHp -= enemyAttackValue;
            GManager.instance.wrightAttackLog(enemyName,GManager.instance.playerName,enemyAttackValue);
        }
    }

    /**
     * �G���|���ꂽ���̏���
     */
    protected void enemyDefeat()
    {
        GManager.instance.wrightDeadLog(enemyName);
        isDefeatEnemy = true;
        GManager.instance.playerMoney += enemyMoney;
        GManager.instance.beforeLevelupExperience = GManager.instance.nowExprience;
        GManager.instance.nowExprience += experiencePoint;
        GManager.instance.mostRecentExperience = experiencePoint;
        GManager.instance.removeEnemyToList(enemyNumber);
        Destroy(gameObject, 0.5f);
    }
}