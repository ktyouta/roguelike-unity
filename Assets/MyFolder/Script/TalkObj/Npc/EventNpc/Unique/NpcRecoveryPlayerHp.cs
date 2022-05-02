using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcRecoveryPlayerHp : NpcBase
{
    [Header("HP回復後のメッセージ(一度会話した後のメッセージ)")] public List<string> afterMessage;
    protected bool talkFlag = false;

    public override void Start()
    {
        base.Start();
    }

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
            recoveryPlayerHp();
            talkFlag = true;
        }
        for (int i = 0; i < tempMessage.Count; i++)
        {
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //処理を待機
            yield return null;
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // 会話をwindowのtextフィールドに表示
            showMessage(tempMessage[i]);
            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }

    /**
     * プレイヤーのHPを回復する
     */
    protected void recoveryPlayerHp()
    {
        GManager.instance.playerHp += 10;
    }
}
