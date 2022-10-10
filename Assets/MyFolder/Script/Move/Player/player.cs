using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    [HideInInspector] public StatusComponentPlayer statusObj;

    [Header("�A�C�e�����C���[")] public LayerMask itemLayer;
    [HideInInspector] public Vector2 playerBeforePosition;
    [HideInInspector] public playerState plState = playerState.Normal;
    [HideInInspector] public float restartLevelDelay = 1f;        //���x�����Ďn������܂ł̕b�P�ʂ̒x�����ԁB(�X�e�[�W�̂���)
    [HideInInspector] public GameObject levelText;
    [HideInInspector] public int nextHorizontalKey = 1;
    [HideInInspector] public int nextVerticalkey = 0;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public Enemy enemyObject;
    [HideInInspector] public int horizontal = 0;
    [HideInInspector] public int vertical = 0;
    [HideInInspector] public bool leftShift = false;
    private Treasure treasureObject;
    private bool isDefeat = false;
    private int reduceFoodCounter = 0;

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
        CheckIfGameOver();

        //�v���C���[�̃^�[���łȂ��A�ړ����A�U�����̓R�}���h���͂��󂯕t���Ȃ�
        if (!GManager.instance.playersTurn || isMoving || isAttack)
        {
            return;
        }
        GManager.instance.isEndPlayerAction = false;
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
            //Debug.Log("inHorizontal" + inHorizontal);
            //Debug.Log("inVertical" + inVertical);
            //�v���[���[���ړ�����������w�肷��p�����[�^�[�Ƃ��āA���������Ɛ��������ɓn���܂��B
            AttemptMove(inHorizontal, inVertical);
        }
        //�U��
        else if (inLeftShift)
        {
            Attack();
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
        yield return SmoothMovement(end);
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
        GManager.instance.enemyNextPosition.Add(end);
        //�v���[���[�̃^�[�����I��������
        GManager.instance.playersTurn = false;
    }

    //Player�ƃg���K�[���Ԃ�������
    //OnTriggerEnter2D�́A�g���K�[�ݒ肵���I�u�W�F�N�g�ƂԂ���ƌĂяo�����
    private void OnTriggerEnter2D(Collider2D other)
    {
        //�Փ˂����g���K�[�̃^�O��Exit�ł��邩�m�F���Ă��������B
        if (other.tag == "Exit")
        {
            //Debug.Log(levelText);
            //levelText.SetActive(true);
            //1�b��Ɏ��̃��x���i�X�e�[�W�j���J�n���邽�߂ɁARestart�֐����Ăяo���܂��B
            Invoke("Restart", restartLevelDelay);

            //���x�����I������̂ŁA�v���[���[�I�u�W�F�N�g�𖳌��ɂ��܂��B
            enabled = false;
        }
    }

    //Restart�͌Ăяo���ꂽ�Ƃ��ɃV�[���������[�h���܂��B
    private void Restart()
    {
        //�Ō�Ƀ��[�h���ꂽ�V�[�������[�h���܂��B���̏ꍇ��Main�A�Q�[�����̗B��̃V�[���ł��B �����āA������u�V���O���v���[�h�Ń��[�h���āA�����̂��̂�u�������܂�
        //���݂̃V�[���̂��ׂẴV�[���I�u�W�F�N�g�����[�h���܂���B
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //SceneManager.LoadScene("Main");
    }


    //LoseFood�́A�G���v���C���[���U�������Ƃ��ɌĂяo����܂��B
    //�����|�C���g�̐����w�肷��p�����[�^�[�������Ƃ�܂��B
    public void LoseFood(int loss)
    {
        //�v���[���[�A�j���[�^�[�̃g���K�[��ݒ肵�āAplayerHit�A�j���[�V�����ɑJ�ڂ��܂��B
        animator.SetTrigger("hit");
        //�Q�[�����I���������ǂ������m�F���܂�
        CheckIfGameOver();
    }


    //CheckIfGameOver�́A�v���[���[���t�[�h�|�C���g�𒴂��Ă��邩�ǂ������`�F�b�N���A����Ȃ��ꍇ�̓Q�[�����I�����܂��B
    private void CheckIfGameOver()
    {
        //�t�[�h�|�C���g�̎c�肪0���Ⴂ�A�܂��͓����ꍇ
        //if (playerStatusObj.playerFoodPoint <= 0 || playerStatusObj.playerHp <= 0)
        if (statusObj.charFood.foodPoint <= 0 || statusObj.charHp.hp <= 0)
        {
            //GManager.instance.wrightDeadLog(playerStatusObj.playerName);
            GManager.instance.wrightDeadLog(statusObj.charName.name);
            //GameManager��GameOver�֐����Ăяo���܂��B
            GManager.instance.GameOver();
            isDefeat = true;
        }
    }

    /**
     * �v���C���[�̍U��
     */
    public void Attack()
    {
        animator.Play("PlayerAttack");
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(nextHorizontalKey, nextVerticalkey);
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer | blockingLayer);
        //�U���̏ꍇ�͌��ݒn��ǉ�
        GManager.instance.enemyNextPosition.Add(transform.position);
        //���ԍ��Ŏ��s�����t���O���I���ɂ��鏈��
        StartCoroutine(waitAttackEnemy());
        isAttack = true;
        //�q�b�g���Ă��Ȃ��ꍇ
        if (hit.transform == null)
        {
            return;
        }
        //�ΏۃI�u�W�F�N�g�̃_���[�W�������s��
        OutAccessComponentBase outAccessObj = hit.transform.gameObject?.GetComponent<OutAccessComponentBase>();
        if (outAccessObj == null)
        {
            return;
        }
        //�_���[�W����
        outAccessObj.callCalculateDamage(statusObj.charAttack.totalAttack, statusObj.charName.name);
    }

    /*
     * �U���R�}���h���͌�Ɉ�莞�ԑ҂��ă^�[�����I������
     */
    private IEnumerator waitAttackEnemy()
    {
        //�U���Ɋւ��Ă͍U����Ƀ^�[�����I������
        yield return new WaitForSeconds(0.3f);
        GManager.instance.isEndPlayerAction = true;
        GManager.instance.playersTurn = false;
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
        if (item.GetComponent<ThrowObject>() == null)
        {
            return;
        }
        if (item.GetComponent<Rigidbody2D>() == null)
        {
            return;
        }
        setPlayerState(playerState.Wait);
        Item tempItem = item.GetComponent<Item>();
        //��������A�C�e���𐶐�
        GameObject newThrownItemObj = Instantiate(item, new Vector3(transform.position.x, transform.position.y, 0.0f), Quaternion.identity) as GameObject;
        newThrownItemObj.SetActive(true);
        newThrownItemObj.GetComponent<Item>().isEnter = true;
        ThrowObject th = newThrownItemObj.GetComponent<ThrowObject>();
        //�v���C���[�������Ă���������Z�b�g
        th.playerHorizontalKey = nextHorizontalKey;
        th.playerVerticalKey = nextVerticalkey;
        //ThrowObject��update������(�A�C�e�����ړ�����)
        th.isThrownObj = true;
        Item throwItem = newThrownItemObj.GetComponent<Item>();
        //�C���x���g���[����폜
        tempItem.deleteSelectedItem(tempItem.id);
        StartCoroutine(movingItem(th));
    }

    /**
     * �A�C�e���𓊂����ۂ̃R���[�`��
     */
    IEnumerator movingItem(ThrowObject th)
    {
        yield return new WaitUntil(() => !th.isThrownObj);
        //�A�C�e������ʊO�ɏo�邩�A��Q���ɓ�����܂ōs���s��
        setPlayerState(playerState.Normal);
        GManager.instance.playersTurn = false;
        yield break;
    }
}