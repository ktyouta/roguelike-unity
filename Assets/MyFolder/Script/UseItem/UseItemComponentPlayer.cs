using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemComponentPlayer : UseItemComponentBase
{
    //�A�C�e�����g�p
    public override void use(Item item, StatusComponentMoving statusObj)
    {
        base.use(item,statusObj);
        GManager.instance.isEndPlayerAction = true;
        GManager.instance.playersTurn = false;
        //�v���C���[�̈ʒu���͕K�����X�g�̐擪�ɂȂ�
        GManager.instance.charsNextPosition.Add(statusObj.transform.position);
    }
}
