using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : TileBase
{
    [Header("消費量")] public int consumeValue;
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
                //GManager.instance.consumeFoodPoint(consumeValue);
            }
        }
    }
}
