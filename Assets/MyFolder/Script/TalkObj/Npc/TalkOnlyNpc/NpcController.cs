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
            //処理を待機
            yield return null;
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // 会話をwindowのtextフィールドに表示
            showMessage(messages[i]);
            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }
}
