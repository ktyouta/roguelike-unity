using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBase : Consumption
{
    [Header("�Փˎ��̃_���[�W��")] public int damagePoint;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    /**
     * �A�C�e�����Փ˂����ꍇ�̏���
     */
    public override void collisionItem(GameObject targetObj)
    {
        int point = damagePoint != 0 ? damagePoint : 10;
        //enemy.enemyHp += point;
        //�ΏۃI�u�W�F�N�g�̉񕜏������s��
        OutAccessComponentBase outAccessObj = targetObj?.GetComponent<OutAccessComponentBase>();
        if (outAccessObj == null)
        {
            return;
        }
        //�񕜏���
        outAccessObj.callCalculateRecoveryHp(point, MessageManager.createMessage("2", outAccessObj.statusObj.charName.name, point.ToString()));
    }
}
