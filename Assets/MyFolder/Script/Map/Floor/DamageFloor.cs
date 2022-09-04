using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    [Header("ダメージ量")] public int consumeValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        GManager.instance.damagePlayerHp(consumeValue);
        GManager.instance.wrightLog("フィールドダメージを受けた");
    }
}
