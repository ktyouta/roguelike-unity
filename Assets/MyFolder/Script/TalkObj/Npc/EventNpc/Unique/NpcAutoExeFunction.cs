using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAutoExeFunction : NpcBase
{
    public enum ExeAutoNum
    {
        RecoveryPlayerHp, 
        skeltonEnemyAppearance,
        ghostEnemyAppearance
    }
    [Header("��x��b������̃��b�Z�[�W")] public List<string> afterMessage;
    [Header("��b�̍ۂɎ��s����֐����w�肷��ϐ�")] public ExeAutoNum exeFuncNum;
    protected bool talkFlag = false;
    private EventManager eManager;

    public override void Start()
    {
        eManager = GManager.instance.GetComponent<EventManager>();
        base.Start();
    }

    protected override IEnumerator TalkEvent()
    {
        List<string> tempMessage;
        if (talkFlag)
        {
            tempMessage = afterMessage;
        }
        else
        {
            tempMessage = messages;
            autoExeFunction();
            talkFlag = true;
        }
        for (int i = 0; i < tempMessage.Count; i++)
        {
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return null;
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(tempMessage[i]);
            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }

    /**
     * ��b���Ɏ��s����֐�
     */
    private void autoExeFunction()
    {
        switch(exeFuncNum)
        {
            case ExeAutoNum.RecoveryPlayerHp:
                recoveryPlayerHp();
                break;
            case ExeAutoNum.skeltonEnemyAppearance:
                onSkeltonAppearanceEnemyFlg();
                break;
            case ExeAutoNum.ghostEnemyAppearance:
                onGhostAppearanceEnemyFlg();
                break;
            default:
                break;
        }
    }

    /**
     * �v���C���[��HP���񕜂���
     */
    protected void recoveryPlayerHp()
    {
        //GManager.instance.playerHp += 10;
    }

    /**
     *�G(�[��)�̏o���p�t���O���I���ɂ���
     */
    private void onSkeltonAppearanceEnemyFlg()
    {
        eManager.skeltonAppearanceFlg = true;
    }

    /**
     *�G(�S�[�X�g)�̏o���p�t���O���I���ɂ���
     */
    private void onGhostAppearanceEnemyFlg()
    {
        eManager.ghostAppearanceFlg = true;
    }
}
