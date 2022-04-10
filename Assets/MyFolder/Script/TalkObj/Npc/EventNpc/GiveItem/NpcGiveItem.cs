using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcGiveItem : NpcBase
{
    [Header("アイテム引き渡し後のメッセージ(一度会話した後のメッセージ)")] public List<string> afterMessage;
    [Header("プレイヤーに渡すアイテム")] public GameObject giveItem;
    protected bool talkFlag = false;

    private void Start()
    {
        base.Start();
        messages.Add("アイテムゲット");
    }

    // 親クラスから呼ばれるコールバックメソッド (接触 & ボタン押したときに実行)
    //protected override IEnumerator OnAction()
    //{
    //    GManager.instance.npcNameText.text = npcName + "：";
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
    //        //処理を待機
    //        yield return new WaitForSeconds(0.07f);
    //        GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
    //        // 会話をwindowのtextフィールドに表示
    //        showMessage(tempMessage[i]);

    //        // キー入力を待機
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
            //処理を待機
            yield return new WaitForSeconds(0.07f);
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // 会話をwindowのtextフィールドに表示
            showMessage(tempMessage[i]);
            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }

    protected void giveItemToPlayer()
    {
        GManager.instance.addItem(giveItem);
    }
}
