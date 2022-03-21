using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : NpcBase
{
    //[Header("NPC�̃��b�Z�[�W")]public List<string> messages;
    //[HideInInspector] public Image npcImage;
    //[Header("NPC�̗����G")] public Sprite npcPicture;
    //[Header("NPC�̖��O")] public string npcName;

    // �e�N���X����Ă΂��R�[���o�b�N���\�b�h (�ڐG & �{�^���������Ƃ��Ɏ��s)
    //protected override IEnumerator OnAction()
    //{
    //    GManager.instance.npcNameText.text = npcName + "�F";
    //    GManager.instance.npcImage.SetActive(true);
    //    GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
    //    for (int i = 0; i < messages.Count; ++i)
    //    {
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
    //        //������ҋ@
    //        yield return new WaitForSeconds(0.07f);
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    //        // ��b��window��text�t�B�[���h�ɕ\��
    //        showMessage(messages[i]);

    //        // �L�[���͂�ҋ@
    //        yield return new WaitUntil(() => Input.anyKeyDown);
    //    }
    //    GManager.instance.npcImage.GetComponent<Image>().sprite = null;
    //    GManager.instance.npcImage.SetActive(false);
    //    yield break;
    //}

    protected override IEnumerator TalkEvent()
    {
        for (int i = 0; i < messages.Count; i++)
        {
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return new WaitForSeconds(0.07f);
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(messages[i]);

            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }
}
