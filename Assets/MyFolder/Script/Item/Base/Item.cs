using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector]public int id;
    [Header("名前")]public string name;
    [Header("アイテムのタイプ")]public string type;
    [Header("アイテムの説明")] public string itemDescription;
    [Header("ステージ難易度に応じたID")] public int diffId;
    [Header("買値")] public int buyPrice;
    [Header("売値")] public int sellPrice;
    [HideInInspector] public bool isUsedFlag = false;
    public GameObject itemPanelObj;
    public GameObject itemPanel;
    public GameObject commandPanel;
    public GameObject itemDescriptionPanel;
    [HideInInspector] public bool isEnter = false;
    [HideInInspector] public bool isPut = false;
    private player playerObj = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //itemObj = this.gameObject;
    }

    /**
     * アイテムを使用
     */
    public virtual void useItem()
    {
        itemPanelObj = GameObject.Find("ItemUseList");
        commandPanel = GameObject.Find("CommandPanel");
        itemPanel = GameObject.Find("ItemPanel");
        itemDescriptionPanel = GameObject.Find("ItemDescriptionPanel");
        itemPanelObj.SetActive(false);
        itemPanel.SetActive(false);
        commandPanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        GManager.instance.isMenuOpen = !GManager.instance.isMenuOpen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがFoodであるか確認してください。
        if (other.tag == "Player")
        {
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
            //アイテムの所持制限を超えている場合
            if (GManager.instance.itemList.Count > GManager.instance.nowMaxPosession)
            {
                GManager.instance.wrightInventoryFullLog();
                return;
            }
            //アイテム取得後、非表示
            addItemInventory();
        }
    }

    /**
     * プレイヤーのインベントリにアイテムを追加する
     */
    public void addItemInventory()
    {
        int itemId = GManager.instance.itemList.Count;
        GetComponent<Item>().id = itemId;
        GManager.instance.addItem(this.gameObject);
        this.gameObject.SetActive(false);
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
