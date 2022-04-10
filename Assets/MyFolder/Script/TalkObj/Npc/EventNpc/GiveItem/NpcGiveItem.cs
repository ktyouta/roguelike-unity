using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcGiveItem : NpcBase
{
    [Header("�A�C�e�������n����̃��b�Z�[�W(��x��b������̃��b�Z�[�W)")] public List<string> afterMessage;
    [Header("�v���C���[�ɓn���A�C�e��")] public GameObject giveItem;
    protected bool talkFlag = false;

    private void Start()
    {
        base.Start();
        messages.Add("�A�C�e���Q�b�g");
    }

    // �e�N���X����Ă΂��R�[���o�b�N���\�b�h (�ڐG & �{�^���������Ƃ��Ɏ��s)
    //protected override IEnumerator OnAction()
    //{
    //    GManager.instance.npcNameText.text = npcName + "�F";
    //    GManager.instance.npcImage.SetActive(true);
    //    GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
    //    List<string> tempMessage;
    //    if (talkFlag)
    //    {
    //        tempMessage = afterMessage;
    //    }
    //    else
    //    {
    //        tempMessage = messages;
    //        giveItemToPlayer();
    //        talkFlag = true;
    //    }
    //    for (int i = 0; i < tempMessage.Count; ++i)
    //    {
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
    //        //������ҋ@
    //        yield return new WaitForSeconds(0.07f);
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    //        // ��b��window��text�t�B�[���h�ɕ\��
    //        showMessage(tempMessage[i]);

    //        // �L�[���͂�ҋ@
    //        yield return new WaitUntil(() => Input.anyKeyDown);
    //    }
    //    GManager.instance.npcImage.GetComponent<Image>().sprite = null;
    //    GManager.instance.npcImage.SetActive(false);
    //    yield break;
    //}

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
            giveItemToPlayer();
            talkFlag = true;
        }
        for (int i = 0; i < tempMessage.Count; i++)
        {
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return new WaitForSeconds(0.07f);
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(tempMessage[i]);
            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }

    protected void giveItemToPlayer()
    {
        GManager.instance.addItem(giveItem);
    }
}
