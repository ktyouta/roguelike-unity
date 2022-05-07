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
        GManager.instance.fellows.Add(GetComponent<NpcFellow>());
        GetComponent<NpcFellow>().enabled = true;
        Transform gameTransform = this.gameObject.transform;
        //子オブジェクトのコライダーを破棄
        foreach (Transform child in gameTransform)
        {
            Destroy(child.gameObject);
        }
        //レイヤーを変更
        this.gameObject.layer = LayerMask.NameToLayer("NpcFellow");
        GetComponent<NpcFellowTalk>().enabled = false;
    }
}
