using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : TileBase
{

    [Header("回復量")] public int recoveryValue;
    private bool isColCounter = false;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがExitであるか確認してください。
        if (other.tag == "Player")
        {
            isColCounter = !isColCounter;
            if (isColCounter)
            {
                //GManager.instance.recoveryHp(recoveryValue);
                OutAccessComponentBase outAccessObj = other.transform.gameObject?.GetComponent<OutAccessComponentBase>();
                if (outAccessObj == null)
                {
                    return;
                }
                //回復処理
                outAccessObj.callCalculateRecoveryHp(recoveryValue, MessageManager.createMessage("2", outAccessObj.statusObj.charName.name, recoveryValue.ToString()));
            }
        }
    }
}
