using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcChoisesTest : NpcChoices
{

    protected override void selectMessage(int funcNumber)
    {
        //メッセージノードの切り替え
        nowNodeIndex = funcNumber;
        switch (funcNumber)
        {
            case 0:
                Debug.Log("funcnumber"+funcNumber);
                break;
            case 1:
                Debug.Log("funcnumber" + funcNumber);
                break;
            default:
                break;
        }
        //選択肢押下後に止めていた処理(コルーチン)を進める
        clickChioseButtonFlag = true;
    }
}
