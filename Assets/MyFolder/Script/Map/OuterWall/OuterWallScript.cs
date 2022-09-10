using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallScript : MonoBehaviour
{
    [Header("�j��s�t���O")] public bool isIndestructible;
    [Header("�ǂ̑ϋv�l")] public int wallHp;

    // Start is called before the first frame update
    void Start()
    {
        wallHp = wallHp == 0 ?100:wallHp;
    }

    /**
     * �O�ǂ̃_���[�W�v�Z
     */
    public void calculateWallDamage(int attackValue)
    {
        //�j��s��
        if (isIndestructible)
        {
            GManager.instance.wrightLog("�j��s�\�I�u�W�F�N�g�ł��B");
            return;
        }
        wallHp -= attackValue;
        if (wallHp <= 0)
        {
            GManager.instance.wrightLog("�ǂ���ꂽ�B");
            Destroy(this.gameObject);
        }
        else
        {
            GManager.instance.wrightLog("�O�ǂ̎c��ϋv�l�F" + wallHp);
        }
    }
}
