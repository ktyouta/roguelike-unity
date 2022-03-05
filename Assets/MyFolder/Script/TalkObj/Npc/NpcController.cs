using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : TalkBase
{
    [Header("NPCのメッセージ")]public List<string> messages;
    [HideInInspector] public Image npcImage;
    [Header("NPCの立ち絵")] public Sprite npcPicture;
    //[HideInInspector]public GameObject npcCanvas; 

    void Start()
    {
        //npcImage = GetComponent<Image>();
        base.Start();
    }

    // 親クラスから呼ばれるコールバックメソッド (接触 & ボタン押したときに実行)
    protected override IEnumerator OnAction()
    {
        GManager.instance.npcImage.SetActive(true);
        GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
        for (int i = 0; i < messages.Count; ++i)
        {
            // 1フレーム分 処理を待機
            yield return null;

            // 会話をwindowのtextフィールドに表示
            showMessage(messages[i]);

            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        GManager.instance.npcImage.GetComponent<Image>().sprite = null;
        GManager.instance.npcImage.SetActive(false);
        yield break;
    }
}
