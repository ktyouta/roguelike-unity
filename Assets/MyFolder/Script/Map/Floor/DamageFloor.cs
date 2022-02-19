using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    [Header("�_���[�W��")] public int consumeValue;
    private bool isColCounter = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�Փ˂����g���K�[�̃^�O��Exit�ł��邩�m�F���Ă��������B
        if (other.tag == "Player")
        {
            isColCounter = !isColCounter;
            if (isColCounter)
            {
                GManager.instance.damagePlayerHp(consumeValue);
            }
        }
    }
}
