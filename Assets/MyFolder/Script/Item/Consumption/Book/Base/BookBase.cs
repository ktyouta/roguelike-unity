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
    public override void collisionItem(Enemy enemy)
    {
        int point = damagePoint != 0 ? damagePoint : 10;
        enemy.enemyHp += point;
    }
}
