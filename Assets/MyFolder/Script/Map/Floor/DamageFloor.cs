using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    [Header("ダメージ量")] public int consumeValue;
    private bool isColCounter = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがExitであるか確認してください。
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
