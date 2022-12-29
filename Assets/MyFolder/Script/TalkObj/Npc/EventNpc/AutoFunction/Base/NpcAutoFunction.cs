using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcAutoFunction : NpcBase
{
    [System.Serializable]
    public class AllBranchFuncTriggerMessageBlock
    {
        [Header("全メッセージ(分岐含む)を格納(ブロック単位)")] public List<BranchFuncTriggerMessageBlock> allBranchMessage = new List<BranchFuncTriggerMessageBlock>();
        [Header("再度会話をする際に表示するメッセージリストのノード番号")] public int blockNodeNumber;
    }

    [System.Serializable]
    public class BranchFuncTriggerMessageBlock
    {
        [Header("画面に表示されるメッセージおよび実行する関数の番号を格納したリスト(ループで一つずつ取り出される)")] public List<FuncTriggerMessages> allBranchFuncMessage = new List<FuncTriggerMessages>();
    }

    [System.Serializable]
    public class FuncTriggerMessages
    {
        [Header("画面上に表示されるメッセージ")] public string message;
        [Header("メッセージが表示されるタイミングで関数を実行するかのフラグ")] public bool isTrigger;
        [Header("実行する関数の番号")] public int funcNumber;
        [Header("表示するメッセージを切り替えるノート番号(関数で切り替わる)")] public int nodeNumber;
    }

    //現在のブロック番号
    protected int blockMessageNodeNumber = 0;
    //現在のブロック内で表示するメッセージのノード番号
    protected int nowMessageNodeNumber = 0;
    [Header("分岐を含む全てのメッセージ(会話後の会話の含む)")] public List<AllBranchFuncTriggerMessageBlock> allMessageBlock = new List<AllBranchFuncTriggerMessageBlock>();

    protected override IEnumerator TalkEvent()
    {
        nowMessageNodeNumber = 0;
        List<BranchFuncTriggerMessageBlock> tempBranchFuncTriggerBlock = getNodeBlock(allMessageBlock);
        //ブロック番号から該当するブロックが返却できなかった場合は、0番目のブロックを使用
        if (tempBranchFuncTriggerBlock.Count < 1)
        {
            tempBranchFuncTriggerBlock = allMessageBlock[0].allBranchMessage;
        }
        for (int i = 0; i < tempBranchFuncTriggerBlock.Count; i++)
        {
            //現在のノードからメッセージまたは選択肢を取得できなかった場合は、会話を終了する
            if (nowMessageNodeNumber == -1)
            {
                showMessage("会話を終了します。");
                yield return new WaitUntil(() => Input.anyKeyDown);
                break;
            }
            //現在のノード番号に一致する会話を取得
            FuncTriggerMessages tempFuncTriggerMessage = getNodeMessage(tempBranchFuncTriggerBlock[i].allBranchFuncMessage);
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //処理を待機
            yield return null;
            pManager.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            // 会話をwindowのtextフィールドに表示
            showMessage(tempFuncTriggerMessage.message);
            //メッセージ表示時に関数を実行する場合
            if (tempFuncTriggerMessage.isTrigger)
            {
                autoFunc(tempFuncTriggerMessage.funcNumber);
            }
            // キー入力を待機
            yield return new WaitUntil(() => Input.anyKeyDown);
            //getNodeMessage関数で現在のノードに一致したメッセージを取得できていない場合
            if (tempFuncTriggerMessage.nodeNumber == -1)
            {
                nowMessageNodeNumber = -1;
                yield return null;
            }
        }
    }

    /**
     * 現在のブロック番号に一致するブロックを返却
     */
    public List<BranchFuncTriggerMessageBlock> getNodeBlock(List<AllBranchFuncTriggerMessageBlock> argMessage)
    {
        List<BranchFuncTriggerMessageBlock> tempFuncTriggerMessageBlock = new List<BranchFuncTriggerMessageBlock>();
        for (int i = 0; i < argMessage.Count; i++)
        {
            if (argMessage[i].blockNodeNumber == blockMessageNodeNumber)
            {
                tempFuncTriggerMessageBlock = argMessage[i].allBranchMessage;
                break;
            }
        }
        return tempFuncTriggerMessageBlock;
    }

    /**
     * 現在のノード番号に一致する会話を返却
     */
    public FuncTriggerMessages getNodeMessage(List<FuncTriggerMessages> argMessage)
    {
        FuncTriggerMessages tempFuncTriggerMessage = new FuncTriggerMessages();
        tempFuncTriggerMessage.message = "ノードに一致する会話がありません。";
        tempFuncTriggerMessage.isTrigger = false;
        tempFuncTriggerMessage.nodeNumber = -1;
        for (int i = 0; i < argMessage.Count; i++)
        {
            if (nowMessageNodeNumber == argMessage[i].nodeNumber)
            {
                tempFuncTriggerMessage = argMessage[i];
                break;
            }
        }
        return tempFuncTriggerMessage;
    }

    /**
     * メッセージが表示された際に自動で実行される関数
     */
    protected abstract void autoFunc(int funcNumber);
}
