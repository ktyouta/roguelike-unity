using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : NpcBase
{
    //[Header("NPCのメッセージ")]public List<string> messages;
    //[HideInInspector] public Image npcImage;
    //[Header("NPCの立ち絵")] public Sprite npcPicture;
    //[Header("NPCの名前")] public string npcName;

    // 親クラスから呼ばれるコールバックメソッド (接触 & ボタン押したときに実行)
    //protected override IEnumerator OnAction()
    //{
    //    GManager.instance.npcNameText.text = npcName + "：";
    //    GManager.instance.npcImage.SetActive(true);
    //    GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
    //    for (int i = 0; i < messages.Count; ++i)
    //    {
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
    //        //処理を待機
    //        yield return new WaitForSeconds(0.07f);
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    //        // 会話をwindowのtextフィールドに表示
    //        showMessage(messages[i]);

    //        // キー入力を待機
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
            //処理を待機
            yield return new WaitForSeconds(0.07f);
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // 会話をwindowのtextフィールドに表示
            showMessage(messages[i]);

            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }
}
