using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAutoGiveItem : NpcAutoFunction
{
    [Header("�v���C���[�ɓn���A�C�e��")] public GameObject giveItem;

    /**
     * ���b�Z�[�W���\�����ꂽ�ۂɎ����Ŏ��s�����֐�
     */
    protected override void autoFunc(int funcNumber)
    {
        switch (funcNumber)
        {
            //�������s���Ȃ�
            case 0:
                break;
            //�v���C���[�ɃA�C�e����n��
            case 1:
                giveItemToPlayer();
                break;
            default:
                break;
        }
    }

    /**
     * �v���C���[�ɃA�C�e����n��
     */
    protected void giveItemToPlayer()
    {
        GameObject tempItem = Instantiate(giveItem) as GameObject;
        //���������̃`�F�b�N
        if (GManager.instance.addItem(tempItem))
        {
            blockMessageNodeNumber = 1;
        }
        else
        {
            //�m�[�h��؂�ւ���
            nowMessageNodeNumber = 1;
        }
    }
}
