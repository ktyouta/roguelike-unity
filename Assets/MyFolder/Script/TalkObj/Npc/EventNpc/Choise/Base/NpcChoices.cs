using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NpcChoices : NpcBase
{
    [HideInInspector] protected bool isEndTalk;
    [System.Serializable]
    public class MessageClass
    {
        [Header("画面上に表示されるメッセージ")]public string message;
        [Header("選択肢を表示するフラグ")]public bool isSelect;
        [Header("メッセージツリーのノード番号")]public int messageNodeNumber;
    }

    [System.Serializable]
    public class BranchMessageClass
    {
        [Header("メッセージ、選択肢フラグ、ノード番号を一つのまとまりとしたリスト")] public List<MessageClass> branchMessageBlock = new List<MessageClass>();
    }

    [System.Serializable]
    public class ChioseMessageBlockClass
    {
        [Header("選択肢とノード番号のセット")] public List<ChoiseMessageClass> choiseMessage = new List<ChoiseMessageClass>();
    }

    [System.Serializable]
    public class ChoiseMessageClass
    {
        //選択肢を格納する
        [Header("画面に表示される全選択肢、選択時の分岐用の番号を一つのまとまりとしたリスト")] public List<choiseMessageFuncClass> choiseMessages = new List<choiseMessageFuncClass>();
        [Header("メッセージツリーのノード番号")] public int messageNodeNumber;
    }

    [System.Serializable]
    public class choiseMessageFuncClass
    {
        [Header("選択肢")]public string message;
        [Header("選択肢を選んだ際に実行する関数の番号")]public int funcNumber;
    }

    //会話メッセージ
    [Header("分岐を含む全てのメッセージ")] public List<BranchMessageClass> branchMessages = new List<BranchMessageClass>();
    //フラグを格納したメッセージ
    [Header("通常のメッセージ")] public List<MessageClass> message = new List<MessageClass>();
    //選択肢
    [Header("分岐を含む全ての選択肢")] public List<ChioseMessageBlockClass> choiseMessageBlock = new List<ChioseMessageBlockClass>();
    [Header("選択肢ボタン")] public GameObject selectBtn;
    //現在表示されるメッセージのインデックス
    protected int nowNodeIndex = 0;
    protected bool clickChioseButtonFlag = false;

    protected override IEnumerator TalkEvent()
    {
        nowNodeIndex = 0;
        isEndTalk = false;
        //どの選択肢を表示するかを管理
        int choiseCount = 0;
        for (int i = 0; i < branchMessages.Count; i++)
        {
            //現在のノードからメッセージまたは選択肢を取得できなかった場合は、会話を終了する
            if (nowNodeIndex == -1)
            {
                showMessage("会話を終了します。");
                yield return new WaitUntil(() => Input.anyKeyDown);
                break;
            }
            clickChioseButtonFlag = false;
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //処理を待機
            yield return null;
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            MessageClass displayMessageBlock = getNodeMessageBlock(branchMessages[i].branchMessageBlock);
            // 会話をwindowのtextフィールドに表示
            showMessage(displayMessageBlock.message);
            //選択肢を表示するパターン
            if (displayMessageBlock.isSelect)
            {
                GManager.instance.choisePanel.SetActive(true);
                var parentPosition = GManager.instance.choisePanel.transform.position;
                //現在のノード番号に一致する選択肢を取得
                List<choiseMessageFuncClass> messageBlocks = getNodeChoiseBlock(choiseMessageBlock[choiseCount].choiseMessage);
                //選択肢の数から選択肢パネルの高さを求める
                float panelHeight = messageBlocks.Count * 40f + 10;
                RectTransform choisePanelRect = GManager.instance.choisePanel.GetComponent<RectTransform>();
                Vector2 size = choisePanelRect.sizeDelta;
                size.y = panelHeight;
                choisePanelRect.sizeDelta = size;
                //ボタンの設置座標を設定するためにpivotを取得
                float pivotY = choisePanelRect.pivot.y;
                //現在の選択肢を表示する
                for (int j=0;j< messageBlocks.Count; j++)
                {
                    //選択肢ボタンの生成
                    GameObject choiseButton = Instantiate(selectBtn, new Vector3(parentPosition.x-10, pivotY + panelHeight*0.5f - 30 - j*35, 0f), Quaternion.identity) as GameObject;
                    //choisePanelの子オブジェクトにする
                    choiseButton.transform.SetParent(GManager.instance.choisePanel.transform,false);
                    choiseButton.transform.Find("Text").GetComponent<Text>().text = messageBlocks[j].message;
                    int funcIndex = messageBlocks[j].funcNumber;
                    //選択肢をクリックした際のメソッドを設定
                    choiseButton.GetComponent<Button>().onClick.AddListener(() => selectMessage(funcIndex));
                }
                yield return new WaitUntil(() => clickChioseButtonFlag);
                GManager.instance.choisePanel.SetActive(false);
                //選択肢ボタンをすべて削除
                foreach (Transform child in GManager.instance.choisePanel.transform)
                {
                    Destroy(child.gameObject);
                }
                choiseCount++;
            }
            //通常のメッセージ
            else
            {
                // キー入力を待機
                yield return new WaitUntil(() => Input.anyKeyDown);
                //getNodeMessageBlock関数で現在のノードに一致したメッセージを取得できていない場合
                if (displayMessageBlock.messageNodeNumber == -1)
                {
                    nowNodeIndex = -1;
                    yield return null;
                }
            }
        }
        isEndTalk = true;
    }

    /**
     * 選択肢クリック時のイベント
     * 継承先の内部で clickChioseButtonFlag = true; の処理必須
     */
    protected abstract void selectMessage(int funcNumber);

    /**
     * 現在のノード番号に一致した選択肢を返す
     */
    List<choiseMessageFuncClass> getNodeChoiseBlock(List<ChoiseMessageClass> messagesBlock)
    {
        List<choiseMessageFuncClass> tempMessageBlock = new List<choiseMessageFuncClass>();
        tempMessageBlock.Add(new choiseMessageFuncClass() { message = "ノードに一致する選択肢がありません。", funcNumber = -1 });
        for (int i=0;i< messagesBlock.Count;i++)
        {
            //現在のノードに一致する選択肢が見つかった場合
            if (messagesBlock[i].messageNodeNumber == nowNodeIndex)
            {
                return messagesBlock[i].choiseMessages;
            }
        }
        return tempMessageBlock;
    }

    /**
     * 現在のノード番号に一致した会話を返す
     */
    MessageClass getNodeMessageBlock(List<MessageClass> messagesBlock)
    {
        MessageClass tempMessageBlock = new MessageClass();
        tempMessageBlock.message = "ノードに一致する会話がありません。";
        tempMessageBlock.isSelect = false;
        tempMessageBlock.messageNodeNumber = -1;
        for (int i = 0; i < messagesBlock.Count; i++)
        {
            //現在のノードに一致する会話が見つかった場合
            if (messagesBlock[i].messageNodeNumber == nowNodeIndex)
            {
                return messagesBlock[i];
            }
        }
        return tempMessageBlock;
    }
}
