using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentOuterWall : DamageActionComponentBase
{
    //�j��s�t���O
    [HideInInspector] public bool isIndestructible;

    public override void reciveDamageAction(int hp)
    {
        if (isIndestructible)
        {
            GManager.instance.wrightLog("�j��s�\�I�u�W�F�N�g�ł��B");
            return;
        }
        if (hp <= 0)
        {
            GManager.instance.wrightLog("�ǂ���ꂽ�B");
            Destroy(this.gameObject);
        }
        else
        {
            GManager.instance.wrightLog("�O�ǂ̎c��ϋv�l�F" + hp);
        }
    }
}
