using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : TileBase
{

    [Header("�񕜗�")] public int recoveryValue;
    private bool isColCounter = false;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�Փ˂����g���K�[�̃^�O��Exit�ł��邩�m�F���Ă��������B
        if (other.tag == "Player")
        {
            isColCounter = !isColCounter;
            if (isColCounter)
            {
                GManager.instance.recoveryHp(recoveryValue);
            }
        }
    }
}
