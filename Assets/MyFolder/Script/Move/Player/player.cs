using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class player : MovingObject
{
    [Header("�A�C�e�����C���[")] public LayerMask itemLayer;
    [HideInInspector] public Vector2 playerBeforePosition;
    [HideInInspector] public playerState plState = playerState.Normal;
    [HideInInspector] public bool isAttackEnemey = false;
    [HideInInspector] public float restartLevelDelay = 1f;        //���x�����Ďn������܂ł̕b�P�ʂ̒x�����ԁB(�X�e�[�W�̂���)
    [HideInInspector] public GameObject levelText;
    [HideInInspector] public int nextHorizontalKey = 1;
    [HideInInspector] public int nextVerticalkey = 0;
    private Animator animator;                    //�v���[���[�̃A�j���[�^�[�R���|�[�l���g�ւ̎Q�Ƃ��i�[���邽�߂Ɏg�p����܂��B
    private bool isAttack = false;
    private Enemy enemyObject;
    private Treasure treasureObject;
    private bool isDefeat = false;

    public enum playerState
    {
        Normal,
        Talk,
        Command,
        Wait
    }

    protected override void Start()
    {
        //�v���[���[�̃A�j���[�^�[�R���|�[�l���g�ւ̃R���|�[�l���g�Q�Ƃ��擾����
        animator = GetComponent<Animator>();
        //MovingObject��{�N���X��Start�֐����Ăяo���܂��B
        base.Start();
    }


    //���̊֐��́A���삪�����܂��͔�A�N�e�B�u�ɂȂ����Ƃ��ɌĂяo����܂��B�i�G���A�ړ��̎��ɌĂяo�����j
    private void OnDisable()
    {
        //Player�I�u�W�F�N�g�������ɂȂ��Ă���ꍇ�́A���݂̃��[�J���t�[�h�̍��v��GameManager�ɕۑ����āA���̃��x���ōă��[�h�ł���悤�ɂ��܂��B
        //GManager.instance.playerFoodPoints = GManager.instance.playerFoodPoints;
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

        //���x���A�b�v
        if (GManager.instance.nowExprience >= GManager.instance.nowMaxExprience)
        {
            GManager.instance.updateLevel();
            GManager.instance.updateStatus();
        }

        //Debug.Log(GManager.instance.playersTurn);
        //�v���C���[�̔ԂłȂ��ꍇ�A�֐����I�����܂��B
        if (!GManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;      //�����ړ��������i�[���邽�߂Ɏg�p����܂�
        int vertical = 0;        //�����ړ��������i�[���邽�߂Ɏg�p����܂��B
        bool leftShift = false;       //�U��

        //���̓}�l�[�W���[������͂��擾���A�����Ɋۂ߁A�����ɕۑ�����x���̈ړ�������ݒ肵�܂�
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //���̓}�l�[�W���[������͂��擾���A�����Ɋۂ߁A�����ɕۑ�����y���̈ړ�������ݒ肵�܂�
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //�U��
        leftShift = Input.GetKey("left shift");

        //�����Ɉړ����邩�ǂ������m�F���A�ړ�����ꍇ�͐����Ƀ[���ɐݒ肵�܂��B(�Y���h�~)
        if (horizontal != 0)
        {
            vertical = 0;
            nextHorizontalKey = horizontal > 0 ?1:-1;
            nextVerticalkey = 0;
        }
        else if (vertical != 0)
        {
            horizontal = 0;
            nextVerticalkey = vertical > 0 ?1:-1;
            nextHorizontalKey = 0;
        }
        //�����܂��͐����Ƀ[���ȊO�̒l�����邩�ǂ������m�F���܂�
        if (horizontal != 0 || vertical != 0)
        {
            //�v���[���[���ړ�����������w�肷��p�����[�^�[�Ƃ��āA���������Ɛ��������ɓn���܂��B
            AttemptMove(horizontal, vertical);
        }
        else if (leftShift && !isAttack)
        {
            Attack();
            isAttack = true;
        }
        else if (!leftShift)
        {
            isAttack = false;
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
        //boxCollider�𖳌��ɂ��āA���C���L���X�g�����̃I�u�W�F�N�g���g�̃R���C�_�[�ɓ�����Ȃ��悤�ɂ��܂��B
        boxCollider.enabled = false;

        //�n�_����I�_�܂Ń��C�����L���X�g���āAblockingLayer�̏Փ˂��`�F�b�N���܂��B(�����Ŏ����̃I�u�W�F�N�g�Ƃ̐ڐG���肪�o�Ȃ��悤��false���Ă���)
        hit = Physics2D.Linecast(transform.position, end, blockingLayer | enemyLayer | treasureLayer);

        //���C���L���X�g���boxCollider���ēx�L���ɂ��܂�
        boxCollider.enabled = true;

        //�q�b�g�����ꍇ�͈ړ��s��
        if (hit.transform != null)
        {
            return;
        }
        //SmoothMovement�R���[�`�����J�n
        StartCoroutine(SmoothMovement(end));
        //�ړ���̏���
        playerMoved(end);
    }

    /**
     * �ړ���̏���(�����x�̌��Z��)
     */
    private void playerMoved(Vector2 end)
    {
        //���Ԃ�NPC�����݂���ꍇ�̓v���C���[�̈ړ��O�̈ʒu��ۑ�����
        if (GManager.instance.fellows.Count > 0)
        {
            playerBeforePosition = transform.position;
        }
        //�v���C���[���ړ����邽�тɁA�t�[�h�|�C���g�̍��v���猸�Z
        GManager.instance.playerFoodPoint--;

        //�v���C���[���ړ����ăt�[�h�|�C���g���������̂ŁA�Q�[�����I���������ǂ������m�F�B
        CheckIfGameOver();
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
        if (GManager.instance.playerFoodPoint <= 0 || GManager.instance.playerHp <= 0)
        {
            GManager.instance.wrightDeadLog(GManager.instance.playerName);
            //GameManager��GameOver�֐����Ăяo���܂��B
            GManager.instance.GameOver();
            isDefeat = true;
        }
    }

    /**
     * �U��
     */
    protected void Attack()
    {
        animator.Play("PlayerAttack");
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(nextHorizontalKey, nextVerticalkey);
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer);
        boxCollider.enabled = true;
        if (hit.transform)
        {
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.layer == Define.ENEMY_LAYER)
            {
                enemyObject = hitObj.GetComponent<Enemy>();
                enemyObject.enemyHp -= GManager.instance.playerAttack;
                GManager.instance.wrightAttackLog(GManager.instance.playerName, enemyObject.enemyName, GManager.instance.playerAttack);
            }
            else if (hit.collider.gameObject.layer == Define.TREASURE_LAYER)
            {
                treasureObject = hitObj.GetComponent<Treasure>();
                treasureObject.treasureHp -= GManager.instance.playerAttack;                
            }
        }
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
        GameObject newPutItem = Instantiate(item, new Vector3(transform.position.x,transform.position.y, 0.0f), Quaternion.identity) as GameObject;
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
            Debug.Log("�I�u�W�F�N�g��ThrowObject�����Ă��܂���");
            return;
        }
        if (item.GetComponent<Rigidbody2D>() == null)
        {
            Debug.Log("�I�u�W�F�N�g��Rigidbody2D�����Ă��܂���");
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