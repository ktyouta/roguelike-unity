using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public abstract class Item : MonoBehaviour
{
    [Header("アイテムのタイプ")] public ItemTypeEnum type;
    [HideInInspector]public int id;
    [Header("名前")]public new string name;
    [Header("アイテムの説明")] public string itemDescription;
    [Header("ステージ難易度に応じたID")] public int diffId;
    [Header("買値")] public int buyPrice;
    [Header("売値")] public int sellPrice;
    [HideInInspector] public bool isPut = false;
    public GameObject itemPanelObj;
    public GameObject itemPanel;
    public GameObject commandPanel;
    public GameObject itemDescriptionPanel;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    /**
     * アイテムを使用
     */
    public abstract void useItem(StatusComponentBase statusObj);

    /**
     * アイテムが衝突した場合の処理
     */
    public abstract void collisionItem(GameObject targetObj);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        //アイテムを足元に置いたとき
        if (isPut)
        {
            return;
        }
        //所持制限のチェック
        if (ItemManager.addItem(this.gameObject))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPut = false;
        return;   
    }

    /**
     * アイテムのリストから該当するアイテムを削除する
     */
    public void deleteSelectedItem(int selectId)
    {
        for (int i = 0; i < ItemManager.itemList.Count; i++)
        {
            if (ItemManager.itemList[i].GetComponent<Item>().id == selectId)
            {
                ItemManager.itemList.RemoveAt(i);
                this.gameObject.SetActive(false);
            }
        }
    }
}
