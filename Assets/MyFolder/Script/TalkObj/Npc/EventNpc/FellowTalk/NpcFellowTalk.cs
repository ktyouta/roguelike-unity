using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellowTalk : NpcChoices
{
    protected override void selectMessage(int funcNumber)
    {
        //メッセージノードの切り替え
        nowNodeIndex = funcNumber;
        switch (funcNumber)
        {
            case 0:
                StartCoroutine(becomeFellow());
                break;
            case 1:
                break;
            default:
                break;
        }
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
        //NPC(仲間になった後用)のコンポーネントを有効にする
        npcFellow.enabled = true;
        npcFellow.npcName = npcFellowTalk.npcName;
        GManager.instance.fellows.Add(npcFellow);
        Transform gameTransform = this.gameObject.transform;
        //子オブジェクトのコライダーを破棄
        foreach (Transform child in gameTransform)
        {
            Destroy(child.gameObject);
        }
        //レイヤーを変更
        this.gameObject.layer = LayerMask.NameToLayer("NpcFellow");
        npcFellowTalk.enabled = false;
    }
}
