using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public abstract class NotAddItem : MonoBehaviour
{
    [Header("名前")] public string name;
    [Header("アイテムの説明")] public string itemDescription;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがFoodであるか確認してください。
        if (other.tag == "Player")
        {
            getItem();
            Destroy(this.gameObject);
        }
    }

    protected abstract void getItem();
}
