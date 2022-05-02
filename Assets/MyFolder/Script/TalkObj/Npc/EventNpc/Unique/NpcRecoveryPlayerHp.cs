using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcRecoveryPlayerHp : NpcBase
{
    [Header("HP�񕜌�̃��b�Z�[�W(��x��b������̃��b�Z�[�W)")] public List<string> afterMessage;
    protected bool talkFlag = false;

    public override void Start()
    {
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
            recoveryPlayerHp();
            talkFlag = true;
        }
        for (int i = 0; i < tempMessage.Count; i++)
        {
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return null;
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(tempMessage[i]);
            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }

    /**
     * �v���C���[��HP���񕜂���
     */
    protected void recoveryPlayerHp()
    {
        GManager.instance.playerHp += 10;
    }
}
