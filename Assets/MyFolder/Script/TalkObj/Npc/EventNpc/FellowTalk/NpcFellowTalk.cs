using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class NpcFellowTalk : NpcChoices
{
    [Header("仲間にするのに必要な魅力度")] public int requiredCharm;
    protected override void selectMessage(int funcNumber)
    {
        int tempNowIndex = funcNumber;
        switch (nowNodeIndex)
        {
            //仲間を入れ替える際のノード
            case -98:
                replaceNpc(funcNumber);
                tempNowIndex = -98;
                break;
            //仲間にできる上限数を超えている際のノード
            case -100:
                switch (funcNumber)
                {
                    //入れ替える
                    case -100:
                        List<choiseMessageFuncClass> tempChoiseMessages = new List<choiseMessageFuncClass>();
                        //NPCの名前をIDで新たに選択肢を作成
                        for (int i=0;i<GManager.instance.fellows.Count;i++)
                        {
                            tempChoiseMessages.Add(new choiseMessageFuncClass() { message = GManager.instance.fellows[i].npcName, funcNumber = GManager.instance.fellows[i].npcId});
                        }
                        ChoiseMessageClass tempChoiseMessage = new ChoiseMessageClass();
                        tempChoiseMessage.choiseMessages = tempChoiseMessages;
                        tempChoiseMessage.messageNodeNumber = -98;
                        tempChoiseMessage.choiseType = ChoiseMessageTypeList.dynamicChoise;
                        ChioseMessageBlockClass tempChoiseMessageBlock = new ChioseMessageBlockClass();
                        tempChoiseMessageBlock.choiseMessage.Add(tempChoiseMessage);
                        choiseMessageBlock.Add(tempChoiseMessageBlock);
                        tempNowIndex = -98;
                        break;
                    case -101:
                        tempNowIndex = -97;
                        break;
                    default:
                        break;
                }
                break;
            default:
                switch (funcNumber)
                {
                    //仲間にする
                    case 0:
                        int tempRequiredCharm = requiredCharm == 0 ? 1 : requiredCharm;
                        //仲間にできる上限数を超えている
                        if (GManager.instance.fellows.Count > Common.Define.FELLOWS_MAXNUM)
                        {
                            tempNowIndex = -100;
                        }
                        //仲間にするための魅力が足りない
                        else if (GManager.instance.playerCharm < tempRequiredCharm)
                        {
                            tempNowIndex = -99;
                        }
                        else
                        {
                            GManager.instance.playerCharm -= tempRequiredCharm;
                            StartCoroutine(becomeFellow());
                        }
                        break;
                    case 1:
                        break;
                    default:
                        break;
                }
                break;
        }
        //メッセージノードの切り替え
        nowNodeIndex = tempNowIndex;
        //選択肢押下後に止めていた処理(コルーチン)を進める
        clickChioseButtonFlag = true;
    }

    /*
     * プレイヤーの仲間になる
     */
    protected IEnumerator becomeFellow()
    {
        yield return new WaitUntil(() => isEndTalk);
        NpcFellowTalk npcFellowTalk = GetComponent<NpcFellowTalk>();
        NpcFellow npcFellow = GetComponent<NpcFellow>();
        settingFellow(npcFellowTalk, npcFellow);
        //NPC用のリストに追加
        GManager.instance.fellows.Add(npcFellow);
        //NPC(仲間になった後用)のコンポーネントを有効にする
        npcFellow.enabled = true;
        Transform gameTransform = this.gameObject.transform;
        settingChildCollider();
        npcFellowTalk.enabled = false;
    }

    /**
     * 仲間にするNPCの設定
     */
    protected void settingFellow(NpcFellowTalk npcFellowTalk, NpcFellow npcFellow)
    {
        npcFellow.npcName = npcFellowTalk.npcName;
        npcFellow.npcId = GManager.instance.latestNpcId;
        GManager.instance.latestNpcId++;
    }

    /**
     * NPCの入れ替え
     */
    protected void replaceNpc(int targetNpcId)
    {
        for (int i=0;i<GManager.instance.fellows.Count;i++)
        {
            //選択したNPCと入れ替え
            if (targetNpcId == GManager.instance.fellows[i].npcId)
            {
                NpcFellowTalk npcFellowTalk = GetComponent<NpcFellowTalk>();
                NpcFellow npcFellow = GetComponent<NpcFellow>();
                settingFellow(npcFellowTalk, npcFellow);
                //NPC(入れ替え対象)を会話用に戻す
                GManager.instance.fellows[i].GetComponent<NpcFellowTalk>().enabled = true;
                Transform gameTransform = GManager.instance.fellows[i].transform;
                //子オブジェクトのコライダーを有効にする
                foreach (Transform child in gameTransform)
                {
                    BoxCollider2D col = child.GetComponent<BoxCollider2D>();
                    if (col != null)
                    {
                        col.enabled = true;
                    }
                }
                GManager.instance.fellows[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
                GManager.instance.fellows[i].enabled = false;
                //位置の入れ替え
                GManager.instance.fellows[i].transform.position = npcFellowTalk.transform.position;
                //リストの入れ替え
                GManager.instance.fellows[i] = npcFellow;
                settingChildCollider();
                //NPC(仲間になった後用)のコンポーネントを有効にする
                npcFellow.enabled = true;
                break;
            }
        }
    }

    /**
     * 子オブジェクトのコライダーの無効化とレイヤーの変更
     */
    protected void settingChildCollider()
    {
        Transform gameTransform = this.gameObject.transform;
        //子オブジェクトのコライダーを無効にする
        foreach (Transform child in gameTransform)
        {
            BoxCollider2D col = child.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
        //レイヤーを変更
        this.gameObject.layer = LayerMask.NameToLayer("NpcFellow");
    }
}
