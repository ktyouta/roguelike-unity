using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallScript : MonoBehaviour
{
    [Header("�j��s�t���O")] public bool isIndestructible;
    [Header("�ǂ̑ϋv�l")] public int wallHp;
    [HideInInspector] public bool isAttacked = false;
    private int beforeWallHp;

    // Start is called before the first frame update
    void Start()
    {
        wallHp = wallHp == 0 ?100:wallHp;
        beforeWallHp = wallHp;
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̍U�������m�����ꍇ
        if (!isAttacked)
        {
            return;
        }
        isAttacked = false;
        //�ϋv�l�����������ꍇ
        if (beforeWallHp != wallHp)
        {
            GManager.instance.wrightLog("�O�ǂ̎c��ϋv�l�F"+wallHp);
            beforeWallHp = wallHp;
        }
        else
        {
            GManager.instance.wrightLog("�j��s�\�I�u�W�F�N�g�ł��B");
        }
        // �j��\�őϋv�l��0�ȉ�
        if (wallHp <= 0 )
        {
            GManager.instance.wrightLog("�ǂ���ꂽ�B");
            Destroy(this.gameObject);
        }
    }
}
