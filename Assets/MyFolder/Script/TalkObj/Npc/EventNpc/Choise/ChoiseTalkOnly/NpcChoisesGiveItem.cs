using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcChoisesGiveItem : NpcChoices
{
    [Header("�v���C���[�ɓn���A�C�e��")] public GameObject giveItem;
    protected override void selectMessage(int funcNumber)
    {
        //���b�Z�[�W�m�[�h�̐؂�ւ�
        nowNodeIndex = funcNumber;
        switch (funcNumber)
        {
            case 0:
                Debug.Log("funcnumber" + funcNumber);
                giveItemToPlayer();
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

    /*
     * �v���C���[�ɃA�C�e����n��
     */
    protected void giveItemToPlayer()
    {
        GManager.instance.addItem(giveItem);
    }
}
