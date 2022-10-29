using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    private player playerObj;
    private Button attackButton;
    private AttackComponentBase attackComponentObj;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        attackButton = GetComponent<Button>();
        attackButton.onClick.AddListener(attackAction);
        attackComponentObj = playerObj.GetComponent<AttackComponentBase>();
    }

    /**
     * �U���{�^������
     */
    private void attackAction()
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
        //playerObj.Attack();
        attackComponentObj.attack(playerObj.nextHorizontalKey,playerObj.nextVerticalkey);
    }
}
