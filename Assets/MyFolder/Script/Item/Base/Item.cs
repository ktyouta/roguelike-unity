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
    [HideInInspector] public bool isEnter = false;
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
    public abstract void useItem();

    /**
     * アイテムが衝突した場合の処理
     */
    public abstract void collisionItem(Enemy enemy);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        //出入り時に2回実行される対策
        isEnter = !isEnter;
        if (!isEnter)
        {
            return;
        }
        //アイテムを足元に置いたとき
        if (isPut)
        {
            isEnter = !isEnter;
            isPut = !isPut;
            return;
        }
        //所持制限のチェック
        if (GManager.instance.addItem(this.gameObject))
        {
            this.gameObject.SetActive(false);
        }
    }

    /**
     * アイテムのリストから該当するアイテムを削除する
     */
    public void deleteSelectedItem(int selectId)
    {
        for (int i = 0; i < GManager.instance.itemList.Count; i++)
        {
            if (GManager.instance.itemList[i].GetComponent<Item>().id == selectId)
            {
                GManager.instance.itemList.RemoveAt(i);
                this.gameObject.SetActive(false);
            }
        }
    }
}
