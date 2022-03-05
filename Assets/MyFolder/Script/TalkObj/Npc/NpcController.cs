using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : TalkBase
{
    [Header("NPC�̃��b�Z�[�W")]public List<string> messages;
    [HideInInspector] public Image npcImage;
    [Header("NPC�̗����G")] public Sprite npcPicture;
    //[HideInInspector]public GameObject npcCanvas; 

    void Start()
    {
        //npcImage = GetComponent<Image>();
        base.Start();
    }

    // �e�N���X����Ă΂��R�[���o�b�N���\�b�h (�ڐG & �{�^���������Ƃ��Ɏ��s)
    protected override IEnumerator OnAction()
    {
        GManager.instance.npcImage.SetActive(true);
        GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
        for (int i = 0; i < messages.Count; ++i)
        {
            // 1�t���[���� ������ҋ@
            yield return null;

            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(messages[i]);

            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        GManager.instance.npcImage.GetComponent<Image>().sprite = null;
        GManager.instance.npcImage.SetActive(false);
        yield break;
    }
}
