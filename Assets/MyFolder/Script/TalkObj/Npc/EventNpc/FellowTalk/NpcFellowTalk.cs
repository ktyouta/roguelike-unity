using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class NpcFellowTalk : NpcChoices
{
    [Header("���Ԃɂ���̂ɕK�v�Ȗ��͓x")] public int requiredCharm;
    protected override void selectMessage(int funcNumber)
    {
        int tempNowIndex = funcNumber;
        switch (nowNodeIndex)
        {
            //���Ԃ����ւ���ۂ̃m�[�h
            case -98:
                replaceNpc(funcNumber);
                tempNowIndex = -98;
                break;
            //���Ԃɂł��������𒴂��Ă���ۂ̃m�[�h
            case -100:
                switch (funcNumber)
                {
                    //����ւ���
                    case -100:
                        List<choiseMessageFuncClass> tempChoiseMessages = new List<choiseMessageFuncClass>();
                        //NPC�̖��O��ID�ŐV���ɑI�������쐬
                        for (int i=0;i<GManager.instance.fellows.Count;i++)
                        {
                            tempChoiseMessages.Add(new choiseMessageFuncClass() { message = GManager.instance.fellows[i].npcName, funcNumber = GManager.instance.fellows[i].npcId});
                        }
                        ChoiseMessageClass tempChoiseMessage = new ChoiseMessageClass();
                        tempChoiseMessage.choiseMessages = tempChoiseMessages;
                        tempChoiseMessage.messageNodeNumber = -98;
                        tempChoiseMessage.choiseType = ChoiseMessageTypeList.dynamicChoise;
                        ChioseMessageBlockClass tempChoiseMessageBlock = new ChioseMessageBlockClass();
                        tempChoiseMessageBlock.choiseMessage.Add(tempChoiseMessage);
                        choiseMessageBlock.Add(tempChoiseMessageBlock);
                        tempNowIndex = -98;
                        break;
                    case -101:
                        tempNowIndex = -97;
                        break;
                    default:
                        break;
                }
                break;
            default:
                switch (funcNumber)
                {
                    //���Ԃɂ���
                    case 0:
                        int tempRequiredCharm = requiredCharm == 0 ? 1 : requiredCharm;
                        //���Ԃɂł��������𒴂��Ă���
                        if (GManager.instance.fellows.Count > Common.Define.FELLOWS_MAXNUM)
                        {
                            tempNowIndex = -100;
                        }
                        //���Ԃɂ��邽�߂̖��͂�����Ȃ�
                        else if (GManager.instance.playerCharm < tempRequiredCharm)
                        {
                            tempNowIndex = -99;
                        }
                        else
                        {
                            GManager.instance.playerCharm -= tempRequiredCharm;
                            StartCoroutine(becomeFellow());
                        }
                        break;
                    case 1:
                        break;
                    default:
                        break;
                }
                break;
        }
        //���b�Z�[�W�m�[�h�̐؂�ւ�
        nowNodeIndex = tempNowIndex;
        //�I����������Ɏ~�߂Ă�������(�R���[�`��)��i�߂�
        clickChioseButtonFlag = true;
    }

    /*
     * �v���C���[�̒��ԂɂȂ�
     */
    protected IEnumerator becomeFellow()
    {
        yield return new WaitUntil(() => isEndTalk);
        NpcFellowTalk npcFellowTalk = GetComponent<NpcFellowTalk>();
        NpcFellow npcFellow = GetComponent<NpcFellow>();
        settingFellow(npcFellowTalk, npcFellow);
        //NPC�p�̃��X�g�ɒǉ�
        GManager.instance.fellows.Add(npcFellow);
        //NPC(���ԂɂȂ�����p)�̃R���|�[�l���g��L���ɂ���
        npcFellow.enabled = true;
        Transform gameTransform = this.gameObject.transform;
        settingChildCollider();
        npcFellowTalk.enabled = false;
    }

    /**
     * ���Ԃɂ���NPC�̐ݒ�
     */
    protected void settingFellow(NpcFellowTalk npcFellowTalk, NpcFellow npcFellow)
    {
        npcFellow.npcName = npcFellowTalk.npcName;
        npcFellow.npcId = GManager.instance.latestNpcId;
        GManager.instance.latestNpcId++;
    }

    /**
     * NPC�̓���ւ�
     */
    protected void replaceNpc(int targetNpcId)
    {
        for (int i=0;i<GManager.instance.fellows.Count;i++)
        {
            //�I������NPC�Ɠ���ւ�
            if (targetNpcId == GManager.instance.fellows[i].npcId)
            {
                NpcFellowTalk npcFellowTalk = GetComponent<NpcFellowTalk>();
                NpcFellow npcFellow = GetComponent<NpcFellow>();
                settingFellow(npcFellowTalk, npcFellow);
                //NPC(����ւ��Ώ�)����b�p�ɖ߂�
                GManager.instance.fellows[i].GetComponent<NpcFellowTalk>().enabled = true;
                Transform gameTransform = GManager.instance.fellows[i].transform;
                //�q�I�u�W�F�N�g�̃R���C�_�[��L���ɂ���
                foreach (Transform child in gameTransform)
                {
                    BoxCollider2D col = child.GetComponent<BoxCollider2D>();
                    if (col != null)
                    {
                        col.enabled = true;
                    }
                }
                GManager.instance.fellows[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
                GManager.instance.fellows[i].enabled = false;
                //�ʒu�̓���ւ�
                GManager.instance.fellows[i].transform.position = npcFellowTalk.transform.position;
                //���X�g�̓���ւ�
                GManager.instance.fellows[i] = npcFellow;
                settingChildCollider();
                //NPC(���ԂɂȂ�����p)�̃R���|�[�l���g��L���ɂ���
                npcFellow.enabled = true;
                break;
            }
        }
    }

    /**
     * �q�I�u�W�F�N�g�̃R���C�_�[�̖������ƃ��C���[�̕ύX
     */
    protected void settingChildCollider()
    {
        Transform gameTransform = this.gameObject.transform;
        //�q�I�u�W�F�N�g�̃R���C�_�[�𖳌��ɂ���
        foreach (Transform child in gameTransform)
        {
            BoxCollider2D col = child.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
        //���C���[��ύX
        this.gameObject.layer = LayerMask.NameToLayer("NpcFellow");
    }
}
