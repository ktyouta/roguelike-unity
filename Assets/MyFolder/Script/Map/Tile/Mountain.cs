using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : TileBase
{
    [Header("�����")] public int consumeValue;
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
                //GManager.instance.consumeFoodPoint(consumeValue);
            }
        }
    }
}
