using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public abstract class NotAddItem : MonoBehaviour
{
    [Header("���O")] public string name;
    [Header("�A�C�e���̐���")] public string itemDescription;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�Փ˂����g���K�[�̃^�O��Food�ł��邩�m�F���Ă��������B
        if (other.tag == "Player")
        {
            getItem();
            Destroy(this.gameObject);
        }
    }

    protected abstract void getItem();
}
