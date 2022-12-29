using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAutoExeFunction : NpcBase
{
    public enum ExeAutoNum
    {
        RecoveryPlayerHp, 
        skeltonEnemyAppearance,
        ghostEnemyAppearance
    }
    [Header("一度会話した後のメッセージ")] public List<string> afterMessage;
    [Header("会話の際に実行する関数を指定する変数")] public ExeAutoNum exeFuncNum;
    protected bool talkFlag = false;
    private EventManager eManager;

    public override void Start()
    {
        eManager = GManager.instance.GetComponent<EventManager>();
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
            autoExeFunction();
            talkFlag = true;
        }
        for (int i = 0; i < tempMessage.Count; i++)
        {
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //処理を待機
            yield return null;
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // 会話をwindowのtextフィールドに表示
            showMessage(tempMessage[i]);
            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }

    /**
     * 会話時に実行する関数
     */
    private void autoExeFunction()
    {
        switch(exeFuncNum)
        {
            case ExeAutoNum.RecoveryPlayerHp:
                recoveryPlayerHp();
                break;
            case ExeAutoNum.skeltonEnemyAppearance:
                onSkeltonAppearanceEnemyFlg();
                break;
            case ExeAutoNum.ghostEnemyAppearance:
                onGhostAppearanceEnemyFlg();
                break;
            default:
                break;
        }
    }

    /**
     * プレイヤーのHPを回復する
     */
    protected void recoveryPlayerHp()
    {
        //GManager.instance.playerHp += 10;
    }

    /**
     *敵(骸骨)の出現用フラグをオンにする
     */
    private void onSkeltonAppearanceEnemyFlg()
    {
        eManager.skeltonAppearanceFlg = true;
    }

    /**
     *敵(ゴースト)の出現用フラグをオンにする
     */
    private void onGhostAppearanceEnemyFlg()
    {
        eManager.ghostAppearanceFlg = true;
    }
}
