using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcAutoFunction : NpcBase
{
    [System.Serializable]
    public class AllBranchFuncTriggerMessageBlock
    {
        [Header("�S���b�Z�[�W(����܂�)���i�[(�u���b�N�P��)")] public List<BranchFuncTriggerMessageBlock> allBranchMessage = new List<BranchFuncTriggerMessageBlock>();
        [Header("�ēx��b������ۂɕ\�����郁�b�Z�[�W���X�g�̃m�[�h�ԍ�")] public int blockNodeNumber;
    }

    [System.Serializable]
    public class BranchFuncTriggerMessageBlock
    {
        [Header("��ʂɕ\������郁�b�Z�[�W����ю��s����֐��̔ԍ����i�[�������X�g(���[�v�ň�����o�����)")] public List<FuncTriggerMessages> allBranchFuncMessage = new List<FuncTriggerMessages>();
    }

    [System.Serializable]
    public class FuncTriggerMessages
    {
        [Header("��ʏ�ɕ\������郁�b�Z�[�W")] public string message;
        [Header("���b�Z�[�W���\�������^�C�~���O�Ŋ֐������s���邩�̃t���O")] public bool isTrigger;
        [Header("���s����֐��̔ԍ�")] public int funcNumber;
        [Header("�\�����郁�b�Z�[�W��؂�ւ���m�[�g�ԍ�(�֐��Ő؂�ւ��)")] public int nodeNumber;
    }

    //���݂̃u���b�N�ԍ�
    protected int blockMessageNodeNumber = 0;
    //���݂̃u���b�N���ŕ\�����郁�b�Z�[�W�̃m�[�h�ԍ�
    protected int nowMessageNodeNumber = 0;
    [Header("������܂ޑS�Ẵ��b�Z�[�W(��b��̉�b�̊܂�)")] public List<AllBranchFuncTriggerMessageBlock> allMessageBlock = new List<AllBranchFuncTriggerMessageBlock>();

    protected override IEnumerator TalkEvent()
    {
        nowMessageNodeNumber = 0;
        List<BranchFuncTriggerMessageBlock> tempBranchFuncTriggerBlock = getNodeBlock(allMessageBlock);
        //�u���b�N�ԍ�����Y������u���b�N���ԋp�ł��Ȃ������ꍇ�́A0�Ԗڂ̃u���b�N���g�p
        if (tempBranchFuncTriggerBlock.Count < 1)
        {
            tempBranchFuncTriggerBlock = allMessageBlock[0].allBranchMessage;
        }
        for (int i = 0; i < tempBranchFuncTriggerBlock.Count; i++)
        {
            //���݂̃m�[�h���烁�b�Z�[�W�܂��͑I�������擾�ł��Ȃ������ꍇ�́A��b���I������
            if (nowMessageNodeNumber == -1)
            {
                showMessage("��b���I�����܂��B");
                yield return new WaitUntil(() => Input.anyKeyDown);
                break;
            }
            //���݂̃m�[�h�ԍ��Ɉ�v�����b���擾
            FuncTriggerMessages tempFuncTriggerMessage = getNodeMessage(tempBranchFuncTriggerBlock[i].allBranchFuncMessage);
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return null;
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(tempFuncTriggerMessage.message);
            //���b�Z�[�W�\�����Ɋ֐������s����ꍇ
            if (tempFuncTriggerMessage.isTrigger)
            {
                autoFunc(tempFuncTriggerMessage.funcNumber);
            }
            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
            //getNodeMessage�֐��Ō��݂̃m�[�h�Ɉ�v�������b�Z�[�W���擾�ł��Ă��Ȃ��ꍇ
            if (tempFuncTriggerMessage.nodeNumber == -1)
            {
                nowMessageNodeNumber = -1;
                yield return null;
            }
        }
    }

    /**
     * ���݂̃u���b�N�ԍ��Ɉ�v����u���b�N��ԋp
     */
    public List<BranchFuncTriggerMessageBlock> getNodeBlock(List<AllBranchFuncTriggerMessageBlock> argMessage)
    {
        List<BranchFuncTriggerMessageBlock> tempFuncTriggerMessageBlock = new List<BranchFuncTriggerMessageBlock>();
        for (int i = 0; i < argMessage.Count; i++)
        {
            if (argMessage[i].blockNodeNumber == blockMessageNodeNumber)
            {
                tempFuncTriggerMessageBlock = argMessage[i].allBranchMessage;
                break;
            }
        }
        return tempFuncTriggerMessageBlock;
    }

    /**
     * ���݂̃m�[�h�ԍ��Ɉ�v�����b��ԋp
     */
    public FuncTriggerMessages getNodeMessage(List<FuncTriggerMessages> argMessage)
    {
        FuncTriggerMessages tempFuncTriggerMessage = new FuncTriggerMessages();
        tempFuncTriggerMessage.message = "�m�[�h�Ɉ�v�����b������܂���B";
        tempFuncTriggerMessage.isTrigger = false;
        tempFuncTriggerMessage.nodeNumber = -1;
        for (int i = 0; i < argMessage.Count; i++)
        {
            if (nowMessageNodeNumber == argMessage[i].nodeNumber)
            {
                tempFuncTriggerMessage = argMessage[i];
                break;
            }
        }
        return tempFuncTriggerMessage;
    }

    /**
     * ���b�Z�[�W���\�����ꂽ�ۂɎ����Ŏ��s�����֐�
     */
    protected abstract void autoFunc(int funcNumber);
}
