using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcChoisesTest : NpcChoices
{

    protected override void selectMessage(int funcNumber)
    {
        //���b�Z�[�W�m�[�h�̐؂�ւ�
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
        //�I����������Ɏ~�߂Ă�������(�R���[�`��)��i�߂�
        clickChioseButtonFlag = true;
    }
}
