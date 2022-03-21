using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NpcChoices : NpcBase
{
    [System.Serializable]
    public class MessageClass
    {
        [Header("��ʏ�ɕ\������郁�b�Z�[�W")]public string message;
        [Header("�I������\������t���O")]public bool isSelect;
        [Header("���b�Z�[�W�c���[�̃m�[�h�ԍ�")]public int messageNodeNumber;
    }

    [System.Serializable]
    public class BranchMessageClass
    {
        [Header("���b�Z�[�W�A�I�����t���O�A�m�[�h�ԍ�����̂܂Ƃ܂�Ƃ������X�g")] public List<MessageClass> branchMessageBlock = new List<MessageClass>();
    }

    [System.Serializable]
    public class ChioseMessageBlockClass
    {
        [Header("�I�����ƃm�[�h�ԍ��̃Z�b�g")] public List<ChoiseMessageClass> choiseMessage = new List<ChoiseMessageClass>();
    }

    [System.Serializable]
    public class ChoiseMessageClass
    {
        //�I�������i�[����
        [Header("��ʂɕ\�������S�I�����A�I�����̕���p�̔ԍ�����̂܂Ƃ܂�Ƃ������X�g")] public List<choiseMessageFuncClass> choiseMessages = new List<choiseMessageFuncClass>();
        [Header("���b�Z�[�W�c���[�̃m�[�h�ԍ�")] public int messageNodeNumber;
    }

    [System.Serializable]
    public class choiseMessageFuncClass
    {
        [Header("�I����")]public string message;
        [Header("�I������I�񂾍ۂɎ��s����֐��̔ԍ�")]public int funcNumber;
    }

    //��b���b�Z�[�W
    [Header("������܂ޑS�Ẵ��b�Z�[�W")] public List<BranchMessageClass> branchMessages = new List<BranchMessageClass>();
    //�t���O���i�[�������b�Z�[�W
    [Header("�ʏ�̃��b�Z�[�W")] public List<MessageClass> message = new List<MessageClass>();
    //�I����
    [Header("������܂ޑS�Ă̑I����")] public List<ChioseMessageBlockClass> choiseMessageBlock = new List<ChioseMessageBlockClass>();
    [Header("�I�����{�^��")] public GameObject itemBtn;
    //���ݕ\������郁�b�Z�[�W�̃C���f�b�N�X
    protected int nowNodeIndex = 0;
    protected bool clickChioseButtonFlag = false;

    protected override IEnumerator TalkEvent()
    {
        nowNodeIndex = 0;
        //�ǂ̑I������\�����邩���Ǘ�
        int choiseCount = 0;
        for (int i = 0; i < branchMessages.Count; i++)
        {
            //���݂̃m�[�h����I�������擾�ł��Ȃ������ꍇ�́A��b���I������
            if (nowNodeIndex == -1)
            {
                showMessage("��b���I�����܂��B");
                yield return new WaitUntil(() => Input.anyKeyDown);
                break;
            }
            clickChioseButtonFlag = false;
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return new WaitForSeconds(0.07f);
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //�I������\������p�^�[��
            if (branchMessages[i].branchMessageBlock[nowNodeIndex].isSelect)
            {
                GManager.instance.choisePanel.SetActive(true);
                // ��b��window��text�t�B�[���h�ɕ\��
                showMessage(branchMessages[i].branchMessageBlock[nowNodeIndex].message);
                var parentPosition = GManager.instance.choisePanel.transform.position;
                //List<choiseMessageFuncClass> messageBlocks = choiseMessageBlock[choiseCount].choiseMessage[nowNodeIndex].choiseMessages;
                //���݂̃m�[�h�ԍ��Ɉ�v����I�������擾
                List<choiseMessageFuncClass> messageBlocks = getNodeMessageBlock(choiseMessageBlock[choiseCount].choiseMessage);
                //���݂̑I������\������
                for (int j=0;j< messageBlocks.Count; j++)
                {
                    //�I�����{�^���̐���
                    GameObject choiseButton = Instantiate(itemBtn, new Vector3(parentPosition.x-10, parentPosition.y+20-j*35, 0f), Quaternion.identity) as GameObject;
                    //choisePanel�̎q�I�u�W�F�N�g�ɂ���
                    choiseButton.transform.SetParent(GManager.instance.choisePanel.transform,false);
                    choiseButton.transform.Find("Text").GetComponent<Text>().text = messageBlocks[j].message;
                    int funcIndex = messageBlocks[j].funcNumber;
                    //�I�������N���b�N�����ۂ̃��\�b�h��ݒ�
                    choiseButton.GetComponent<Button>().onClick.AddListener(() => selectMessage(funcIndex));
                }
                yield return new WaitUntil(() => clickChioseButtonFlag);
                GManager.instance.choisePanel.SetActive(false);
                //�I�����{�^�������ׂč폜
                foreach (Transform child in GManager.instance.choisePanel.transform)
                {
                    Destroy(child.gameObject);
                }
                choiseCount++;
            }
            //�ʏ�̃��b�Z�[�W
            else
            {
                // ��b��window��text�t�B�[���h�ɕ\��
                showMessage(branchMessages[i].branchMessageBlock[nowNodeIndex].message);
                // �L�[���͂�ҋ@
                yield return new WaitUntil(() => Input.anyKeyDown);
            }
        }
    }

    /**
     * �I�����N���b�N���̃C�x���g
     * �p����̓����� clickChioseButtonFlag = true; �̏����K�{
     */
    protected abstract void selectMessage(int funcNumber);

    /**
     * ���݂̃m�[�h�ԍ��Ɉ�v�����I������Ԃ�
     */
    List<choiseMessageFuncClass> getNodeMessageBlock(List<ChoiseMessageClass> messagesBlock)
    {
        List<choiseMessageFuncClass> tempMessageBlock = new List<choiseMessageFuncClass>();
        tempMessageBlock.Add(new choiseMessageFuncClass() { message = "�m�[�h�Ɉ�v����I����������܂���B", funcNumber = -1 });
        for (int i=0;i< messagesBlock.Count;i++)
        {
            //���݂̃m�[�h�Ɉ�v����I���������������ꍇ
            if (messagesBlock[i].messageNodeNumber == nowNodeIndex)
            {
                return messagesBlock[i].choiseMessages;
            }
        }
        return tempMessageBlock;
    }
}
