using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    // �v���C���[�̃X�e�[�^�X�p�I�u�W�F�N�g
    [HideInInspector] public StatusComponentPlayer statusObj;

    [Header("�A�C�e�����C���[")] public LayerMask itemLayer;
    [HideInInspector] public Vector2 playerBeforePosition;
    [HideInInspector] public playerState plState = playerState.Normal;
    [HideInInspector] public float restartLevelDelay = 1f;        //���x�����Ďn������܂ł̕b�P�ʂ̒x�����ԁB(�X�e�[�W�̂���)
    [HideInInspector] public GameObject levelText;
    [HideInInspector] public int nextHorizontalKey;
    [HideInInspector] public int nextVerticalkey;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public Enemy enemyObject;
    [HideInInspector] public int horizontal = 0;
    [HideInInspector] public int vertical = 0;
    [HideInInspector] public bool leftShift = false;
    private Treasure treasureObject;
    private bool isDefeat = false;
    private int reduceFoodCounter = 0;

    // �@�\�R���|�[�l���g
    [HideInInspector] public AttackComponentBase attackComponentObj;
    [HideInInspector] public ThrowComponentBase throwComponentObj;

    public enum playerState
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    //���̊֐��́A���삪�����܂��͔�A�N�e�B�u�ɂȂ����Ƃ��ɌĂяo����܂��B�i�G���A�ړ��̎��ɌĂяo�����j
    private void OnDisable()
    {
        //Player�I�u�W�F�N�g�������ɂȂ��Ă���ꍇ�́A���݂̃��[�J���t�[�h�̍��v��GameManager�ɕۑ����āA���̃��x���ōă��[�h�ł���悤�ɂ��܂��B
        //GManager.instance.playerFoodPoints = GManager.instance.playerFoodPoints;
    }

    protected override void Start()
    {
        base.Start();
        // 1F�̎��̂ݎ擾���A�ȍ~�͑O�̃V�[����������p��
        setPlayerStatus();
        // �U���p�R���|�[�l���g
        attackComponentObj = GetComponent<AttackComponentBase>();
        // �A�C�e���̓����p�R���|�[�l���g
        throwComponentObj = GetComponent<ThrowComponentBase>();
        nextHorizontalKey = 0;
        nextVerticalkey = -1;
    }

    /**
     * �L�����N�^�[�̃X�e�[�^�X���擾
     */
    protected virtual void setPlayerStatus()
    {
        // �L���X�g����^���L�����N�^�[���Ƃɕς���
        statusObj = (StatusComponentPlayer)GetComponent<StatusComponentBase>();
    }

    private void Update()
    {
        CheckIfGameOver();

        if (!isAttack)
        {
            if (nextHorizontalKey > 0)
            {
                animator?.Play("PlayerRightWalk");
            }
            else if (nextHorizontalKey < 0)
            {
                animator?.Play("PlayerLeftWalk");
            }
            else if (nextVerticalkey > 0)
            {
                animator?.Play("PlayerUpWalk");
            }
            else if (nextVerticalkey < 0)
            {
                animator?.Play("PlayerDownWalk");
            }
        }
        
        horizontal = 0;      //�����ړ��������i�[���邽�߂Ɏg�p����܂�
        vertical = 0;        //�����ړ��������i�[���邽�߂Ɏg�p����܂��B
        leftShift = false;       //�U��

        //���̓}�l�[�W���[������͂��擾���A�����Ɋۂ߁A�����ɕۑ�����x���̈ړ�������ݒ肵�܂�
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        //���̓}�l�[�W���[������͂��擾���A�����Ɋۂ߁A�����ɕۑ�����y���̈ړ�������ݒ肵�܂�
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        //�U��
        leftShift = Input.GetKeyDown("left shift");
        //�v���C���[���s��������
        playerMove(horizontal,vertical,leftShift);
    }

    public void playerMove(int inHorizontal,int inVertical,bool inLeftShift)
    {
        //�v���C���[�̏�Ԃ��ʏ�ȊO
        if (plState != playerState.Normal)
        {
            return;
        }
        //�Q�[���I�[�o�[�܂��̓��j���[�I�[�v����
        if (isDefeat || !GManager.instance.isCloseCommand)
        {
            return;
        }
        //�v���C���[�̃^�[���łȂ��A�ړ����A�U�����͍s���s��
        if (!GManager.instance.playersTurn || isMoving || isAttack)
        {
            return;
        }

        GManager.instance.isEndPlayerAction = false;
        //�����Ɉړ����邩�ǂ������m�F���A�ړ�����ꍇ�͐����Ƀ[���ɐݒ肵�܂��B(�Y���h�~)
        if (inHorizontal != 0)
        {
            nextHorizontalKey = inHorizontal > 0 ? 1 : -1;
            nextVerticalkey = 0;
        }
        else if (inVertical != 0)
        {
            nextVerticalkey = inVertical > 0 ? 1 : -1;
            nextHorizontalKey = 0;
        }
        //�����܂��͐����Ƀ[���ȊO�̒l�����邩�ǂ������m�F���܂�
        if (inHorizontal != 0 || inVertical != 0)
        {
            //�v���[���[���ړ�����������w�肷��p�����[�^�[�Ƃ��āA���������Ɛ��������ɓn���܂��B
            AttemptMove(inHorizontal, inVertical);
        }
        //�U��
        else if (inLeftShift)
        {
            attackComponentObj?.attack(nextHorizontalKey, nextVerticalkey);
        }
    }

    /**
     * �v���C���[�̏�Ԃ��X�V
     */
    public void setPlayerState(playerState state)
    {
        plState = state;
    }

    /**
     * �L�����N�^�[�̈ړ�
     */
    protected override void moveChar(Vector2 end)
    {
        RaycastHit2D hit;
        //�n�_����I�_�܂Ń��C�����L���X�g���āAblockingLayer�̏Փ˂��`�F�b�N���܂��B(�����Ŏ����̃I�u�W�F�N�g�Ƃ̐ڐG���肪�o�Ȃ��悤��false���Ă���)
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | enemyLayer | treasureLayer | npcLayer);
        //�q�b�g�����ꍇ�͈ړ��s��
        if (hit.transform != null)
        {
            return;
        }
        //SmoothMovement�R���[�`�����J�n
        StartCoroutine(playerSmoothMovement(end));
        //�ړ���̏���
        playerMoved(end);
    }

    private IEnumerator playerSmoothMovement(Vector2 end)
    {
        yield return StartCoroutine(SmoothMovement(end));
        //�ړ�������Ƀt���O���I���ɂ���
        GManager.instance.isEndPlayerAction = true;
    }

    /**
     * �ړ���̏���(�����x�̌��Z��)
     * ��playerSmoothMovement�̃R���[�`���Ɠ����Ɏ��s�����
     */
    private void playerMoved(Vector2 end)
    {
        //�v���C���[�̈ړ��O�̈ʒu��ۑ�����
        playerBeforePosition = transform.position;
        reduceFoodCounter++;
        //�v���C���[���ړ����邽�тɁA�t�[�h�|�C���g�̍��v���猸�Z
        if (reduceFoodCounter == 5)
        {
            statusObj.charFood.subFoodPoint(1);
            reduceFoodCounter = 0;
        }

        //�v���C���[���ړ����ăt�[�h�|�C���g���������̂ŁA�Q�[�����I���������ǂ������m�F�B
        CheckIfGameOver();
        //�v���C���[�̈ʒu���͕K�����X�g�̐擪�ɂȂ�
        GManager.instance.charsNextPosition.Add(end);
        //�v���[���[�̃^�[�����I��������
        GManager.instance.playersTurn = false;
    }

    //CheckIfGameOver�́A�v���[���[���t�[�h�|�C���g�𒴂��Ă��邩�ǂ������`�F�b�N���A����Ȃ��ꍇ�̓Q�[�����I�����܂��B
    private void CheckIfGameOver()
    {
        //�t�[�h�|�C���g�̎c�肪0���Ⴂ�A�܂��͓����ꍇ
        //if (playerStatusObj.playerFoodPoint <= 0 || playerStatusObj.playerHp <= 0)
        if (statusObj.charFood.foodPoint <= 0 || statusObj.charHp.hp <= 0)
        {
            GManager.instance.wrightLog(GManager.instance.messageManager.createMessage("6", statusObj.charName.name));
            GManager.instance.GameOver();
            isDefeat = true;
        }
    }

    /**
     * �A�C�e�����g�p
     */
    public void useItem(GameObject item)
    {
        item.GetComponent<Item>().useItem();
    }

    /**
     * �A�C�e�����v���C���[�̑����ɒu��
     */
    public void putItemFloor(GameObject item)
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position, itemLayer);
        if (hit.transform != null)
        {
            GManager.instance.wrightLog("�A�C�e����u���܂���B");
            return;
        }
        Item tempItem = item.GetComponent<Item>();
        GameObject newPutItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newPutItem.SetActive(true);
        newPutItem.GetComponent<Item>().isEnter = true;
        newPutItem.GetComponent<Item>().isPut = true;
        tempItem.deleteSelectedItem(tempItem.id);
    }

    /**
     * �A�C�e���𓊂���
     */
    public void throwItem(GameObject item)
    {
        StartCoroutine(throwComponentObj.throwObject(nextVerticalkey, nextHorizontalKey, item));
    }
}