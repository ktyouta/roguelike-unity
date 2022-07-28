using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private player playerObj;
    private bool isPushTop = false;
    private bool isPushBottom = false;
    private bool isPushRight = false;
    private bool isPushLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̏�Ԃ��ʏ�ȊO
        if (playerObj.plState != player.playerState.Normal)
        {
            return;
        }
        //�v���C���[�̃^�[���łȂ��A�ړ����A�U�����̓R�}���h���͂��󂯕t���Ȃ�
        if (!GManager.instance.playersTurn || playerObj.isMoving || playerObj.isAttack)
        {
            return;
        }
        if (isPushTop)
        {
            playerObj.playerMove(0, 1, false);
        }
        else if (isPushBottom)
        {
            playerObj.playerMove(0, -1, false);
        }
        else if (isPushRight)
        {
            playerObj.playerMove(1, 0, false);
        }
        else if (isPushLeft)
        {
            playerObj.playerMove(-1, 0, false);
        }
    }

    /**
     * �v���C���[��������Ɉړ�������
     */
    public void clickTopArrowBtn()
    {
        isPushTop = true;
    }

    /**
     * �v���C���[���������Ɉړ�������
     */
    public void clickBottomArrowBtn()
    {
        isPushBottom = true;
    }

    /**
     * �v���C���[���E�����Ɉړ�������
     */
    public void clickRightArrowBtn()
    {
        isPushRight = true;
    }

    /**
     * �v���C���[���������Ɉړ�������
     */
    public void clickLeftArrowBtn()
    {
        isPushLeft = true;
    }

    /**
     * �{�^���̃N���b�N���I��
     */
    public void buttonUp()
    {
        isPushTop = false;
        isPushBottom = false;
        isPushRight = false;
        isPushLeft = false;
    }
}
