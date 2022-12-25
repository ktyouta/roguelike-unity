using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    //�����̃A�C�e������������
    private const int INIT_MAX_POSESSION = 3;

    //�v���C���[�̃A�C�e�����X�g
    public static List<GameObject> itemList;
    //�A�C�e���̏���������
    public static int nowMaxPosession = INIT_MAX_POSESSION;

    static ItemManager()
    {
        itemList = new List<GameObject>();
    }

    /**
     * �擾�����A�C�e�������X�g�ɒǉ�����
     */
    public static bool addItem(GameObject item)
    {
        //�A�C�e���̏��������𒴂��Ă���ꍇ
        if (itemList.Count + 1 > nowMaxPosession)
        {
            GManager.instance.wrightLog(MessageManager.createMessage("3"));
            return false;
        }
        //�C���x���g���[����̏�ԂȂ�0�����蓖�Ă�
        int itemId = itemList.Count == 0 ? 0 : itemList[itemList.Count - 1].GetComponent<Item>().id + 1;
        //ID�̊��蓖��
        item.GetComponent<Item>().id = itemId;
        itemList.Add(item);
        return true;
    }

}
