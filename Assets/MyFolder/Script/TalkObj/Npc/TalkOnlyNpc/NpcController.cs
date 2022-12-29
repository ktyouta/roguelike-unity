using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : NpcBase
{
    protected override IEnumerator TalkEvent()
    {
        for (int i = 0; i < messages.Count; i++)
        {
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //������ҋ@
            yield return null;
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // ��b��window��text�t�B�[���h�ɕ\��
            showMessage(messages[i]);
            // �L�[���͂�ҋ@
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }
}
