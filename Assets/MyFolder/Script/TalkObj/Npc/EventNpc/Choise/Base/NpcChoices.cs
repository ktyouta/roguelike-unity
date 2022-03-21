using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NpcChoices : NpcBase
{
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
    [Header("選択肢ボタン")] public GameObject itemBtn;
    //現在表示されるメッセージのインデックス
    protected int nowNodeIndex = 0;
    protected bool clickChioseButtonFlag = false;

    protected override IEnumerator TalkEvent()
    {
        nowNodeIndex = 0;
        //どの選択肢を表示するかを管理
        int choiseCount = 0;
        for (int i = 0; i < branchMessages.Count; i++)
        {
            //現在のノードから選択肢を取得できなかった場合は、会話を終了する
            if (nowNodeIndex == -1)
            {
                showMessage("会話を終了します。");
                yield return new WaitUntil(() => Input.anyKeyDown);
                break;
            }
            clickChioseButtonFlag = false;
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //処理を待機
            yield return new WaitForSeconds(0.07f);
            GManager.instance.npcWindowImage.transform.Find("Cursol").gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //選択肢を表示するパターン
            if (branchMessages[i].branchMessageBlock[nowNodeIndex].isSelect)
            {
                GManager.instance.choisePanel.SetActive(true);
                // 会話をwindowのtextフィールドに表示
                showMessage(branchMessages[i].branchMessageBlock[nowNodeIndex].message);
                var parentPosition = GManager.instance.choisePanel.transform.position;
                //List<choiseMessageFuncClass> messageBlocks = choiseMessageBlock[choiseCount].choiseMessage[nowNodeIndex].choiseMessages;
                //現在のノード番号に一致する選択肢を取得
                List<choiseMessageFuncClass> messageBlocks = getNodeMessageBlock(choiseMessageBlock[choiseCount].choiseMessage);
                //現在の選択肢を表示する
                for (int j=0;j< messageBlocks.Count; j++)
                {
                    //選択肢ボタンの生成
                    GameObject choiseButton = Instantiate(itemBtn, new Vector3(parentPosition.x-10, parentPosition.y+20-j*35, 0f), Quaternion.identity) as GameObject;
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
                // 会話をwindowのtextフィールドに表示
                showMessage(branchMessages[i].branchMessageBlock[nowNodeIndex].message);
                // キー入力を待機
                yield return new WaitUntil(() => Input.anyKeyDown);
            }
        }
    }

    /**
     * 選択肢クリック時のイベント
     * 継承先の内部で clickChioseButtonFlag = true; の処理必須
     */
    protected abstract void selectMessage(int funcNumber);

    /**
     * 現在のノード番号に一致した選択肢を返す
     */
    List<choiseMessageFuncClass> getNodeMessageBlock(List<ChoiseMessageClass> messagesBlock)
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
}
